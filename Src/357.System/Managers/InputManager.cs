using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Engine.System.Enums;

namespace Engine.System.Managers
{
  public class InputManager
  {
    #region Fields
    private static InputManager _instance;
    private KeyboardState _currentKeyboardState, _previousKeyboardState;

    private GamePadState _currentGamePadState, _previousGamePadState;

    private MouseState _currentMouseState, _previousMouseState;

    private PlayerIndex _index = PlayerIndex.One;
    private bool refreshData = false;
    private bool _leftIsHeld;
    #endregion

    #region Properties
    public static InputManager Instance
    {
      get
      {
        if (_instance == null)
          _instance = new InputManager();
        return _instance;
      }
    }

    public KeyboardState CurrentKeyState
    {
      get
      {
        return _currentKeyboardState;
      }
    }

    public KeyboardState PreviousKeyState
    {
      get
      {
        return _previousKeyboardState;
      }
    }

    public GamePadState CurrentGamePadState
    {
      get
      {
        return _currentGamePadState;
      }
    }

    public GamePadState PreviousGamePadState
    {
      get
      {
        return _previousGamePadState;
      }
    }

    public MouseState CurrentMouseState
    {
      get
      {
        return _currentMouseState;
      }
    }

    public MouseState PreviousMouseState
    {
      get
      {
        return _previousMouseState;
      }
    }

    public Vector2 CurrentMousePosition = new Vector2();

    /// <summary>
    /// The index that is used to poll the gamepad. 
    /// </summary>
    public PlayerIndex Index
    {
      get { return _index; }
      set
      {
        _index = value;
        if (refreshData)
        {
          Update();
        }
      }
    }

    /// <summary>
    /// The current position of the left stick. 
    /// Y is automatically reversed for you.
    /// </summary>
    public Vector2 LeftStickPosition
    {
      get
      {
        return new Vector2(
            _currentGamePadState.ThumbSticks.Left.X,
            -_currentGamePadState.ThumbSticks.Left.Y);
      }
    }

    /// <summary>
    /// The current position of the right stick.
    /// Y is automatically reversed for you.
    /// </summary>
    public Vector2 RightStickPosition
    {
      get
      {
        return new Vector2(
            _currentGamePadState.ThumbSticks.Right.X,
            -_currentGamePadState.ThumbSticks.Right.Y);
      }
    }

    /// <summary>
    /// The current velocity of the left stick.
    /// Y is automatically reversed for you.
    /// expressed as: 
    /// current stick position - last stick position.
    /// </summary>
    public Vector2 LeftStickVelocity
    {
      get
      {
        Vector2 temp =
            _currentGamePadState.ThumbSticks.Left -
            _previousGamePadState.ThumbSticks.Left;
        return new Vector2(temp.X, -temp.Y);
      }
    }

    /// <summary>
    /// The current velocity of the right stick.
    /// Y is automatically reversed for you.
    /// expressed as: 
    /// current stick position - last stick position.
    /// </summary>
    public Vector2 RightStickVelocity
    {
      get
      {
        Vector2 temp =
            _currentGamePadState.ThumbSticks.Right -
            _previousGamePadState.ThumbSticks.Right;
        return new Vector2(temp.X, -temp.Y);
      }
    }

    /// <summary>
    /// The current position of the left trigger.
    /// </summary>
    public float LeftTriggerPosition
    {
      get { return _currentGamePadState.Triggers.Left; }
    }

    /// <summary>
    /// The current position of the right trigger.
    /// </summary>
    public float RightTriggerPosition
    {
      get { return _currentGamePadState.Triggers.Right; }
    }

    /// <summary>
    /// The velocity of the left trigger.
    /// expressed as: 
    /// current trigger position - last trigger position.
    /// </summary>
    public float LeftTriggerVelocity
    {
      get
      {
        return
            _currentGamePadState.Triggers.Left -
            _previousGamePadState.Triggers.Left;
      }
    }

    /// <summary>
    /// The velocity of the right trigger.
    /// expressed as: 
    /// current trigger position - last trigger position.
    /// </summary>
    public float RightTriggerVelocity
    {
      get
      {
        return _currentGamePadState.Triggers.Right -
            _previousGamePadState.Triggers.Right;
      }
    }

    /// <summary>
    /// The current mouse position.
    /// </summary>
    public Vector2 MousePosition
    {
      get { return new Vector2(_currentMouseState.X, _currentMouseState.Y); }
    }

    /// <summary>
    /// The current mouse velocity.
    /// Expressed as: 
    /// current mouse position - last mouse position.
    /// </summary>
    public Vector2 MouseVelocity
    {
      get
      {
        return (
            new Vector2(_currentMouseState.X, _currentMouseState.Y) -
            new Vector2(_previousMouseState.X, _previousMouseState.Y)
            );
      }
    }

    /// <summary>
    /// The current mouse scroll wheel position.
    /// See the Mouse's ScrollWheel property for details.
    /// </summary>
    public float MouseScrollWheelPosition
    {
      get
      {
        return _currentMouseState.ScrollWheelValue;
      }
    }

    /// <summary>
    /// The mouse scroll wheel velocity.
    /// Expressed as:
    /// current scroll wheel position - 
    /// the last scroll wheel position.
    /// </summary>
    public float MouseScrollWheelVelocity
    {
      get
      {
        return (_currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue);
      }
    }
    #endregion

    #region Methods
    public void Update()
    {
      if (!refreshData)
        refreshData = true;

      _previousKeyboardState = _currentKeyboardState;
      _previousGamePadState = _currentGamePadState;
      _previousMouseState = _currentMouseState;
      if (!ScreenManager.Instance.IsTransitioning) //assures that while a screen is transitioning, we don't register anymore inputs
      {
        _currentKeyboardState = Keyboard.GetState();
        _currentGamePadState = GamePad.GetState(_index);
        _currentMouseState = Mouse.GetState();
        CurrentMousePosition.X = _currentMouseState.X;
        CurrentMousePosition.Y = _currentMouseState.Y;

        if (_leftIsHeld == true)
        {
          if (_currentMouseState.LeftButton == ButtonState.Released)
            _leftIsHeld = false;
        }
      }
    }

    public bool KeyPressed(params Keys[] keys)
    {
      foreach (Keys key in keys)
      {
        if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
          return true;
      }
      return false;
    }

    public bool KeyReleased(params Keys[] keys)
    {
      foreach (Keys key in keys)
      {
        if (_currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key))
          return true;
      }
      return false;
    }

    public bool KeyDown(params Keys[] keys)
    {
      foreach (Keys key in keys)
      {
        if (_currentKeyboardState.IsKeyDown(key))
          return true;
      }
      return false;
    }

    public bool LeftHeld()
    {
      if (_currentMouseState.LeftButton == ButtonState.Pressed
          && _previousMouseState.LeftButton == ButtonState.Pressed
          && _leftIsHeld == true)
      {
        return true;
      }
      else if ((_currentMouseState.LeftButton == ButtonState.Pressed
          && _previousMouseState.LeftButton == ButtonState.Pressed) &&
          (_currentMouseState.X != _previousMouseState.X || _currentMouseState.Y != _previousMouseState.Y))
      {
        _leftIsHeld = true;
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Used for debug purposes.
    /// Indicates if the user wants to exit immediately.
    /// </summary>
    public bool ExitRequested
    {
#if (!XBOX)
      get
      {
        return (
            (IsCurPress(Buttons.Start) &&
            IsCurPress(Buttons.Back)) ||
            IsCurPress(Keys.Escape));
      }
#else
      get { return (IsCurPress(Buttons.Start) && IsCurPress(Buttons.Back)); }
#endif
    }

    /// <summary>
    /// Checks if the requested button is a new press.
    /// </summary>
    /// <param name="button">
    /// The button to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected button is being 
    /// pressed in the current state but not the last state.
    /// </returns>
    public bool IsNewPress(Buttons button)
    {
      return (
          _previousGamePadState.IsButtonUp(button) &&
          _currentGamePadState.IsButtonDown(button));
    }

    /// <summary>
    /// Checks if the requested button is a current press.
    /// </summary>
    /// <param name="button">
    /// the button to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected button is being 
    /// pressed in the current state and in the last state.
    /// </returns>
    public bool IsCurPress(Buttons button)
    {
      return (
          _previousGamePadState.IsButtonDown(button) &&
          _currentGamePadState.IsButtonDown(button));
    }

    /// <summary>
    /// Checks if the requested button is an old press.
    /// </summary>
    /// <param name="button">
    /// the button to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected button is not being
    /// pressed in the current state and is being pressed in the last state.
    /// </returns>
    public bool IsOldPress(Buttons button)
    {
      return (
          _previousGamePadState.IsButtonDown(button) &&
          _currentGamePadState.IsButtonUp(button));
    }

    /// <summary>
    /// Checks if the requested key is a new press.
    /// </summary>
    /// <param name="key">
    /// the key to check.
    /// </param>
    /// <returns>
    /// a bool that indicates whether the selected key is being 
    /// pressed in the current state and not in the last state.
    /// </returns>
    public bool IsNewPress(Keys key)
    {
      return (
          _previousKeyboardState.IsKeyUp(key) &&
          _currentKeyboardState.IsKeyDown(key));
    }

    /// <summary>
    /// Checks if the requested key is a current press.
    /// </summary>
    /// <param name="key">
    /// the key to check.
    /// </param>
    /// <returns>
    /// a bool that indicates whether the selected key is being 
    /// pressed in the current state and in the last state.
    /// </returns>
    public bool IsCurPress(Keys key)
    {
      return (
          _previousKeyboardState.IsKeyDown(key) &&
          _currentKeyboardState.IsKeyDown(key));
    }

    /// <summary>
    /// Checks if the requested button is an old press.
    /// </summary>
    /// <param name="key">
    /// the key to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selectde button is not being
    /// pressed in the current state and being pressed in the last state.
    /// </returns>
    public bool IsOldPress(Keys key)
    {
      return (
          _previousKeyboardState.IsKeyDown(key) &&
          _currentKeyboardState.IsKeyUp(key));
    }

    /// <summary>
    /// Checks if the requested mosue button is a new press.
    /// </summary>
    /// <param name="button">
    /// teh mouse button to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected mouse button is being
    /// pressed in the current state but not in the last state.
    /// </returns>
    public bool IsNewPress(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          return (
              _previousMouseState.LeftButton == ButtonState.Released &&
              _currentMouseState.LeftButton == ButtonState.Pressed);
        case MouseButtons.MiddleButton:
          return (
              _previousMouseState.MiddleButton == ButtonState.Released &&
              _currentMouseState.MiddleButton == ButtonState.Pressed);
        case MouseButtons.RightButton:
          return (
              _previousMouseState.RightButton == ButtonState.Released &&
              _currentMouseState.RightButton == ButtonState.Pressed);
        case MouseButtons.ExtraButton1:
          return (
              _previousMouseState.XButton1 == ButtonState.Released &&
              _currentMouseState.XButton1 == ButtonState.Pressed);
        case MouseButtons.ExtraButton2:
          return (
              _previousMouseState.XButton2 == ButtonState.Released &&
              _currentMouseState.XButton2 == ButtonState.Pressed);
        default:
          return false;
      }
    }
    /// <summary>
    /// Checks if the requested mosue button is a current press.
    /// </summary>
    /// <param name="button">
    /// the mouse button to be checked.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected mouse button is being 
    /// pressed in the current state and in the last state.
    /// </returns>
    public bool IsCurPress(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          return (
              _previousMouseState.LeftButton == ButtonState.Pressed &&
              _currentMouseState.LeftButton == ButtonState.Pressed);
        case MouseButtons.MiddleButton:
          return (
              _previousMouseState.MiddleButton == ButtonState.Pressed &&
              _currentMouseState.MiddleButton == ButtonState.Pressed);
        case MouseButtons.RightButton:
          return (
              _previousMouseState.RightButton == ButtonState.Pressed &&
              _currentMouseState.RightButton == ButtonState.Pressed);
        case MouseButtons.ExtraButton1:
          return (
              _previousMouseState.XButton1 == ButtonState.Pressed &&
              _currentMouseState.XButton1 == ButtonState.Pressed);
        case MouseButtons.ExtraButton2:
          return (
              _previousMouseState.XButton2 == ButtonState.Pressed &&
              _currentMouseState.XButton2 == ButtonState.Pressed);
        default:
          return false;
      }
    }
    /// <summary>
    /// Checks if the requested mosue button is an old press.
    /// </summary>
    /// <param name="button">
    /// the mouse button to check.
    /// </param>
    /// <returns>
    /// a bool indicating whether the selected mouse button is not being 
    /// pressed in the current state and is being pressed in the old state.
    /// </returns>
    public bool IsOldPress(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          return (
              _previousMouseState.LeftButton == ButtonState.Pressed &&
              _currentMouseState.LeftButton == ButtonState.Released);
        case MouseButtons.MiddleButton:
          return (
              _previousMouseState.MiddleButton == ButtonState.Pressed &&
              _currentMouseState.MiddleButton == ButtonState.Released);
        case MouseButtons.RightButton:
          return (
              _previousMouseState.RightButton == ButtonState.Pressed &&
              _currentMouseState.RightButton == ButtonState.Released);
        case MouseButtons.ExtraButton1:
          return (
              _previousMouseState.XButton1 == ButtonState.Pressed &&
              _currentMouseState.XButton1 == ButtonState.Released);
        case MouseButtons.ExtraButton2:
          return (
              _previousMouseState.XButton2 == ButtonState.Pressed &&
              _currentMouseState.XButton2 == ButtonState.Released);
        default:
          return false;
      }
    }

#if GameEditorMode
    public void Update(KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
    {
      if (!refreshData)
        refreshData = true;

      _previousKeyboardState = _currentKeyboardState;
      _previousGamePadState = _currentGamePadState;
      _previousMouseState = _currentMouseState;
      if (!ScreenManager.Instance.IsTransitioning) //assures that while a screen is transitioning, we don't register anymore  inputs
      {
        _currentKeyboardState = keyboardState;
        _currentGamePadState = gamePadState;
        _currentMouseState = mouseState;
        CurrentMousePosition.X = _currentMouseState.X;
        CurrentMousePosition.Y = _currentMouseState.Y;

        if (_leftIsHeld == true)
        {
          if (_currentMouseState.LeftButton == ButtonState.Released)
            _leftIsHeld = false;
        }
      }
    }
#endif
    #endregion
  }
}
