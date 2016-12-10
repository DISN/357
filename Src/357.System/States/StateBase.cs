using Engine.System.Actions;
using Engine.System.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.States
{
  public class StateBase
  {
    #region Fields

    protected ContentManager _content;

    private string _stateName;
    private List<ComponentBase> _components;
    private List<ActionBase> _actions;

    #endregion

    #region Constructors
    public StateBase()
    {
      _stateName = String.Empty;
      _components = new List<ComponentBase>();
      _actions = new List<ActionBase>();
    }
    #endregion

    #region Properties

    public string StateName
    {
      get { return _stateName; }
      set { _stateName = value; }
    }

    public List<ComponentBase> Components
    {
      get { return _components; }
      set { _components = value; }
    }

    public List<ActionBase> Actions
    {
      get { return _actions; }
      set { _actions = value; }
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
      // Call update on all components
      _components.ForEach(x => x.Update(gameTime));
      // Call update on all actions
      _actions.ForEach(x => x.Update(gameTime));
    }

    public virtual void Draw()
    {

    }
    #endregion
  }
}
