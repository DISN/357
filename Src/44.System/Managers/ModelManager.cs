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
  public class ModelManager
  {
    #region Fields
    List<ModelBase> _models;
    private static ModelManager _instance;
    #endregion

    #region Constructors
    public ModelManager()
    {
      _models = new List<ModelBase>();
    }
    #endregion

    #region Properties
    public static ModelManager Instance
    {
      get
      {
        if (_instance == null)
        {
          XmlManager<ModelManager> xml = new XmlManager<ModelManager>();
          _instance = xml.Load(DefaultPaths.ModelManagerPath);
        }
        return _instance;
      }
    }

    [XmlIgnore]
    public ContentManager Content { private set; get; }

    [XmlIgnore]
    public GraphicsDevice GraphicsDevice { get { return (Content.ServiceProvider.GetService((typeof(IGraphicsDeviceManager))) as GraphicsDeviceManager).GraphicsDevice; } }

    public List<ModelBase> Models
    {
      get { return _models; }
      set
      {
        _models = value;
      }
    }
    #endregion

    #region Methods
    /// <summary>
    /// LoadContent will be called to load the content on the current loaded models.
    /// </summary>
    public void LoadContent(ContentManager content)
    {
      this.Content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath);

      //if (ModelManager.Instance.Models.Count == 1)
        //ModelManager.Instance.AddModel(new Triangle());
      foreach (ModelBase model in Models)
      {
        model.LoadContent(content);
        model.Load(ScreenManager3D.Instance.GraphicsDevice);
      }
    }

    /// <summary>
    /// UnloadContent will be called to unload the content on the current loaded models.
    /// </summary>
    public void UnloadContent()
    {
      foreach (ModelBase model in Models)
        model.UnloadContent();
      this.Content.Unload();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the current loaded models' positions.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime)
    {
      foreach (ModelBase model in Models)
        model.Update(gameTime);
    }

    /// <summary>
    /// This is called when the current loaded models should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Draw(WorldMatrix worldMatrix, ViewMatrix viewMatrix, ProjectionMatrix projectionMatrix)
    {
      foreach (ModelBase model in Models)
        model.Draw(worldMatrix, viewMatrix, projectionMatrix);
    }

    public void AddModel(ModelBase modelToAdd)
    {
      Models.Add(modelToAdd);
    }

#if GameEditorMode
    public void Reload()
    {
      Models.Clear();
      XmlManager<ModelManager> xml = new XmlManager<ModelManager>();
      _instance = xml.Load(DefaultPaths.ModelManagerPath);
    }
#endif
    #endregion
  }
}
