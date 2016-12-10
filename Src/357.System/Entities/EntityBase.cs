using Engine.System.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Entities
{
  #if GameEditorMode
  public class EntityBase : ISaveable
  {
#else
  public class EntityBase
  {
#endif
    #region Fields
    private Vector2 _velocity;

#if GameEditorMode
    bool _isDirty;
    private List<ISaveable> _objectsToSave;
#endif
    #endregion

    #region Constructors
    public EntityBase()
    {
      _velocity = Vector2.Zero;
    }
    #endregion

    #region Properties
    public Image Image { get; set; }

    public Vector2 Velocity
    {
      get { return _velocity; }
      set
      {
        if (_velocity != value)
          _velocity = value;
      }
    }
    public float X
    {
      get { return _velocity.X; }
      set
      {
        if (_velocity.X != value)
          _velocity.X = value;
      }
    }
    public float Y
    {
      get { return _velocity.Y; }
      set
      {
        if (_velocity.Y != value)
          _velocity.Y = value;
      }
    }

    public float MoveSpeed { get; set; }

    public float Health { get; set; }

    public float Gravity { get; set; }

    public Vector2 PreviousPosition { get; set; }

    public bool ActivateGravity { get; set; }

    public bool SyncTilePosition { get; set; }
    #endregion

    #region Methods
    public virtual void LoadContent()
    {
      Image.LoadContent();
    }

    public virtual void UnloadContent()
    {
      Image.UnloadContent();
    }

    public virtual void Update(GameTime gameTime)
    {
      Image.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      Image.Draw(spriteBatch);
    }

#if GameEditorMode
    public void LoadContent(GraphicsDevice graphicsDevice)
    {
      Image.LoadContent(graphicsDevice);
    }

    public void UnloadContentEditor()
    {
      Image.UnloadContentEditor();
    }

    public void DrawPlayer(SpriteBatch spriteBatch)
    {
      Image.Draw(spriteBatch);
    }
#endif
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
