using Engine.System.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
#if GameEditorMode
using Engine.System.Extensions;
#endif

namespace Engine.System.Entities
{
  public class World
  {
    public World()
    {
      WorldID = -1;
      LinkID = String.Empty;
      Name = String.Empty;
      Map = new List<Map>();
    }

    public int WorldID;

    public string LinkID;

    public string Name;

    [XmlElement("Map")]
    public List<Map> Map;

#if GameEditorMode
    public void Load()
    {
      try
      {
        World loadedWorld = new World();
        XmlManager<World> xmlWorldManager = new XmlManager<World>();
        loadedWorld = xmlWorldManager.Load(LinkID);
        this.WorldID = loadedWorld.WorldID;
        this.LinkID = loadedWorld.LinkID;
        this.Name = loadedWorld.Name;
        this.Map = loadedWorld.Map;

        if (this.Map.Count > 0)
          foreach (Map map in Map)
            map.Load();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
      foreach (Map map in Map)
        map.Initialize(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Map map in Map)
        map.Draw(spriteBatch);
    }

    public XmlAttributeOverrides GenerateXmlAttributeOverrides()
    {
      XmlAttributeOverrides _worldXmlAttributeOverrides = new XmlAttributeOverrides();

      var ignore = new XmlAttributes { XmlIgnore = true };
      _worldXmlAttributeOverrides.Add<Map>(m => m.Layer, ignore);
      _worldXmlAttributeOverrides.Add<Map>(m => m.TileDimensions, ignore);
      _worldXmlAttributeOverrides.Add<Map>(m => m.Name, ignore);

      return _worldXmlAttributeOverrides;
    }
#endif
  }
}
