using Engine.System.Entities;
using Engine.System.Enums;
using Engine.System.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Screens
{
  public class GameplayScreen : ScreenBase
  {
    #region Fields
    private Player _player;
    private Map _map;
    #endregion

    #region Methods
    public override void LoadContent()
    {
      base.LoadContent();

      XmlManager<Player> playerLoader = new XmlManager<Player>();
      XmlManager<Map> mapLoader = new XmlManager<Map>();
      _player = playerLoader.Load(DefaultPaths.DefaultPlayerPath);
      _map = mapLoader.Load(DefaultPaths.DefaultMapPath);
      _player.LoadContent();
      _map.LoadContent();
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
      _player.UnloadContent();
      _map.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      _player.Update(gameTime); //we want to calculate collisions first, based on input, etc.
      _map.Update(gameTime, ref _player);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      _map.Draw(spriteBatch, DrawType.Underlay); //we draw the map first
      _player.Draw(spriteBatch);
      _map.Draw(spriteBatch, DrawType.Overlay);
    }

#if GameEditorMode
    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
      base.LoadContent();

      XmlManager<Player> playerLoader = new XmlManager<Player>();
      XmlManager<Map> mapLoader = new XmlManager<Map>();

      // TODO: Load a desired Player / Map instead of hardcoded. We want to calculate collisions.
      _player = playerLoader.Load(DefaultPaths.DefaultPlayerPath);
      //_map = mapLoader.Load(Global._xmlPath + Global._defaultMap);
      _player.LoadContent(graphicsDevice);
      //_map.LoadContent(graphicsDevice);
    }

    public override void UnloadContentEditor()
    {
      base.UnloadContentEditor();
      _player.UnloadContentEditor();
      //_map.UnloadContentEditor();
    }

    public override void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
    {
      base.Update(gameTime, keyboardState, gamePadState, mouseState);
      _player.Update(gameTime); //we want to calculate collisions first, based on input, etc.
      //_map.Update(gameTime, ref _player);
    }

    public override void Reload(GraphicsDevice graphicsDevice)
    {
      XmlManager<Player> playerLoader = new XmlManager<Player>();
      XmlManager<Map> mapLoader = new XmlManager<Map>();
      _player = playerLoader.Load(DefaultPaths.DefaultPlayerPath);
      //if (File.Exists(Global._xmlTempPath + Global._defaultMap))
      //_map = mapLoader.Load(Global._xmlTempPath + Global._defaultMap);
      //else
      //_map = mapLoader.Load(Global._xmlPath + Global._defaultMap);
      _player.LoadContent(graphicsDevice);
      //_map.LoadContent(graphicsDevice);
    }

    public override void DrawPlayer(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      _player.Draw(spriteBatch);
    }
#endif
    #endregion
  }
}
