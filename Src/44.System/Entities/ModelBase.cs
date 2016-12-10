using Engine.System.Interfaces;
using Engine.System.Managers;
using Engine.System.Matrices;
using Engine.System.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Entities
{
#if GameEditorMode
  public class ModelBase : ISaveable
  {
#else
  public class ModelBase
  {
#endif
    #region Fields
    protected ContentManager _content;

    Model _loadedModel;

    string _modelName;

    Vector3 _position;

    float _rotationX;
    float _rotationY;
    float _rotationZ;

    private List<StateBase> _states;

#if GameEditorMode
    bool _isDirty;
    private List<ISaveable> _objectsToSave;
#endif
    #endregion

    #region Constructors

    public ModelBase(string modelName = "")
      : base()
    {
      _modelName = modelName;
    }

    public ModelBase()
    {
      _loadedModel = new Model();
      _modelName = String.Empty;
      _position = Vector3.Zero;
      _rotationX = 0.0f;
      _rotationY = 0.0f;
      _rotationZ = 0.0f;
      _states = new List<StateBase>();
    }

    #endregion

    #region Properties
    [XmlIgnore]
    public GraphicsDevice GraphicsDevice { get { return (ModelManager.Instance.Content.ServiceProvider.GetService((typeof(IGraphicsDeviceManager))) as GraphicsDeviceManager).GraphicsDevice; } }

    [XmlIgnore]
    public Model LoadedModel
    {
      get { return _loadedModel; }
      set
      {
        _loadedModel = value;
      }
    }

    public string ModelName
    {
      get { return _modelName; }
      set
      {
        _modelName = value;
      }
    }

    public Vector3 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public float RotationX
    {
      get { return _rotationX; }
      set { _rotationX = value; }
    }

    public float RotationY
    {
      get { return _rotationY; }
      set { _rotationY = value; }
    }

    public float RotationZ
    {
      get { return _rotationZ; }
      set { _rotationZ = value; }
    }

    public List<StateBase> States
    {
      get { return _states; }
      set { _states = value; }
    }
    #endregion

    #region Methods
    public void Load(string modelToLoad)
    {
      _loadedModel = _content.Load<Model>(modelToLoad);
    }

    public void Load()
    {
      _loadedModel = _content.Load<Model>(ModelName);
    }

    public virtual void Load(GraphicsDevice graphicsDevice)
    {
      try
      {
        if (!String.IsNullOrEmpty(ModelName))
          _loadedModel = _content.Load<Model>(ModelName);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public virtual void LoadContent(ContentManager content)
    {
      _content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath + "/Models/");
    }

    public virtual void UnloadContent()
    {
      if (_loadedModel != null && _loadedModel.Meshes != null)
      {
        foreach (ModelMesh mesh in _loadedModel.Meshes)
        {
          foreach (BasicEffect effect in mesh.Effects)
          {
            effect.Dispose();
          }
        }
        _loadedModel = null;
      }
      _content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
      float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (InputManager.Instance.IsCurPress(Keys.Left) ||
          InputManager.Instance.IsCurPress(Buttons.LeftThumbstickLeft))
      {
        RotationY -= 0.02f;
      }
      if (InputManager.Instance.IsCurPress(Keys.Right) ||
              InputManager.Instance.IsCurPress(Buttons.LeftThumbstickRight))
      {
        RotationY += 0.02f;
      }
      if (InputManager.Instance.IsCurPress(Keys.Up) ||
              InputManager.Instance.IsCurPress(Buttons.LeftThumbstickUp))
      {
        RotationX -= 0.02f;
      }
      if (InputManager.Instance.IsCurPress(Keys.Down) ||
              InputManager.Instance.IsCurPress(Buttons.LeftThumbstickDown))
      {
        RotationX += 0.02f;
      }

      //RotationX += deltaTime * 2;
      RotationY += deltaTime;

      // Call update on all states
      _states.ForEach(x => x.Update(gameTime));
    }

    public virtual void Draw(WorldMatrix world, ViewMatrix view, ProjectionMatrix projection)
    {
      if (_loadedModel.Meshes != null)
      {
        foreach (ModelMesh mesh in _loadedModel.Meshes)
        {
          foreach (BasicEffect effect in mesh.Effects)
          {
            effect.World = world.TransformedMatrix * Matrix.CreateRotationY(RotationY) * Matrix.CreateRotationX(RotationX) * Matrix.CreateTranslation(Position);
            effect.View = view.TransformedMatrix;
            effect.Projection = projection.TransformedMatrix;
            effect.TextureEnabled = true;
          }

          mesh.Draw();
        }
      }
    }

    public void DrawModel()
    {

    }
    #endregion

#if GameEditorMode
    #region ISaveable Members
    public event EventHandler OnIsDirtyChanged;

    [XmlIgnore]
    public bool IsDirty
    {
      get
      {
        return _isDirty;
      }
      set
      {
        _isDirty = value;

        var handler = OnIsDirtyChanged;
        if (handler != null)
          handler(this, null);
      }
    }

    [XmlIgnore]
    public List<ISaveable> ObjectsToSave
    {
      get
      {
        return _objectsToSave;
      }
      set
      {
        _objectsToSave = value;
      }
    }

    public string Name { get; set; }
    #endregion
#endif
  }
}
