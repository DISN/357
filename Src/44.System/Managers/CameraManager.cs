using Engine.System.Cameras;
using Engine.System.Matrices;
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
using System.Windows;
using System.Xml.Serialization;

namespace Engine.System.Managers
{
  public class CameraManager
  {
    #region Fields
    private Viewport _viewport;
    private float _ratioX;
    private float _ratioY;
    private Vector2 _virtualMousePosition = new Vector2();

    public Color BackgroundColor = Color.Black;

    public bool RenderingToScreenIsFinished;
    private static Matrix _scaleMatrix;
    private bool _dirtyMatrix = true;
    bool _invertScale = false;

    private static CameraManager _instance;
    private XmlManager<CameraBase> _xmlCameraManager;
    private CameraBase _currentCamera, _newCamera;
    #endregion

    #region Constructors
    public CameraManager()
    {
      VirtualWidth = (int)SystemParameters.PrimaryScreenWidth;
      VirtualHeight = (int)SystemParameters.PrimaryScreenHeight;
      ScreenWidth = VirtualWidth;
      ScreenHeight = VirtualHeight;

      _currentCamera = new ProjectionCamera();

      _xmlCameraManager = new XmlManager<CameraBase>();
      _xmlCameraManager.Type = _currentCamera.Type;

      _xmlCameraManager.Load(DefaultPaths.ProjectionCameraPath);
    }
    #endregion

    #region Properties
    public static CameraManager Instance
    {
      get
      {
        if (_instance == null)
        {
          XmlManager<CameraManager> xml = new XmlManager<CameraManager>();
          _instance = xml.Load(DefaultPaths.CameraManagerPath);
        }
        return _instance;
      }
    }

    [XmlIgnore]
    public ContentManager Content { private set; get; }

    [XmlIgnore]
    public GraphicsDevice GraphicsDevice { get { return (Content.ServiceProvider.GetService((typeof(IGraphicsDeviceManager))) as GraphicsDeviceManager).GraphicsDevice; } }

    [XmlIgnore]
    public bool IsTransitioning { private set; get; }

    [XmlIgnore]
    public CameraBase CurrentCamera
    {
      get { return _currentCamera; }
      set { _currentCamera = value; }
    }

    [XmlIgnore]
    public int VirtualHeight;

    [XmlIgnore]
    public int VirtualWidth;

    [XmlIgnore]
    public int ScreenWidth;

    [XmlIgnore]
    public int ScreenHeight;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes resolution renderer and marks it for refresh
    /// </summary>
    public void Initialize()
    {
      SetupVirtualScreenViewport();

      _ratioX = (float)_viewport.Width / VirtualWidth;
      _ratioY = (float)_viewport.Height / VirtualHeight;

      _dirtyMatrix = true;
    }

    /// <summary>
    /// Setup viewport to real screen size
    /// </summary>
    public void SetupFullViewport()
    {
      var vp = new Viewport();
      vp.X = vp.Y = 0;
      vp.Width = ScreenWidth;
      vp.Height = ScreenHeight;
      GraphicsDevice.Viewport = vp;
      _dirtyMatrix = true;
    }

    /// <summary>
    /// LoadContent will be called to load the content on the current camera.
    /// </summary>
    public void LoadContent(ContentManager content)
    {
      this.Content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath);
      CurrentCamera.LoadContent(content);
    }

    /// <summary>
    /// UnloadContent will be called to unload the content on the current camera.
    /// </summary>
    public void UnloadContent()
    {
      CurrentCamera.UnloadContent();
      this.Content.Unload();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the camera's position.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime)
    {
      CurrentCamera.Update(gameTime);
      Transition(gameTime);
    }

    /// <summary>
    /// This is called when the camera should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Draw()
    {
      CurrentCamera.Draw();
    }

    /// <summary>
    /// Changes the current camera to the given camera given in parameter. Doesn't activate an effect by default (yet).
    /// </summary>
    /// <param name="screenName">The camera we want to change to.</param>
    public void ChangeCamera(string cameraName)
    {
      _newCamera = (CameraBase)Activator.CreateInstance(Type.GetType("Engine.System.Cameras." + cameraName));

      IsTransitioning = true;
    }

    private void Transition(GameTime gameTime)
    {
      if (IsTransitioning)
      {
        CurrentCamera.UnloadContent();
        CurrentCamera = _newCamera;
        _xmlCameraManager.Type = CurrentCamera.Type;
        if (File.Exists(CurrentCamera.XMLPath))
          CurrentCamera = _xmlCameraManager.Load(CurrentCamera.XMLPath);
        CurrentCamera.LoadContent(this.Content);
        IsTransitioning = false;
      }
    }

    public Matrix GetTransformationMatrix()
    {
      if (_dirtyMatrix)
        RecreateScaleMatrix();

      return _scaleMatrix;
    }

    private void RecreateScaleMatrix()
    {
      if (!_invertScale)
        Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out _scaleMatrix);
      else
        Matrix.CreateScale((float)ScreenHeight / VirtualHeight, (float)ScreenHeight / VirtualHeight, 1f, out _scaleMatrix);
      _dirtyMatrix = false;
    }

    public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
    {
      var realX = screenPosition.X - _viewport.X;
      var realY = screenPosition.Y - _viewport.Y;

      _virtualMousePosition.X = realX / _ratioX;
      _virtualMousePosition.Y = realY / _ratioY;

      return _virtualMousePosition;
    }

    public void SetupVirtualScreenViewport()
    {
      var targetAspectRatio = VirtualWidth / (float)VirtualHeight;
      // figure out the largest area that fits in this resolution at the desired aspect ratio
      var width = ScreenWidth;
      var height = (int)(width / targetAspectRatio + .5f);

      if (height > ScreenHeight)
      {
        _invertScale = true;
        height = ScreenHeight;
        // PillarBox
        width = (int)(height * targetAspectRatio + .5f);
      }
      else
        _invertScale = false;

      // set up the new viewport centered in the backbuffer
      GraphicsDevice.Viewport = new Viewport
      {
        X = (ScreenWidth / 2) - (width / 2),
        Y = (ScreenHeight / 2) - (height / 2),
        Width = width,
        Height = height
      };
    }

#if GameEditorMode
    public void Zoom(float delta)
    {
      CurrentCamera.CameraPosition = new Vector3(CurrentCamera.CameraPosition.X, CurrentCamera.CameraPosition.Y, CurrentCamera.CameraPosition.Z - delta);
      CurrentCamera.ViewMatrix.CreateLookAt(CurrentCamera.CameraPosition, CurrentCamera.CameraTarget);
    }
#endif
    #endregion
  }
}
