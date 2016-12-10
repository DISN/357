using Engine.System.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Effects
{
  public class ImageEffectBase
  {
    #region Fields
    protected Image _image;
    #endregion

    #region Constructors
    public ImageEffectBase()
    {
      IsActive = false;
    }
    #endregion

    #region Properties
    public bool IsActive;
    #endregion

    #region Methods
    public virtual void LoadContent(ref Image Image)
    {
      this._image = Image;
    }

    public virtual void UnloadContent()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }
    #endregion
  }
}
