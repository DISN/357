using Engine.System.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
#if GameEditorMode
using Engine.System.Extensions;
#endif

namespace Engine.System.Entities
{
  public class Game
  {
    public Game()
    {
      GameID = -1;
      Name = String.Empty;
      World = new List<World>();
      ResourcesToLoad = new List<ResourceBase>();
      LoadedResources = new List<ResourceBase>();
    }

    public int GameID;

    public string Name;

    [XmlElement("World")]
    public List<World> World;

    [XmlElement("ResourceBase")]
    public List<ResourceBase> ResourcesToLoad;

    public List<ResourceBase> LoadedResources;
    
#if GameEditorMode
    public void Load()
    {
      if (this.World.Count > 0)
        foreach (World world in World)
          world.Load();

      // At one point, we could load resources on demand only
      if (this.ResourcesToLoad.Count > 0)
        foreach (ResourceBase resource in ResourcesToLoad)
          if (resource.ResourceType == Enums.ResourceType.Tile)
          {
            Tile newTile = new Tile();
            newTile.ResourceType = resource.ResourceType;
            newTile.LinkID = resource.LinkID;
            newTile.Load();
            LoadedResources.Add(newTile);
          }
          else if (resource.ResourceType == Enums.ResourceType.Sprite)
          {
            Sprite newObject = new Sprite();
            newObject.ResourceType = resource.ResourceType;
            newObject.LinkID = resource.LinkID;
            newObject.Load();
            LoadedResources.Add(newObject);
          }
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
      foreach (World world in World)
        world.Initialize(graphicsDevice);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (World world in World)
        world.Draw(spriteBatch);
    }

    public XmlAttributeOverrides GenerateXmlAttributeOverrides()
    {
      XmlAttributeOverrides _gameXmlAttributeOverrides = new XmlAttributeOverrides();

      var ignore = new XmlAttributes { XmlIgnore = true };
      _gameXmlAttributeOverrides.Add<Game>(m => m.LoadedResources, ignore);
      _gameXmlAttributeOverrides.Add<ResourceBase>(m => m.ResourceID, ignore);
      _gameXmlAttributeOverrides.Add<ResourceBase>(m => m.Name, ignore);
      _gameXmlAttributeOverrides.Add<World>(m => m.Name, ignore);
      _gameXmlAttributeOverrides.Add<World>(m => m.Map, ignore);

      return _gameXmlAttributeOverrides;
    }
#endif
  }
}
