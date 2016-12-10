using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Engine.System.Managers
{
  public class XmlManager<T>
  {
    #region Fields
    #endregion

    #region Constructor
    public XmlManager(XmlAttributeOverrides overrides)
    {
      Type = typeof(T);
      Overrides = overrides;
    }

    public XmlManager()
    {
      Type = typeof(T);
      Overrides = new XmlAttributeOverrides();
    }
    #endregion

    #region Properties
    public Type Type;

    public XmlAttributeOverrides Overrides;
    #endregion

    #region Methods
    public T Load(string path)
    {
      T instance;
      using (TextReader reader = new StreamReader(path))
      {
        XmlSerializer xml = new XmlSerializer(Type, Overrides);
        instance = (T)xml.Deserialize(reader);
      }
      return instance;
    }

    public void Save(string path, object obj)
    {
      using (TextWriter writer = new StreamWriter(path))
      {
        XmlSerializer xml = new XmlSerializer(Type, Overrides);
        xml.Serialize(writer, obj);
      }
    }
    #endregion
  }
}
