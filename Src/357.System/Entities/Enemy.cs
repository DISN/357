using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.System.Managers;
using Microsoft.Xna.Framework.Input;

namespace Engine.System.Entities
{
  public class Enemy : EntityBase
  {
    #region Constructors
    public Enemy()
    {

    }
    #endregion

    #region Methods
    public override void LoadContent()
    {
      base.LoadContent();
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
    }
    #endregion
  }
}
