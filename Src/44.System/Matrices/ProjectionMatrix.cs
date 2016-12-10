using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Matrices
{
  /// <summary>
  /// Class that includes methods useful in order to create Projection Matrices.
  /// 
  /// This matrix essentially tells us what type of camera we are using.
  /// 
  /// It gives the computer the information it needs to determine where on the screen each of our vertices should appear
  /// (it "projects" the points onto the screen). This transformation essentially gets the vertices into screen coordinates,
  /// although the computer still has more work to do as it draws the model on the screen (like rasterization).
  /// </summary>
  public class ProjectionMatrix : MatrixBase
  {
    #region Fields

    #endregion

    #region Constructors
    public ProjectionMatrix(Matrix originalMatrix, Matrix transformedMatrix)
      : base(originalMatrix, transformedMatrix)
    { }

    public ProjectionMatrix()
      : base()
    { }
    #endregion

    #region Properties

    #endregion

    #region Methods
    /// <summary>
    /// Creates a perspective camera matrix.
    /// 
    /// Gives the feeling of depth. This means that distant objects will look small.
    /// </summary>
    /// <param name="fieldOfView">Represents the angle that the camera can see (in radians).</param>
    /// <param name="aspectRatio">The aspect ratio of the window (the ratio of how long the window is to how tall the window is).</param>
    /// <param name="nearPlaneDistance">The near clipping plane.</param>
    /// <param name="farPlaneDistance">The far clipping plane.</param>
    public void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
    {
      this.TransformedMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
    }

    /// <summary>
    /// Creates a perspective camera matrix.
    /// 
    /// Gives the feeling of depth. This means that distant objects will look small.
    /// </summary>
    /// <param name="width">Width of the viewing area at the near clipping plane</param>
    /// <param name="height">Height of the viewing area at the near clipping plane.</param>
    /// <param name="nearPlaneDistance">The near clipping plane.</param>
    /// <param name="farPlaneDistance">The far clipping plane.</param>
    public void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
    {
      this.TransformedMatrix = Matrix.CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance);
    }

    /// <summary>
    /// Creates an off-centered perspective camera matrix (for example, the left side goes out farther than the right side).
    /// 
    /// Gives the feeling of depth. This means that distant objects will look small.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="top"></param>
    /// <param name="nearPlaneDistance">The near clipping plane.</param>
    /// <param name="farPlaneDistance">The far clipping plane.</param>
    public void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
    {
      this.TransformedMatrix = Matrix.CreatePerspectiveOffCenter(left, right, bottom, top, nearPlaneDistance, farPlaneDistance);
    }

    /// <summary>
    /// Creates an ortographic camera matrix.
    /// 
    /// This means that distant objects will look the same size.
    /// </summary>
    /// <param name="width">The number of units across the projection should be.</param>
    /// <param name="height">How tall the projection should be in units.</param>
    /// <param name="zNearPlane">The near clipping plane.</param>
    /// <param name="zFarPlane">The far clipping plane.</param>
    public void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
    {
      this.TransformedMatrix = Matrix.CreateOrthographic(width, height, zNearPlane, zFarPlane);
    }

    /// <summary>
    /// Creates an off-centered ortographic camera matrix (for example, the left side goes out farther than the right side).
    /// 
    /// This means that distant objects will look the same size.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="top"></param>
    /// <param name="zNearPlane">The near clipping plane.</param>
    /// <param name="zFarPlane">The far clipping plane.</param>
    public void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
    {
      this.TransformedMatrix = Matrix.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
    }
    #endregion
  }
}
