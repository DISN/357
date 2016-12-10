using Engine.System.Managers;
using Engine.System.Matrices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Cameras
{
  public class CameraBase
  {
    #region Fields
    protected ContentManager _content;

    protected WorldMatrix _worldMatrix;
    protected ViewMatrix _viewMatrix;
    protected ProjectionMatrix _projectionMatrix;

    Vector3 _cameraPosition, _cameraTarget;
    float _vFovAngle;

    float _fovAngle, _aspectRatio, _near, _far, _zoom, _rotation;
    private bool _isViewTransformationDirty = true;

    Matrix camTranslationMatrix = Matrix.Identity;
    Matrix camRotationMatrix = Matrix.Identity;
    Matrix camScaleMatrix = Matrix.Identity;
    Matrix resTranslationMatrix = Matrix.Identity;

    Vector3 camTranslationVector = Vector3.Zero;
    Vector3 camScaleVector = Vector3.Zero;
    Vector3 resTranslationVector = Vector3.Zero;
    #endregion

    #region Constructors
    public CameraBase()
    {
      Type = this.GetType();
      XMLPath = DefaultPaths.XmlPath + Type.ToString().Replace("Engine.System.Cameras.", "") + ".xml";

      _worldMatrix = new WorldMatrix();
      _viewMatrix = new ViewMatrix();
      _projectionMatrix = new ProjectionMatrix();

      _vFovAngle = 55.0f;
      _zoom = 0.1f;
      _rotation = 0.0f;
      _cameraPosition = Vector3.Zero;
    }
    #endregion

    #region Properties
    public WorldMatrix WorldMatrix
    {
      get { return _worldMatrix; }
      set
      {
        _worldMatrix = value;
      }
    }

    public ViewMatrix ViewMatrix
    {
      get { return _viewMatrix; }
      set
      {
        _viewMatrix = value;
      }
    }

    public ProjectionMatrix ProjectionMatrix
    {
      get { return _projectionMatrix; }
      set
      {
        _projectionMatrix = value;
      }
    }

    [XmlIgnore]
    public Type Type;

    public string XMLPath;

    public Vector3 CameraPosition
    {
      get { return _cameraPosition; }
      set
      {
        _cameraPosition = value;
        _isViewTransformationDirty = true;
      }
    }

    public Vector3 CameraTarget
    {
      get { return _cameraTarget; }
      set
      {
        _cameraTarget = value;
      }
    }

    public float VFovAngle
    {
      get { return _vFovAngle; }
      set { _vFovAngle = value; }
    }

    public float FovAngle
    {
      get { return _fovAngle; }
      set
      {
        _fovAngle = value;
      }
    }

    public float AspectRatio
    {
      get { return _aspectRatio; }
      set
      {
        _aspectRatio = value;
      }
    }

    public float Near
    {
      get { return _near; }
      set
      {
        _near = value;
      }
    }

    public float Far
    {
      get { return _far; }
      set
      {
        _far = value;
      }
    }

    public float Zoom
    {
      get { return _zoom; }
      set
      {
        _zoom = value;
        if (_zoom < 0.1f)
        {
          _zoom = 0.1f;
        }
        _isViewTransformationDirty = true;
      }
    }

    public float Rotation
    {
      get { return _rotation; }
      set
      {
        _rotation = value;
        _isViewTransformationDirty = true;
      }
    }
    #endregion

    #region Methods
    public virtual void LoadContent(ContentManager content)
    {
      _content = new ContentManager(content.ServiceProvider, DefaultPaths.ContentPath);
    }

    public virtual void UnloadContent()
    {
      _content.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
      InputManager.Instance.Update();
    }

    public virtual void Draw()
    {
      //RefreshViewTransformationMatrix();
    }

    public void Move(Vector3 amount)
    {
      CameraPosition += amount;
    }

    public void SetPosition(Vector3 position)
    {
      CameraPosition = position;
    }

    public void RefreshViewTransformationMatrix()
    {
      if (_isViewTransformationDirty)
      {
        camTranslationVector.X = -CameraPosition.X;
        camTranslationVector.Y = -CameraPosition.Y;

        Matrix.CreateTranslation(ref camTranslationVector, out camTranslationMatrix);
        Matrix.CreateRotationZ(Rotation, out camRotationMatrix);

        camScaleVector.X = Zoom;
        camScaleVector.Y = Zoom;
        camScaleVector.Z = 1;

        Matrix.CreateScale(ref camScaleVector, out camScaleMatrix);

        resTranslationVector.X = CameraManager.Instance.VirtualWidth * 0.5f;
        resTranslationVector.Y = CameraManager.Instance.VirtualHeight * 0.5f;
        resTranslationVector.Z = 0;

        Matrix.CreateTranslation(ref resTranslationVector, out resTranslationMatrix);

        ViewMatrix.TransformedMatrix = camTranslationMatrix *
                                       camRotationMatrix *
                                       camScaleMatrix *
                                       resTranslationMatrix *
                                       CameraManager.Instance.GetTransformationMatrix();

        _isViewTransformationDirty = false;
      }
    }

    public void RecalculateTransformationMatrices()
    {
      _isViewTransformationDirty = true;
    }

#if GameEditorMode
    public virtual void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
    {
      InputManager.Instance.Update(keyboardState, gamePadState, mouseState);
    }
#endif
    #endregion
  }
}
