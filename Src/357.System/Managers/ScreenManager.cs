using Engine.System.Entities;
using Engine.System.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Managers
{
  public class ScreenManager
  {
    #region Fields
    private static ScreenManager _instance;
    private XmlManager<ScreenBase> _xmlScreenManager;
    private ScreenBase _currentScreen, _newScreen;
    private SpriteFont _font; // used to show debug information
    private bool _showDebug;

    /// <summary>
    /// Screen stack that will allow us to go back to previous screens
    /// </summary>
    Stack<ScreenBase> _screenStack;
    Vector2 _dimensions;
    #endregion

    #region Constructors
    public ScreenManager()
    {
      _screenStack = new Stack<ScreenBase>();
      FontName = "Fonts/Verdana";
      Dimensions = new Vector2(640, 480);
      _showDebug = false;
#if GameEditorMode
      _currentScreen = new GameplayScreen();
#else
      _currentScreen = new SplashScreen();
#endif
      _xmlScreenManager = new XmlManager<ScreenBase>();
      _xmlScreenManager.Type = _currentScreen.Type;
#if GameEditorMode
#else
      _currentScreen = _xmlScreenManager.Load("../../../Revolver/Editor/Debug/Content/XML/SplashScreen.xml");
#endif
    }
    #endregion

    #region Properties
    public static ScreenManager Instance
    {
      get
      {
        if (_instance == null)
        {
          XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
          _instance = xml.Load("../../../Revolver/Editor/Debug/Content/XML/ScreenManager.xml");
        }
        return _instance;
      }
    }

    public Vector2 Dimensions
    {
      get { return _dimensions; }
      set
      {
        _dimensions = value;
      }
    }

    public Image Image { set; get; }

    public string FontName { set; get; }

    [XmlIgnore]
    public bool ShowDebug
    {
      get { return _showDebug; }
      set { _showDebug = value; }
    }

    [XmlIgnore]
    public ContentManager Content { private set; get; }

    [XmlIgnore]
    public GraphicsDevice GraphicsDevice { set; get; }

    [XmlIgnore]
    public SpriteBatch SpriteBatch { set; get; }

    [XmlIgnore]
    public bool IsTransitioning { private set; get; }
    
    #endregion

    #region Methods
    /// <summary>
    /// LoadContent will be called to load the content on the current screen.
    /// </summary>
    public void LoadContent(ContentManager content)
    {
      this.Content = new ContentManager(content.ServiceProvider, "../../../Revolver/Editor/Debug/Content");
      _currentScreen.LoadContent();
      Image.LoadContent();
      _font = content.Load<SpriteFont>(FontName);
    }

    /// <summary>
    /// UnloadContent will be called to unload the content on the current screen
    /// </summary>
    public void UnloadContent()
    {
      _currentScreen.UnloadContent();
      Image.UnloadContent();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime)
    {
      _currentScreen.Update(gameTime);
      Transition(gameTime);
    }

    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
      _currentScreen.Draw(spriteBatch);
      if (IsTransitioning)
        Image.Draw(spriteBatch);
      if (ShowDebug)
        spriteBatch.DrawString(_font, GraphicsDevice.DisplayMode.RefreshRate.ToString(), new Vector2(0, 0), Color.White);
    }

    /// <summary>
    /// Changes the current screen to the given screen given in parameter. Activates a FadeEffect by default.
    /// </summary>
    /// <param name="screenName">The screen we want to change to.</param>
    public void ChangeScreen(string screenName)
    {
      _newScreen = (ScreenBase)Activator.CreateInstance(Type.GetType("Engine.System.Screens." + screenName));
      _screenStack.Push(_newScreen);

      Image.IsActive = true;
      Image.FadeEffect.Increase = true;
      Image.Alpha = 0.0f; //image is not fully shown
      IsTransitioning = true;
    }

    private void Transition(GameTime gameTime)
    {
      if (IsTransitioning)
      {
        Image.Update(gameTime);
        if (Image.Alpha == 1.0f) //when the image is fully shown
        {
          _currentScreen.UnloadContent();
          _currentScreen = _newScreen;
          _xmlScreenManager.Type = _currentScreen.Type;
          if (File.Exists(_currentScreen.XMLPath))
            _currentScreen = _xmlScreenManager.Load(_currentScreen.XMLPath);
          _currentScreen.LoadContent();
        }
        else if (Image.Alpha == 0.0f)
        {
          Image.IsActive = false;
          IsTransitioning = false;
        }
      }
    }

#if GameEditorMode
    /// <summary>
    /// LoadContent will be called to load the content on the current screen.
    /// </summary>
    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
      this.Content = new ContentManager(content.ServiceProvider, "../../../Revolver/Editor/Debug/Content");
      _currentScreen.LoadContent(graphicsDevice);
      Image.LoadContent(graphicsDevice);
    }
    
    /// <summary>
    /// UnloadContent will be called to unload the content on the current screen.
    /// </summary>
    public void UnloadContentEditor()
    {
      _currentScreen.UnloadContentEditor();
      Image.UnloadContentEditor();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
    {
      _currentScreen.Update(gameTime, keyboardState, gamePadState, mouseState);
      Transition(gameTime);
    }
    
    /// <summary>
    /// LoadContent will be called to load the content on the current screen.
    /// </summary>
    public void ReloadContent(GraphicsDevice graphicsDevice)
    {
      _currentScreen.Reload(graphicsDevice);
    }

    public void DrawPlayer(SpriteBatch spriteBatch)
    {
      _currentScreen.DrawPlayer(spriteBatch);
    }
#endif
    #endregion
  }
}
