using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Decisions
{
  public class DecisionBase
  {
    #region Fields
    protected ContentManager _content;
    #endregion

    #region Constructors
    public DecisionBase()
    {

    }
    #endregion

    #region Properties
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
