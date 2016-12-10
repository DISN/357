using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Interfaces
{
  /// <summary>
  /// Interface ISelectable
  /// </summary>
  public interface ISelectable
  {
    /// <summary>
    /// Gets the selection.
    /// </summary>
    /// <value>The selection.</value>
    object Selection { get; }

    /// <summary>
    /// Gets the selection's name.
    /// </summary>
    /// <value>The selection's name.</value>
    string SelectionName { get; }
  }
}
