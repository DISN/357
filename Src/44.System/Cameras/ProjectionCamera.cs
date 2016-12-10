using Engine.System.Entities;
using Engine.System.Managers;
using Engine.System.Matrices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Cameras
{
  public class ProjectionCamera : CameraBase
  {
    #region Constructors
    public ProjectionCamera()
    {
      CameraPosition = new Vector3(0.0f, 0.0f, 2.0f);
      CameraTarget = new Vector3(0.0f, 0.0f, 0.0f); // Look back at the origin

      AspectRatio = 1920f / 1080f;
      FovAngle = MathHelper.ToRadians(VFovAngle * AspectRatio);
      Near = 0.1f; // the near clipping plane distance
      Far = 5000.0f; // the far clipping plane distance

      _worldMatrix.CreateTranslation(0.0f, 0.0f, 0.0f);
      _viewMatrix.CreateLookAt(CameraPosition, CameraTarget);
      _projectionMatrix.CreatePerspectiveFieldOfView(FovAngle, AspectRatio, Near, Far);
    }
    #endregion

    #region Methods
    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw()
    {
      base.Draw();
    }
    #endregion
  }
}
