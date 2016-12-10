using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Matrices
{
  /// <summary>
  /// Class that includes methods useful in order to create View Matrices.
  /// 
  /// In a sense, this transformation tells us where the player is located in the world.
  /// 
  /// Usually, we take our world space coordinates (WorldMatrix) and apply another transformation to them.
  /// This next transformation is the view matrix.
  /// 
  /// This matrix will put our coordinates into view space, which is where the vertices are in relationship to the
  /// viewer. That is, where the player's camera or eye is located at.
  /// </summary>
  public class ViewMatrix : MatrixBase
  {
    #region Fields

    #endregion

    #region Constructors
    public ViewMatrix(Matrix originalMatrix, Matrix transformedMatrix)
      : base(originalMatrix, transformedMatrix)
    { }

    public ViewMatrix()
      : base()
    { }
    #endregion

    #region Properties

    #endregion

    #region Methods
    /// <summary>
    /// Creates a View Matrix with an already specified up vector.
    /// </summary>
    /// <param name="cameraPosition">The location of the camera in your 3D world.</param>
    /// <param name="cameraTarget">The point in your world that the camera is looking at.</param>
    public void CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget)
    {
      this.TransformedMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
    }

    /// <summary>
    /// Creates a View Matrix.
    /// 
    /// The up vector needs to be specified, because even though the camera has a location in space, and is looking at a particular point,
    /// the camera could still rotate around. You will only really deal with the up vector if you get into a sophisticated flight simulator
    /// or something like that. For the most part, you could use Vector3.Up (along the y-axis) for this value, and not worry about it beyond that.
    /// </summary>
    /// <param name="cameraPosition">The location of the camera in your 3D world.</param>
    /// <param name="cameraTarget">The point in your world that the camera is looking at.</param>
    /// <param name="cameraUpVector">The direction that is 'up' for the camera.</param>
    public void CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
    {
      this.TransformedMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
    }
    #endregion
  }
}
