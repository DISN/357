using Engine.System.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Effects
{
  public class SpriteSheetEffect : ImageEffectBase
  {
    #region Fields
    float _defaultFrame;
    bool _isWalkingReestablished; //just so the character doesn't look retarded when we "spam" the walk buttons/keys
    #endregion

    #region Constructors
    public SpriteSheetEffect()
    {
      _isWalkingReestablished = false;
      AmountOfFrames = new Vector2(7, 4);
      CurrentFrame = new Vector2(3, 0); // X = 1 pour Marth
      _defaultFrame = CurrentFrame.X;
      SwitchFrame = 100;
      FrameCounter = 0;
    }
    #endregion

    #region Properties
    public int FrameCounter;
    public int SwitchFrame;
    public Vector2 CurrentFrame;
    public Vector2 AmountOfFrames;
    public Vector2 LastWalkingFrame; //just so the character doesn't look retarded when we "spam" the walk buttons/keys

    public int Width
    {
      get
      {
        if (_image.Texture != null)
          return _image.Texture.Width / (int)AmountOfFrames.X;
        return 0;
      }
    }

    public int Height
    {
      get
      {
        if (_image.Texture != null)
          return _image.Texture.Height / (int)AmountOfFrames.Y;
        return 0;
      }
    }
    #endregion

    #region Methods
    public override void LoadContent(ref Image Image)
    {
      base.LoadContent(ref Image);
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (_image.IsActive)
      {
        if (!_isWalkingReestablished)
        {
          CurrentFrame.X = LastWalkingFrame.X;
          _isWalkingReestablished = true;
        }

        FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (FrameCounter >= SwitchFrame)
        {
          FrameCounter = 0;
          CurrentFrame.X++;

          if (CurrentFrame.X * Width > _image.Texture.Width)
            CurrentFrame.X = 0;
        }
      }
      else
      {
        LastWalkingFrame.X++;
        if (LastWalkingFrame.X * Width > _image.Texture.Width)
          LastWalkingFrame.X = 0;
        else
          for (int i = 1; i < AmountOfFrames.Length() - 1; i++)
            if (LastWalkingFrame.X == i)
            {
              LastWalkingFrame.X = i + 1;
              break;
            }

        CurrentFrame.X = _defaultFrame;

        _isWalkingReestablished = false;
      }

      if (CurrentFrame.X <= AmountOfFrames.X - 1)
        _image.SourceRect = new Rectangle((int)CurrentFrame.X * Width, (int)CurrentFrame.Y * Height, Width, Height);
    }
    #endregion
  }
}
