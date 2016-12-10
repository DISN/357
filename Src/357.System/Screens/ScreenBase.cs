using Engine.System.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml.Serialization;

namespace Engine.System.Screens
{
  [Serializable]
  public class ScreenBase
  {
    #region Fields
    protected ContentManager _content;

#if GameEditorMode
    [XmlIgnore]
    public GraphicsDevice _graphicsDevice;
#endif
    #endregion

    #region Constructors
    public ScreenBase()
    {
      Type = this.GetType();
      XMLPath = DefaultPaths.XmlPath + Type.ToString().Replace("Engine.System.Screens.", "") + ".xml";
    }
    #endregion

    #region Properties
    [XmlIgnore]
    public Type Type;

    public string XMLPath;
    #endregion

    #region Methods
    public virtual void LoadContent()
    {
      _content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, DefaultPaths.ContentPath);
    }

    public virtual void UnloadContent()
    {
      _content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
      InputManager.Instance.Update();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }
    #endregion

#if GameEditorMode
    public virtual void LoadContent(GraphicsDevice graphicsDevice)
    {
      this._graphicsDevice = graphicsDevice;
    }

    public virtual void UnloadContentEditor()
    {
    }

    public virtual void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
    {
      InputManager.Instance.Update(keyboardState, gamePadState, mouseState);
    }

    public virtual void Reload(GraphicsDevice graphicsDevice)
    {
    }

    public virtual void DrawPlayer(SpriteBatch spriteBatch)
    {
    }
#endif
  }
}
