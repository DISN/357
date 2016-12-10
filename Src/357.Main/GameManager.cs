using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Engine.System.Managers;
using Engine.System.Entities;
using System.IO;
using System.Reflection;
using System.Windows;
using Engine.System;
using MonoGame.Framework;

namespace Engine.Main
{
  /// <summary>
  /// This is the manager that loads and unloads the content needed for a game.
  /// </summary>
  public class GameManager : Microsoft.Xna.Framework.Game
  {
    #region Fields
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    GraphicsDevice _graphicsDevice;
    bool _isLoaded;
    private Vector2 _screenMousePos;
    #endregion

    #region Constructors
#if GameEditorMode
    public GameManager()
    {
      _graphics = new GraphicsDeviceManager(this);

      Content.RootDirectory = DefaultPaths.ContentPath;
    }
#else
    public GameManager()
      : base()
    {
      _graphics = new GraphicsDeviceManager(this);

      Content.RootDirectory = DefaultPaths.ContentPath;
      
      // Test here: Fullscreen borderless mode
      Window.IsBorderless = true;
    }
#endif
    #endregion

    #region Methods
    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content. Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // Test here: Fullscreen borderless mode
      //Window.Position = new Point(0, 0);

      // Set real screen resolution
      _graphics.PreferredBackBufferWidth = (int)SystemParameters.PrimaryScreenWidth;
      _graphics.PreferredBackBufferHeight = (int)SystemParameters.PrimaryScreenHeight;

      //ScreenManager.Instance.Dimensions = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); // Change the screen's size so we can align items
      //ScreenManager.Instance.Image.Scale = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); // Change the fade image's size

      _graphics.PreferMultiSampling = true;
      //_graphics.GraphicsDevice.BlendState = BlendState.Opaque;
      _graphics.GraphicsDevice.RasterizerState.MultiSampleAntiAlias = true;
      _graphics.GraphicsDevice.RasterizerState.CullMode = CullMode.None;
#if GameEditorMode
      _graphics.GraphicsDevice.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
#endif
      _graphics.ApplyChanges();

      Window.ClientSizeChanged += Window_ClientSizeChanged;
      base.Initialize();
    }

    void Window_ClientSizeChanged(object sender, EventArgs e)
    {
      //WinFormsGameWindow window = sender as WinFormsGameWindow;
      //InitializeResolutionIndependence((int)window.ClientBounds.Width, (int)window.ClientBounds.Height);
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      _graphicsDevice = GraphicsDevice;

      // Create a new SpriteBatch, which can be used to draw textures.
      _spriteBatch = new SpriteBatch(_graphicsDevice);

      // Load the player resources
      //ScreenManager.Instance.GraphicsDevice = _graphicsDevice;
      //ScreenManager.Instance.SpriteBatch = _spriteBatch;
      //ScreenManager.Instance.LoadContent(Content);

      // Load the camera resources
      if (!_isLoaded)
      {
        ScreenManager3D.Instance.GraphicsDevice = _graphicsDevice;
        ScreenManager3D.Instance.LoadContent(Content);

        //InitializeResolutionIndependence(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        //CameraManager.Instance.CurrentCamera.Zoom = 1f;
        //CameraManager.Instance.CurrentCamera.SetPosition(new Vector3(CameraManager.Instance.VirtualWidth / 2, CameraManager.Instance.VirtualHeight / 2, 0));

        _isLoaded = true;
      }
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      //ScreenManager.Instance.UnloadContent();
      ScreenManager3D.Instance.UnloadContent();
      Content.Unload();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      //ScreenManager.Instance.Update(gameTime);
      ScreenManager3D.Instance.Update(gameTime);
      _screenMousePos = CameraManager.Instance.ScaleMouseToScreenCoordinates(InputManager.Instance.MousePosition);

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      #region Resolution independance

      // Start by resetting viewport to (0,0,1,1)
      //CameraManager.Instance.SetupFullViewport();
      // Clear to Black
      GraphicsDevice.Clear(CameraManager.Instance.BackgroundColor);
      // Calculate Proper Viewport according to Aspect Ratio
      //CameraManager.Instance.SetupVirtualScreenViewport();
      // and clear that
      // This way we are gonna have black bars if aspect ratio requires it and
      // the clear color on the rest

      #endregion

      //begin sprite drawing using resolution transform matrix
      //all independent resolution objects must be drawn using this matrix for proper scaling and sizing
      //all coordinates must be specified in virtual screen resolution size range
      //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, CameraManager.Instance.GetTransformationMatrix());
      //ScreenManager.Instance.Draw(_spriteBatch);
      //_spriteBatch.End();
      ScreenManager3D.Instance.Draw();

      //reset screen viewport back to full size
      //so we can draw text from the TopLeft corner of the real screen
      //CameraManager.Instance.SetupFullViewport();
      //_spriteBatch.Begin();
      // insert stuff to draw here...
      //_spriteBatch.End();
      //CameraManager.Instance.SetupVirtualScreenViewport();

      base.Draw(gameTime);
    }

    private void InitializeResolutionIndependence(int realScreenWidth, int realScreenHeight)
    {
      CameraManager.Instance.VirtualWidth = (int)SystemParameters.PrimaryScreenWidth;
      CameraManager.Instance.VirtualHeight = (int)SystemParameters.PrimaryScreenHeight;
      CameraManager.Instance.ScreenWidth = realScreenWidth;
      CameraManager.Instance.ScreenHeight = realScreenHeight;
      CameraManager.Instance.Initialize();

      CameraManager.Instance.CurrentCamera.RecalculateTransformationMatrices();
    }

#if GameEditorMode
    public void Load()
    {
      this._graphics = Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
      if (this._graphics != null)
      {
        this._graphics.CreateDevice();
      }
      this.Initialize();
      _graphicsDevice = this._graphics.GraphicsDevice;

      // Create a new SpriteBatch, which can be used to draw textures.
      _spriteBatch = new SpriteBatch(_graphicsDevice);

      // Load the player resources
      //ScreenManager.Instance.GraphicsDevice = _graphicsDevice;
      //ScreenManager.Instance.SpriteBatch = _spriteBatch;
      //ScreenManager.Instance.LoadContent(Content);

      // Load the camera resources
      if (!_isLoaded)
      {
        ScreenManager3D.Instance.GraphicsDevice = _graphicsDevice;
        ScreenManager3D.Instance.LoadContent(Content);

        //CameraManager.Instance.CurrentCamera.Zoom = 1f;
        //CameraManager.Instance.CurrentCamera.SetPosition(new Vector3((float)CameraManager.Instance.VirtualWidth / 2, (float)CameraManager.Instance.VirtualHeight / 2, 0f));
        //InitializeResolutionIndependence(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

        _isLoaded = true;
      }
    }

    public void Unload()
    {
      UnloadContent();
      _isLoaded = false;
    }

    public void Reload()
    {
      //ScreenManager.Instance.ReloadContent(_graphicsDevice);
      ScreenManager3D.Instance.ReloadContent();
    }

    public void Close()
    {
      ModelManager.Instance.Models.Clear();
    }

    public void Update(TimeSpan totalGameTime, TimeSpan elapsedTime, KeyboardState keyboardState)
    {
      Update(new GameTime(totalGameTime, elapsedTime), keyboardState);
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected void Update(GameTime gameTime, KeyboardState keyboardState)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      //ScreenManager.Instance.Update(gameTime, keyboardState);
      ScreenManager3D.Instance.Update(gameTime, keyboardState);
      _screenMousePos = CameraManager.Instance.ScaleMouseToScreenCoordinates(InputManager.Instance.MousePosition);

      base.Update(gameTime);
    }

    public void Draw(TimeSpan totalGameTime, TimeSpan elapsedTime)
    {
      Draw(new GameTime(totalGameTime, elapsedTime));
    }

    /// <summary>
    /// This is what we'll want to call to draw the player and once we Reload (press Stop on the editor), we basically only want to reload the entities/simulation (not the whole world)
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void DrawPlayer(SpriteBatch spriteBatch)
    {
      //ScreenManager.Instance.DrawPlayer(spriteBatch);
    }

    /// <summary>
    /// Actually calculates the new horizontal field of view for the projection camera. The Aspect Ratio parameter that we pass is actually the size of the viewer.
    /// </summary>
    /// <param name="aspectRatio"></param>
    public void ToggleRatio(float aspectRatio, float viewerWidth, float viewerHeight)
    {
      // Calculate new horizontal field of view
      float newHFoVAngle = Math.Abs((float)(2.0 * (double)(Math.Atan((double)Math.Tan((double)CameraManager.Instance.CurrentCamera.VFovAngle / 2) * (double)aspectRatio))));

      // Gets the viewer's actual aspect ratio
      float viewerAspectRatio = viewerWidth > viewerHeight ? (float)(viewerWidth / viewerHeight) : (float)(viewerHeight / viewerWidth);
      CameraManager.Instance.CurrentCamera.AspectRatio = viewerAspectRatio;

      _graphics.PreferredBackBufferWidth = (int)viewerWidth;
      _graphics.PreferredBackBufferHeight = (int)viewerHeight;
      _graphics.ApplyChanges();

      CameraManager.Instance.CurrentCamera.ProjectionMatrix.CreatePerspectiveFieldOfView(newHFoVAngle,
                                                                                         CameraManager.Instance.CurrentCamera.AspectRatio,
                                                                                         CameraManager.Instance.CurrentCamera.Near,
                                                                                         CameraManager.Instance.CurrentCamera.Far);
    }

    public void ToggleSolid()
    {
      RasterizerState rasterizerState = new RasterizerState();
      rasterizerState.FillMode = FillMode.Solid;
      _graphics.GraphicsDevice.RasterizerState = rasterizerState;
    }

    public void ToggleWireframe()
    {
      RasterizerState rasterizerState = new RasterizerState();
      rasterizerState.FillMode = FillMode.WireFrame;
      _graphics.GraphicsDevice.RasterizerState = rasterizerState;
    }
#endif
    #endregion
  }
}
