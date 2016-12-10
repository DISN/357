using Engine.System.Cameras;
using Engine.System.Entities;
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
using System.Xml.Serialization;

namespace Engine.System.Managers
{
  public class ScreenManager3D
  {
    #region Fields
    private static ScreenManager3D _instance;
    #endregion

    #region Constructors
    public ScreenManager3D()
    {
    }
    #endregion

    #region Properties
    public static ScreenManager3D Instance
    {
      get
      {
        if (_instance == null)
        {
          XmlManager<ScreenManager3D> xml = new XmlManager<ScreenManager3D>();
          _instance = xml.Load(DefaultPaths.ScreenManager3DPath);
        }
        return _instance;
      }
    }

    [XmlIgnore]
    public ContentManager Content { private set; get; }

    [XmlIgnore]
    //public GraphicsDevice GraphicsDevice { get { return (Content.ServiceProvider.GetService((typeof(IGraphicsDeviceManager))) as GraphicsDeviceManager).GraphicsDevice; } }
    public GraphicsDevice GraphicsDevice { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// LoadContent will be called to load the content on the current camera.
    /// </summary>
    public void LoadContent(ContentManager content)
    {
      this.Content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath);
      CameraManager.Instance.LoadContent(content);

      ModelManager.Instance.LoadContent(content);
    }

    /// <summary>
    /// UnloadContent will be called to unload the content on the current camera.
    /// </summary>
    public void UnloadContent()
    {
      CameraManager.Instance.UnloadContent();
      ModelManager.Instance.UnloadContent();
      this.Content.Unload();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the camera's position.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime)
    {
      CameraManager.Instance.Update(gameTime);
      ModelManager.Instance.Update(gameTime);
    }

    /// <summary>
    /// This is called when the camera should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Draw()
    {
      CameraManager.Instance.Draw();
      ModelManager.Instance.Draw(CameraManager.Instance.CurrentCamera.WorldMatrix, CameraManager.Instance.CurrentCamera.ViewMatrix, CameraManager.Instance.CurrentCamera.ProjectionMatrix);
    }

#if GameEditorMode
    /// <summary>
    /// Allows the game to run logic such as updating the camera's position.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
      CameraManager.Instance.Update(gameTime);
      ModelManager.Instance.Update(gameTime);
    }

    /// <summary>
    /// LoadContent will be called to load the content on the current screen.
    /// </summary>
    public void ReloadContent()
    {
      ModelManager.Instance.Reload();
      ModelManager.Instance.LoadContent(this.Content);
    }
#endif
    #endregion
  }
}
