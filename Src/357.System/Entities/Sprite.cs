using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.System.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.System.Entities
{
  public class Sprite : ResourceBase
  {
    #region Constructors
    public Sprite()
    {
      ResourceType = Enums.ResourceType.Sprite;
#if GameEditorMode
      TileDimensions = new Vector2();
#endif
    }
    #endregion

    #region Properties
#if GameEditorMode
    public Tile SourceImage;

    public Vector2 TileDimensions;
#endif
    #endregion

    #region Methods
#if GameEditorMode
    public override void Load()
    {
      try
      {
        Sprite loadedSprite = new Sprite();
        XmlManager<Sprite> xmlGameObjectManager = new XmlManager<Sprite>();
        loadedSprite = xmlGameObjectManager.Load(LinkID);
        this.ResourceID = loadedSprite.ResourceID;
        this.LinkID = loadedSprite.LinkID;
        this.Name = loadedSprite.Name;
      }
      catch (Exception)
      {
        throw;
      }
    }
#endif
    #endregion
  }
}
