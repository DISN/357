using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Engine.System.Enums;
using Engine.System.Interfaces;

namespace Engine.System.Entities
{
#if GameEditorMode
  public class Layer : ISaveable
  {
#else
  public class Layer
  {
#endif
    public class TileMap
    {
      [XmlElement("Row")]
      public List<string> Row;

      public TileMap()
      {
        Row = new List<string>();
      }
    }

    #region Fields
    private List<Tile> _underlayingTiles, _overlayingTiles;
    private TileState _state;

#if GameEditorMode
    List<List<Vector2>> _editorTileMap;
    bool _isDirty;
    private List<ISaveable> _objectsToSave;
#endif
    #endregion

    #region Constructors
    public Layer()
    {
      Image = new Image();
      _underlayingTiles = new List<Tile>();
      _overlayingTiles = new List<Tile>();
      SolidTiles = String.Empty;
      OverlayTiles = String.Empty;
#if GameEditorMode
      Name = String.Empty;
      TileDimensions = new Vector2();
      _editorTileMap = new List<List<Vector2>>();
      _isDirty = false;
      LayerNumber = -1;
#endif
    }
    #endregion

    #region Properties
    [XmlElement("TileMap")]
    public TileMap Tile;

    public Image Image;

    public string SolidTiles, OverlayTiles;

    public int LayerNumber;
#if GameEditorMode
    [XmlIgnore]
    public Vector2 TileDimensions;

    [XmlIgnore]
    public List<List<Vector2>> EditorTileMap
    {
      get { return _editorTileMap; }
      set
      {
        if (_editorTileMap != value)
          _editorTileMap = value;
      }
    }
#endif
    #endregion

    #region Methods
    public void LoadContent(Vector2 tileDimensions)
    {
      Image.LoadContent();
      Vector2 position = -tileDimensions;

      foreach (string row in Tile.Row)
      {
        string[] split = row.Split(']');
        position.X = -tileDimensions.X;
        position.Y += tileDimensions.Y;
        foreach (string s in split)
        {
          if (s != String.Empty)
          {
            position.X += tileDimensions.X;
            if (!s.Contains('x'))
            {
              _state = TileState.Passive;
              Tile tile = new Tile();

              string str = s.Replace("[", String.Empty);
              int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
              int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

              if (SolidTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                _state = TileState.Solid;

              tile.LoadContent(position, new Rectangle(value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y), _state);

              if (OverlayTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                _overlayingTiles.Add(tile);
              else
                _underlayingTiles.Add(tile);
            }
          }
        }
      }
    }

    public void UnloadContent()
    {
      Image.UnloadContent();
    }

    public void Update(GameTime gameTime, ref Player player)
    {
      // TODO: Optimize this. It's not the best thing to do to loop through all the tiles lists
      foreach (Tile tile in _underlayingTiles)
        tile.Update(gameTime, ref player);

      foreach (Tile tile in _overlayingTiles)
        tile.Update(gameTime, ref player);
    }

    public void Draw(SpriteBatch spriteBatch, DrawType drawType)
    {
      // TODO: Optimize this. It's not the best thing to do to loop through all the tiles lists
      List<Tile> tiles;
      if (drawType == DrawType.Underlay)
        tiles = _underlayingTiles;
      else
        tiles = _overlayingTiles;

      foreach (Tile tile in tiles)
      {
        Image.Position = tile.Position;
        Image.SourceRect = tile.SourceRect;
        Image.Draw(spriteBatch);
      }
    }

#if GameEditorMode
    public void LoadContent(Vector2 tileDimensions, GraphicsDevice graphicsDevice)
    {
      Image.LoadContent(graphicsDevice);
      Vector2 position = -tileDimensions;

      foreach (string row in Tile.Row)
      {
        string[] split = row.Split(']');
        position.X = -tileDimensions.X;
        position.Y += tileDimensions.Y;
        foreach (string s in split)
        {
          if (s != String.Empty)
          {
            position.X += tileDimensions.X;
            if (!s.Contains('x'))
            {
              _state = TileState.Passive;
              Tile tile = new Tile();

              string str = s.Replace("[", String.Empty);
              int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
              int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

              if (SolidTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                _state = TileState.Solid;

              tile.LoadContent(position, new Rectangle(value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y), _state);

              if (OverlayTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                _overlayingTiles.Add(tile);
              else
                _underlayingTiles.Add(tile);
            }
          }
        }
      }
    }

    public void UnloadContentEditor()
    {
      Image.UnloadContentEditor();
    }

    public void Initialize(GraphicsDevice graphicsDevice, Vector2 tileDimensions)
    {
      foreach (string row in Tile.Row)
      {
        string[] split = row.Split(']');
        List<Vector2> tempTileMap = new List<Vector2>();
        foreach (string s in split)
        {
          int value1, value2;
          if (s != String.Empty && !s.Contains('x'))
          {
            string str = s.Replace("[", String.Empty);
            value1 = int.Parse(str.Substring(0, str.IndexOf(":")));
            value2 = int.Parse(str.Substring(str.IndexOf(":") + 1));
          }
          else
            value1 = value2 = -1;

          tempTileMap.Add(new Vector2(value1, value2));
        }
        EditorTileMap.Add(tempTileMap);
      }

      Image.Initialize(graphicsDevice);
      this.TileDimensions = tileDimensions;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < EditorTileMap.Count; i++)
        for (int j = 0; j < EditorTileMap[i].Count; j++)
          if (EditorTileMap[i][j] != -Vector2.One)
          {
            Image.Position = new Vector2(j * TileDimensions.X, i * TileDimensions.Y);
            Image.SourceRect = new Rectangle((int)(EditorTileMap[i][j].X * TileDimensions.X), (int)(EditorTileMap[i][j].Y * TileDimensions.Y), (int)TileDimensions.X, (int)TileDimensions.Y);
            Image.Draw(spriteBatch);
          }

      Image.Position = Vector2.Zero;
      Image.SourceRect = Image.Texture.Bounds;
    }

    public void ReplaceTile(Vector2 mousePosition, Rectangle selectedTileRegion)
    {
      Vector2 startIndex = new Vector2(mousePosition.X / TileDimensions.X, mousePosition.Y / TileDimensions.Y);
      Vector2 tileIndex = new Vector2(selectedTileRegion.X, selectedTileRegion.Y - 1);
      Vector2 mapIndex = Vector2.Zero;

      for (int i = (int)startIndex.Y; i <= startIndex.Y + selectedTileRegion.Height; i++)
      {
        tileIndex.X = selectedTileRegion.X;
        tileIndex.Y++;
        for (int j = (int)startIndex.X; j <= startIndex.X + selectedTileRegion.Width; j++)
        {
          if (tileIndex.X * TileDimensions.X > Image.Texture.Width ||
            tileIndex.Y * TileDimensions.Y > Image.Texture.Height)
            mapIndex = -Vector2.One;
          else
            mapIndex = tileIndex;

          try
          {
            EditorTileMap[i][j] = mapIndex;
          }
          catch
          {
            while (EditorTileMap.Count <= i)
            {
              List<Vector2> tempTileMap = new List<Vector2>();
              for (int k = 0; k < EditorTileMap[0].Count; k++)
                tempTileMap.Add(-Vector2.One);
              EditorTileMap.Add(tempTileMap);
            }

            while (EditorTileMap[i].Count <= j)
              EditorTileMap[i].Add(-Vector2.One);
          }

          tileIndex.X++;
        }
      }
      IsDirty = true;
    }

    public Tile SelectTile(Vector2 mousePosition)
    {
      int iIndex = (int)(mousePosition.X / TileDimensions.X);
      int jIndex = (int)(mousePosition.Y / TileDimensions.Y);
      Vector2 tileIndex;
      try
      {
        tileIndex = EditorTileMap[iIndex][jIndex];
      }
      catch
      {
        tileIndex = Vector2.Zero;
      }

      Tile tile = new Tile();
      tile.LoadContent(new Vector2(jIndex * TileDimensions.X, iIndex * TileDimensions.Y), new Rectangle((int)(tileIndex.X * TileDimensions.X), (int)(tileIndex.Y * TileDimensions.Y), (int)TileDimensions.X, (int)TileDimensions.Y), TileState.Passive);

      return tile;
    }

    public void Save()
    {
      Tile.Row = new List<string>();

      for (int i = 0; i < EditorTileMap.Count; i++)
      {
        string row = String.Empty;
        for (int j = 0; j < EditorTileMap[i].Count; j++)
          if (EditorTileMap[i][j] == -Vector2.One)
            row += "[x:x]";
          else
            row += "[" + EditorTileMap[i][j].X.ToString() + ":" + EditorTileMap[i][j].Y.ToString() + "]";
        Tile.Row.Add(row);
      }

      IsDirty = false;
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
