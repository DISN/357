using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.System.Matrices
{
  public class MatrixBase
  {
    #region Fields
    Matrix _originalMatrix;
    Matrix _transformedMatrix;
    #endregion

    #region Constructors
    public MatrixBase(Matrix originalMatrix, Matrix transformedMatrix = new Matrix())
    {
      _originalMatrix = originalMatrix;
    }

    public MatrixBase()
    {
      _originalMatrix = new Matrix();
      _transformedMatrix = new Matrix();
    }
    #endregion

    #region Properties
    public Matrix OriginalMatrix
    {
      get { return _originalMatrix; }
      set
      {
        _originalMatrix = value;
      }
    }

    public Matrix TransformedMatrix
    {
      get { return _transformedMatrix; }
      set
      {
        _transformedMatrix = value;
      }
    }
    #endregion

    #region Methods
    public float ConvertDegreesToRadians(float degrees)
    {
      return MathHelper.ToRadians(degrees);
    }
    #endregion
  }
}
