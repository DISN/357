using Engine.System.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Effects
{
  /// <summary>
  /// Activates a fade animation on an image
  /// </summary>
  public class FadeEffect : ImageEffectBase
  {
    #region Constructor
    public FadeEffect()
    {
      FadeSpeed = 1;
      Increase = false;
    }
    #endregion

    #region Properties
    public float FadeSpeed;
    public bool Increase;
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
        if (!Increase)
          _image.Alpha -= FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        else
          _image.Alpha += FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_image.Alpha < 0.0f)
        {
          Increase = true;
          _image.Alpha = 0.0f;
        }
        else if (_image.Alpha > 1.0f)
        {
          Increase = false;
          _image.Alpha = 1.0f;
        }
      }
      else
        _image.Alpha = 1.0f;
    }
    #endregion
  }
}
