using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.System.Managers;
using Microsoft.Xna.Framework.Input;

namespace Engine.System.Entities
{
  public class Player : EntityBase
  {
    #region Fields
    float _jumpSpeed;
    #endregion

    #region Constructors
    public Player()
    {
      _jumpSpeed = 1500.0f;
      Gravity = 100.0f;
      SyncTilePosition = false;
      ActivateGravity = true;
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
      Image.IsActive = true;

      // Free-form timed-based movement (instead of tile-based movement), includes diagonal movement
      if (InputManager.Instance.KeyDown(Keys.Down))
      {
        if (InputManager.Instance.KeyDown(Keys.Right))
          X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else if (InputManager.Instance.KeyDown(Keys.Left))
          X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else
          X = 0;

        Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 0; // 1e rangee qui correspond aux sprites vers le bas
      }
      else if (InputManager.Instance.KeyDown(Keys.Up))
      {
        if (InputManager.Instance.KeyDown(Keys.Right))
          X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else if (InputManager.Instance.KeyDown(Keys.Left))
          X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else
          X = 0;

        Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 3; // Derniere rangee qui correspond aux sprites vers le haut
      }
      else if (InputManager.Instance.KeyDown(Keys.Right))
      {
        if (InputManager.Instance.KeyDown(Keys.Down))
          Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else if (InputManager.Instance.KeyDown(Keys.Up))
          Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else
          Y = 0;

        X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 2; // 3e rangee qui correspond aux sprites vers la droite
      }
      else if (InputManager.Instance.KeyDown(Keys.Left))
      {
        if (InputManager.Instance.KeyDown(Keys.Down))
          Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else if (InputManager.Instance.KeyDown(Keys.Up))
          Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else
          Y = 0;

        X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 1; // 2e rangee qui correspond aux sprites vers la gauche
      }
      else
        X = Y = 0;

      /*if (InputManager.Instance.KeyDown(Keys.Right))
      {
        X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 2; // 3e rangee qui correspond aux sprites vers la droite
      }
      else if (InputManager.Instance.KeyDown(Keys.Left))
      {
        X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Image.SpriteSheetEffect.CurrentFrame.Y = 1; // 2e rangee qui correspond aux sprites vers la gauche
      }
      else
      {
        Image.IsActive = false;
        X = 0;
      }

      if (InputManager.Instance.KeyDown(Keys.Up) && !ActivateGravity)
      {
        Y = -_jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        ActivateGravity = true;
      }

      if (ActivateGravity)
        Y += Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      else
        Y = 0;*/
      
      if (X == 0 && Y == 0)
        Image.IsActive = false;

      Image.Position += Velocity;
      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
    }
    #endregion
  }
}
