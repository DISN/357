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
using System.Runtime.InteropServices;
using System.IO;
using Engine.System.Managers;
using Engine.System.Interfaces;

namespace Engine.System.Entities
{
#if GameEditorMode
  public class Map : ISaveable
  {
#else
  public class Map
  {
#endif
    #region Fields
#if GameEditorMode
    private bool _isDirty;
    private List<ISaveable> _objectsToSave;
#endif
    #endregion

    #region Constructors
    public Map()
    {
      Layer = new List<Layer>();
      TileDimensions = Vector2.Zero;
#if GameEditorMode
      MapID = -1;
      LinkID = String.Empty;
      Name = String.Empty;
      GridImage = new Image();
      _isDirty = false;
      _objectsToSave = new List<ISaveable>();
#endif
    }
    #endregion

    #region Properties
    [XmlElement("Layer")]
    public List<Layer> Layer;

    public Vector2 TileDimensions;

#if GameEditorMode
    public int MapID;

    public string LinkID;

    [XmlIgnore]
    public Image GridImage;

    public int MapWidth;

    public int MapHeight;
#endif
    #endregion

    #region Methods
    public void LoadContent()
    {
      foreach (Layer layer in Layer)
        layer.LoadContent(TileDimensions);
    }

    public void UnloadContent()
    {
      foreach (Layer layer in Layer)
        layer.UnloadContent();
    }

    public void Update(GameTime gameTime, ref Player player)
    {
      foreach (Layer layer in Layer)
        layer.Update(gameTime, ref player);
    }

    public void Draw(SpriteBatch spriteBatch, DrawType drawType)
    {
      foreach (Layer layer in Layer)
        layer.Draw(spriteBatch, drawType);
    }

#if GameEditorMode
    public void LoadContent(GraphicsDevice graphicsDevice)
    {
      foreach (Layer layer in Layer)
        layer.LoadContent(TileDimensions, graphicsDevice);
    }

    public void Load()
    {
      try
      {
        Map loadedMap = new Map();
        XmlManager<Map> xmlMapManager = new XmlManager<Map>();
        loadedMap = xmlMapManager.Load(LinkID);
        this.MapID = loadedMap.MapID;
        this.LinkID = loadedMap.LinkID;
        this.Name = loadedMap.Name;
        this.TileDimensions = loadedMap.TileDimensions;
        this.Layer = loadedMap.Layer;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void UnloadContentEditor()
    {
      foreach (Layer layer in Layer)
        layer.UnloadContentEditor();
    }

    public void Save()
    {
      foreach (Layer layer in Layer)
        layer.Save();

      XmlManager<Map> xmlMapManager = new XmlManager<Map>();
      if (!String.IsNullOrEmpty(LinkID))
        xmlMapManager.Save(LinkID, this);
      else
        xmlMapManager.Save(DefaultPaths.XmlFolder + Name + ".xml", this);
    }

    public void SaveAs(string filePath)
    {
      foreach (Layer layer in Layer)
        layer.Save();

      XmlManager<Map> xmlMapManager = new XmlManager<Map>();
      xmlMapManager.Save(filePath, this);
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
      foreach (Layer layer in Layer)
      {
        layer.Initialize(graphicsDevice, TileDimensions);
        layer.OnIsDirtyChanged += layer_OnIsDirtyChanged;
      }

      GridImage.Path = DefaultPaths.GridCellPath;
      GridImage.Alpha = 0.30F;
      GridImage.Initialize(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Layer layer in Layer)
        layer.Draw(spriteBatch);
    }

    public void DrawGrid(SpriteBatch spriteBatch)
    {
      int width = MapWidth / 32;
      int height = MapHeight / 32;

      for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
        {
          GridImage.Position = new Vector2(j * TileDimensions.X, i * TileDimensions.Y);
          GridImage.SourceRect = new Rectangle(0, 0, (int)TileDimensions.X, (int)TileDimensions.Y);
          GridImage.DrawEditor(spriteBatch);
        }

        GridImage.Position = Vector2.Zero;
        GridImage.SourceRect = GridImage.Texture.Bounds;
    }
#endif
    #endregion

    #region Events
#if GameEditorMode
    void layer_OnIsDirtyChanged(object sender, EventArgs e)
    {
      IsDirty = true;
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

        foreach (Layer layer in Layer)
        {
          if (!ObjectsToSave.Contains(layer) && layer.IsDirty)
            ObjectsToSave.Add(layer);
          else if (ObjectsToSave.Contains(layer) && !layer.IsDirty)
            ObjectsToSave.Remove(layer);
        }

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
