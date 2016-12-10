using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Interfaces
{
  public interface ISaveable
  {
    /// <summary>
    /// Indicates if the object is dirty, i.e. can be saved
    /// </summary>
    bool IsDirty { get; set; }

    /// <summary>
    /// If applicable, returns the list of objects to save for this ISaveable object
    /// </summary>
    List<ISaveable> ObjectsToSave { get; set; }

    /// <summary>
    /// Returns the name of the object
    /// </summary>
    string Name { get; set; }
  }
}
