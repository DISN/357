using Engine.System.Effects;
using Engine.System.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.System.Entities
{
  [Serializable]
  public class Image
  {
    #region Fields
    Vector2 _origin;
    ContentManager _content;
    RenderTarget2D _renderTarget;
    SpriteFont _font;
    Dictionary<string, ImageEffectBase> _effectList;
    #endregion

    #region Constructor
    public Image()
    {
      Path = Text = Effects = String.Empty;
      FontName = "Fonts/Verdana";
      Position = Vector2.Zero;
      Scale = Vector2.One;
      Alpha = 1.0f;
      Rotation = 0.0f;
      SourceRect = Rectangle.Empty;
      _effectList = new Dictionary<string, ImageEffectBase>();
    }
    #endregion

    #region Properties
    public float Alpha, Rotation;
    public string Text, FontName, Path, Effects;
    public Vector2 Position, Scale;
    public Rectangle SourceRect;
    public bool IsActive;
    public FadeEffect FadeEffect; //passed in reference later so we can modify it's properties
    public SpriteSheetEffect SpriteSheetEffect;

    [XmlIgnore]
    public Texture2D Texture;
    #endregion

    #region Methods
    public void LoadContent()
    {
      _content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, DefaultPaths.ContentPath);

      if (Path != String.Empty)
        Texture = _content.Load<Texture2D>(Path);

      _font = _content.Load<SpriteFont>(FontName);

      Vector2 dimensions = Vector2.Zero;

      if (Texture != null)
      {
        dimensions.X += Texture.Width;
        dimensions.Y = Math.Max(Texture.Height, _font.MeasureString(Text).Y);
      }
      else
        dimensions.Y = _font.MeasureString(Text).Y;
      dimensions.X += _font.MeasureString(Text).X;

      if (SourceRect == Rectangle.Empty)
        SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

      // The actual rendering
      _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
      ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
      ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
      ScreenManager.Instance.SpriteBatch.Begin();
      if (Texture != null)
        ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
      ScreenManager.Instance.SpriteBatch.DrawString(_font, Text, Vector2.Zero, Color.White);
      ScreenManager.Instance.SpriteBatch.End();

      Texture = _renderTarget;

      ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

      // Set image effects
      SetEffect<FadeEffect>(ref FadeEffect);
      SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

      if (Effects != String.Empty)
      {
        string[] split = Effects.Split(':');
        foreach (string item in split)
          ActivateEffect(item);
      }
    }

    public void UnloadContent()
    {
      _content.Unload();
      foreach (var effect in _effectList)
        DeactivateEffect(effect.Key);
    }

    public void Update(GameTime gameTime)
    {
      foreach (var effect in _effectList)
        if (effect.Value.IsActive)
          effect.Value.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2); //center point of the image
      spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, Rotation, _origin, Scale, SpriteEffects.None, 0.0f);
    }

    void SetEffect<T>(ref T effect)
    {
      if (effect == null)
        effect = (T)Activator.CreateInstance(typeof(T));
      else
      {
        (effect as ImageEffectBase).IsActive = true;
        var obj = this;
        (effect as ImageEffectBase).LoadContent(ref obj);
      }

      _effectList.Add(effect.GetType().ToString().Replace("Engine.System.Effects.", ""), (effect as ImageEffectBase)); //remove the namespace, as we just want the name
    }

    public void ActivateEffect(string effect)
    {
      if (_effectList.ContainsKey(effect))
      {
        _effectList[effect].IsActive = true;
        var obj = this;
        _effectList[effect].LoadContent(ref obj);
      }
    }

    public void DeactivateEffect(string effect)
    {
      if (_effectList.ContainsKey(effect))
      {
        _effectList[effect].IsActive = false;
        _effectList[effect].UnloadContent();
      }
    }

    public void StoreEffects()
    {
      Effects = String.Empty;

      foreach (var effect in _effectList)
        if (effect.Value.IsActive)
          Effects += effect.Key + ":";

      if (Effects != String.Empty)
        Effects.Remove(Effects.Length - 1); //removes the last column
    }

    public void RestoreEffects()
    {
      foreach (var effect in _effectList)
        DeactivateEffect(effect.Key);

      string[] split = Effects.Split(':');
      foreach (string s in split)
        ActivateEffect(s);
    }

#if GameEditorMode
    public void LoadContent(GraphicsDevice graphicsDevice)
    {
      //_content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "../../../Revolver/Editor/Debug/Content");

      if (Path != String.Empty)
        using (FileStream stream = File.OpenRead(DefaultPaths.ContentFolder + Path + ".png"))
          Texture = Texture2D.FromStream(graphicsDevice, stream);

      //_font = _content.Load<SpriteFont>(FontName);

      Vector2 dimensions = Vector2.Zero;

      if (Texture != null)
      {
        dimensions.X += Texture.Width;
        dimensions.Y = Texture.Height;
      }
      //else
      //dimensions.Y = _font.MeasureString(Text).Y;
      //dimensions.X += _font.MeasureString(Text).X;

      if (SourceRect == Rectangle.Empty)
        SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

      // The actual rendering
      _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
      ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
      ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
      ScreenManager.Instance.SpriteBatch.Begin();
      if (Texture != null)
        ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
      //ScreenManager.Instance.SpriteBatch.DrawString(_font, Text, Vector2.Zero, Color.White);
      ScreenManager.Instance.SpriteBatch.End();

      Texture = _renderTarget;

      ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

      // Set image effects
      SetEffect<FadeEffect>(ref FadeEffect);
      SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

      if (Effects != String.Empty)
      {
        string[] split = Effects.Split(':');
        foreach (string item in split)
          ActivateEffect(item);
      }
    }

    public void UnloadContentEditor()
    {
      foreach (var effect in _effectList)
        DeactivateEffect(effect.Key);
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
      if (!string.IsNullOrEmpty(Path))
        using (FileStream stream = File.OpenRead(DefaultPaths.ContentFolder + Path + ".png"))
          Texture = Texture2D.FromStream(graphicsDevice, stream);

      if (SourceRect == Rectangle.Empty)
        SourceRect = Texture.Bounds;
    }

    public void DrawEditor(SpriteBatch spriteBatch)
    {
      _origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2); //center point of the image
      spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, Rotation, _origin, Scale, SpriteEffects.None, 0.0f);
      //spriteBatch.Draw(Texture, Position, SourceRect, Color.White * Alpha, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
    }
#endif
    #endregion
  }

}
