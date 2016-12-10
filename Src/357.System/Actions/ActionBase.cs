using Engine.System.Decisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Actions
{
  public class ActionBase
  {
    #region Fields

    protected ContentManager _content;

    private string _actionName;
    private List<DecisionBase> _decisions;

    #endregion

    #region Constructors
    public ActionBase()
    {
      _actionName = String.Empty;
      _decisions = new List<DecisionBase>();
    }
    #endregion

    #region Properties

    public string ActionName
    {
      get { return _actionName; }
      set { _actionName = value; }
    }

    public List<DecisionBase> Decisions
    {
      get { return _decisions; }
      set { _decisions = value; }
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
      // Call update on all decisions
      _decisions.ForEach(x => x.Update(gameTime));
    }

    public virtual void Draw()
    {

    }
    #endregion
  }
}
