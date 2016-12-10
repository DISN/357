using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Matrices
{
  /// <summary>
  /// Class that includes methods useful in order to create World Matrices.
  /// 
  /// The world matrix basically tells us where the model is located in the world.
  /// 
  /// The coordinates of our vertices represent where they are in relationship with the rest of the model. At this point, we apply the world matrix.
  /// This transforms our model space coordinates into world space. World space coordinates represent where the vertices (and the entire model) are
  /// in relationship to the whole world.
  /// </summary>
  public class WorldMatrix : MatrixBase
  {
    #region Fields

    #endregion

    #region Constructors
    public WorldMatrix(Matrix originalMatrix, Matrix transformedMatrix)
      : base(originalMatrix, transformedMatrix)
    { }

    public WorldMatrix()
      : base()
    { }
    #endregion

    #region Properties

    #endregion

    #region Methods
    /// <summary>
    /// Creates a translation matrix.
    /// 
    /// Used to translate (slide or move) the points of a model from one location to another.
    /// </summary>
    /// <param name="position"></param>
    public void CreateTranslation(float xPosition, float yPosition, float zPosition)
    {
      this.TransformedMatrix = Matrix.CreateTranslation(xPosition, yPosition, zPosition);
    }

    /// <summary>
    /// Creates a translation matrix.
    /// 
    /// Used to translate (slide or move) the points of a model from one location to another.
    /// </summary>
    /// <param name="position"></param>
    public void CreateTranslation(Vector3 position)
    {
      this.TransformedMatrix = Matrix.CreateTranslation(position);
    }

    /// <summary>
    /// Creates a matrix that rotates around the x-axis.
    /// 
    /// Used to rotate the points of a model.
    /// </summary>
    /// <param name="angleInRadians"></param>
    public void CreateRotationX(float angleInRadians)
    {
      this.TransformedMatrix = Matrix.CreateRotationX(angleInRadians);
    }

    /// <summary>
    /// Creates a matrix that rotates around the y-axis.
    /// 
    /// Used to rotate the points of a model.
    /// </summary>
    /// <param name="angleInRadians"></param>
    public void CreateRotationY(float angleInRadians)
    {
      this.TransformedMatrix = Matrix.CreateRotationY(angleInRadians);
    }

    /// <summary>
    /// Creates a matrix that rotates around the z-axis.
    /// 
    /// Used to rotate the points of a model.
    /// </summary>
    /// <param name="angleInRadians"></param>
    public void CreateRotationZ(float angleInRadians)
    {
      this.TransformedMatrix = Matrix.CreateRotationZ(angleInRadians);
    }

    /// <summary>
    /// Creates a matrix that rotates points around an arbitrary axis.
    /// 
    /// Used to rotate the points of a model.
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    public void CreateFromAxisAngle(Vector3 axis, float angle)
    {
      this.TransformedMatrix = Matrix.CreateFromAxisAngle(axis, angle);
    }

    /// <summary>
    /// Creates a scale matrix.
    /// 
    /// Used to scale the points of a model to make it bigger or smaller.
    /// </summary>
    /// <param name="scale"></param>
    public void CreateScale(float scale)
    {
      this.TransformedMatrix = Matrix.CreateScale(scale);
    }

    /// <summary>
    /// Creates a matrix from multiple transformations.
    /// 
    /// It is important to do these transformations in the correct order. In normal math, 3 * 2 is the same as 2 * 3. Order doesn't matter.
    /// However, with matrices, the order matters. The order reflects the order we perform our operations in.
    /// 
    /// As an illustration, imagine you are standing at a particular point facing north, and you are going to move ten feet forward, and also turn right 90°.
    /// If you move forward then turn right 90°, then you will be ten feet north of where you were, facing east. If you turn first, then move, you will be
    /// ten feet east of where you were, facing east. Getting the order wrong makes very weird things happen.
    /// 
    /// Make sure you get your matrices in the correct order. And the correct order is in the reverse order of the order you want them done.
    /// </summary>
    /// <param name="matrices"></param>
    public void CreateMultipleWorldTransformation(params Matrix[] matrices)
    {
      foreach (Matrix matrix in matrices)
        this.TransformedMatrix *= matrix;
    }
    #endregion
  }
}
