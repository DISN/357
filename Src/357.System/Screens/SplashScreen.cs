using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Engine.System.Entities;
using Engine.System.Managers;
using Microsoft.Xna.Framework.Input;

namespace Engine.System.Screens
{
  [Serializable]
  public class SplashScreen : ScreenBase
  {
    #region Fields
    private TimeSpan _elapsedTime;
    private TimeSpan _maxTime = TimeSpan.FromSeconds(3);
    #endregion

    #region Properties
    [XmlElement("Image")]
    public Image Image;
    #endregion

    #region Methods
    public override void LoadContent()
    {
      base.LoadContent();
      Image.LoadContent();
      AlignImage();
    }

    private void AlignImage()
    {
      Vector2 dimensions = Vector2.Zero;
      dimensions = new Vector2(Image.SourceRect.Width, Image.SourceRect.Height);

      // Center our image
      dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X) / 2, (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2);

      // Align it
      Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - Image.SourceRect.Width) / 2, dimensions.Y);
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
      Image.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      Image.Update(gameTime);

      _elapsedTime += gameTime.ElapsedGameTime;

      if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Z) || _elapsedTime >= _maxTime && !ScreenManager.Instance.IsTransitioning)
        ScreenManager.Instance.ChangeScreen("TitleScreen");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      Image.Draw(spriteBatch);
    }

#if GameEditorMode
    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
      base.LoadContent();
      Image.LoadContent(graphicsDevice);
    }
#endif
    #endregion
  }
}
