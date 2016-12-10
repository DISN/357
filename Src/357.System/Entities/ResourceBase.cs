using Engine.System.Enums;
using Engine.System.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Entities
{
  [Serializable]
#if GameEditorMode
  public class ResourceBase : ISaveable, ISelectable
  { 
#else
  public class ResourceBase
  {
#endif
    #region Fields
#if GameEditorMode
    bool _isDirty;
    private List<ISaveable> _objectsToSave;
#endif
    #endregion

    #region Constructors
    public ResourceBase()
    {
      Type = this.GetType();
      MotionType = Enums.MotionType.Static;
#if GameEditorMode
      ResourceID = -1;
      LinkID = String.Empty;
      Name = String.Empty;
      IncludedInLayers = new List<int>();
#endif
    }
    #endregion

    #region Properties
    [XmlIgnore]
    public Type Type;

    [XmlElement("ResourceType")]
    public Engine.System.Enums.ResourceType ResourceType;

    [XmlElement("MotionType")]
    public Engine.System.Enums.MotionType MotionType;

    [XmlElement("LinkID")]
    public string LinkID;

#if GameEditorMode
    public int ResourceID;

    public List<int> IncludedInLayers;
#endif
    #endregion

    #region Methods
    public Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
    {
      Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

      Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
      Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

      tileSheet.GetData<Color>(tileSheetData);

      int index = 0;
      for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
        for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
        {
          croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
          index++;
        }

      croppedImage.SetData<Color>(croppedImageData);

      return croppedImage;
    }

#if GameEditorMode
    public virtual void Load()
    {
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

    #region ISelectable Members
    public object Selection
    {
      get { return this; }
    }

    public string SelectionName
    {
      get { return Name; }
    }
    #endregion
#endif
  }
}
