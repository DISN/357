using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Enums
{
  public enum ResourceType
  {
    [XmlEnum(Name="Tile")]
    Tile,
    [XmlEnum(Name = "GameObject")]
    Sprite
  }
}
