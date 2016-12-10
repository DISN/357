using Engine.System.Enums;
using Engine.System.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Managers
{
  public class MenuManager
  {
    #region Fields
    Menu _menu;
    bool _isTransitioning;
    #endregion

    #region Constructors
    public MenuManager()
    {
      _menu = new Menu();
      _menu.OnMenuChange += _menu_OnMenuChange;
    }
    #endregion

    #region Methods
    public void LoadContent(string menuPath)
    {
      if (menuPath != String.Empty)
        _menu.ID = menuPath;
    }

    public void UnloadContent()
    {
      _menu.UnloadContent();
    }

    public void Update(GameTime gameTime)
    {
      if (!_isTransitioning)
        _menu.Update(gameTime);

      if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Space) && !_isTransitioning)
      {
        if (_menu.Items[_menu.ItemNumber].LinkType == LinkType.Screen)
          ScreenManager.Instance.ChangeScreen(_menu.Items[_menu.ItemNumber].LinkID);
        else
        {
          _isTransitioning = true;
          _menu.Transition(1.0f);
          foreach (MenuItem item in _menu.Items)
          {
            item.Image.StoreEffects();
            item.Image.ActivateEffect("FadeEffect"); // default effect
          }
        }
      }
      Transition(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _menu.Draw(spriteBatch);
    }

    void Transition(GameTime gameTime)
    {
      if (_isTransitioning)
      {
        for (int i = 0; i < _menu.Items.Count; i++)
        {
          _menu.Items[i].Image.Update(gameTime);

          float firstItemAlpha = _menu.Items[0].Image.Alpha;
          float lastItemAlpha = _menu.Items[_menu.Items.Count - 1].Image.Alpha;

          if (firstItemAlpha == 0.0f && lastItemAlpha == 0.0f)
            _menu.ID = _menu.Items[_menu.ItemNumber].LinkID;
          else if (firstItemAlpha == 1.0f && lastItemAlpha == 1.0f)
          {
            _isTransitioning = false;
            foreach (MenuItem item in _menu.Items)
              item.Image.RestoreEffects();
          }
        }
      }
    }
    #endregion

    #region Events
    private void _menu_OnMenuChange(object sender, EventArgs e)
    {
      XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
      _menu.UnloadContent();
      _menu = xmlMenuManager.Load(_menu.ID);
      _menu.LoadContent();
      _menu.OnMenuChange += _menu_OnMenuChange; //since we have a brand new menu, we need to reset this event handler
      _menu.Transition(0.0f); //we reset all the alphas to 0

      foreach (MenuItem item in _menu.Items)
      {
        item.Image.StoreEffects();
        item.Image.ActivateEffect("FadeEffect"); // default effect
      }
    }
    #endregion
  }
}
