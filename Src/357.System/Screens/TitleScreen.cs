using Engine.System.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Screens
{
  public class TitleScreen : ScreenBase
  {
    #region Fields
    private MenuManager _menuManager;
    #endregion

    #region Constructors
    public TitleScreen()
    {
      _menuManager = new MenuManager();
    }
    #endregion

    #region Methods
    public override void LoadContent()
    {
      base.LoadContent();
      _menuManager.LoadContent(DefaultPaths.XmlPath + "TitleMenu.xml");
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
      _menuManager.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      _menuManager.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      _menuManager.Draw(spriteBatch);
    }
    #endregion
  }
}
