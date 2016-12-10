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

namespace Engine.System.Entities
{
  public class Triangle : ModelBase
  {
    #region Fields
    VertexPositionColor[] _vertices;
    BasicEffect _effect;
    VertexBuffer _buffer; //takes the vertices we defined, and pass them to the graphics device's buffer for rendering.
    #endregion

    #region Constructors
    public Triangle()
    {

    }
    #endregion

    #region Properties
    #endregion

    #region Methods
    public override void Load(GraphicsDevice graphicsDevice)
    {
      base.Load(graphicsDevice);

      // Setup vertices (declared clockwise)
      _vertices = new VertexPositionColor[3]
      {
        new VertexPositionColor(new Vector3(0.0f, 1.0f, 0.0f), Color.Red),
        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 0.0f), Color.Blue),
        new VertexPositionColor(new Vector3(1.0f, -1.0f, 0.0f), Color.Green)
      };

      // Initialize effect
      _effect = new BasicEffect(ScreenManager3D.Instance.GraphicsDevice);

      // Create our vertex buffer
      _buffer = new VertexBuffer(ScreenManager3D.Instance.GraphicsDevice, VertexPositionColor.VertexDeclaration, 3, BufferUsage.WriteOnly);
      _buffer.SetData(_vertices);

      // Set our position
      Position = new Vector3(1.0f, 0.0f, -1.0f);
      
      // Set our rotation
      RotationY = 28.5f;
    }

    public override void LoadContent(ContentManager content)
    {
      base.LoadContent(content);
    }

    public override void UnloadContent()
    {
      _effect.Dispose();
      _buffer.Dispose();
      ScreenManager3D.Instance.GraphicsDevice.SetVertexBuffer(null);
      _vertices = null;
      _effect = null;
      _buffer = null;
      base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw(WorldMatrix world, ViewMatrix view, ProjectionMatrix projection)
    {
      _effect.World = world.TransformedMatrix * Matrix.CreateRotationY(RotationY) * Matrix.CreateTranslation(Position);
      _effect.View = view.TransformedMatrix;
      _effect.Projection = projection.TransformedMatrix;
      _effect.VertexColorEnabled = true;

      foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
      {
        pass.Apply();
        ScreenManager3D.Instance.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, _vertices, 0, 1);
      }
    }
    #endregion

    #region Events
    
    #endregion
  }
}
