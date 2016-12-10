using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Components
{
  public class ComponentBase
  {
    #region Fields

    protected ContentManager _content;

    private string _componentName;

    #endregion

    #region Constructors
    public ComponentBase()
    {
      _componentName = String.Empty;
    }
    #endregion

    #region Properties

    public string ComponentName
    {
      get { return _componentName; }
      set { _componentName = value; }
    }

    #endregion

    #region Methods
    public virtual void LoadContent(ContentManager content)
    {
      _content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath);
    }

    public virtual void UnloadContent()
    {
      _content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void Draw()
    {

    }
    #endregion
  }
}
