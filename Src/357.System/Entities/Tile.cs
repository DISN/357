using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Engine.System.Enums;
using System.Xml.Serialization;
using System.IO;
using Engine.System.Managers;

namespace Engine.System.Entities
{
  public class Tile : ResourceBase
  {
    #region Fields
    private Vector2 _position;
    private Rectangle _sourceRect;
    private TileState _state;
    #endregion

    #region Constructors
    public Tile()
    {
      ResourceType = Enums.ResourceType.Tile;
      TileState = Enums.TileState.Solid;
#if GameEditorMode
      TileDimensions = new Vector2();
#endif
    }
    #endregion

    #region Properties
    public Vector2 Position { get { return _position; } }

    public Rectangle SourceRect { get { return _sourceRect; } }

    [XmlElement("TileState")]
    public Engine.System.Enums.TileState TileState;

#if GameEditorMode
    public Image Image;

    public Vector2 TileDimensions;
#endif
    #endregion

    #region Methods
    public void LoadContent(Vector2 position, Rectangle sourceRect, TileState state)
    {
      this._position = position;
      this._sourceRect = sourceRect;
      this._state = state;
    }

    public void UnloadContent()
    {

    }

    public void Update(GameTime gameTime, ref Player player)
    {
      // Collision detection (TODO: Need to do it for 8-directions, this was only for tile-based movement)
      if (_state == TileState.Solid)
      {
        Rectangle tileRect = new Rectangle((int)Position.X, (int)Position.Y, _sourceRect.Width, _sourceRect.Height);
        Rectangle playerRect = new Rectangle((int)player.Image.Position.X, (int)player.Image.Position.Y, player.Image.SourceRect.Width, player.Image.SourceRect.Height);

        if (playerRect.Intersects(tileRect))
        {
          if (player.X < 0)
            player.Image.Position.X = tileRect.Right;
          else if (player.X > 0)
            player.Image.Position.X = tileRect.Left - player.Image.SourceRect.Width; // top left corner of the sprite is positioned correctly
          else if (player.Y < 0)
            player.Image.Position.Y = tileRect.Bottom;
          else
            player.Image.Position.Y = tileRect.Top - player.Image.SourceRect.Height;

          player.Velocity = Vector2.Zero;
        }
      }
    }

#if GameEditorMode
    public override void Load()
    {
      try
      {
        Tile loadedTile = new Tile();
        XmlManager<Tile> xmlTileManager = new XmlManager<Tile>();
        loadedTile = xmlTileManager.Load(LinkID);
        this.ResourceID = loadedTile.ResourceID;
        this.LinkID = loadedTile.LinkID;
        this.Name = loadedTile.Name;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
      Image.Initialize(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Image.Draw(spriteBatch);
    }
#endif
    #endregion
  }
}
