#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace ProjectStella
{
    public class InputManager
    {
        InputState currentState;   // current frame input
        InputState lastState;      // last frame input

        InputState input;

        /// <summary>
        /// Create a new input manager
        /// </summary>
        public InputManager()
        {
            currentState = new InputState();
            lastState = new InputState();

            input = new InputState();
        }

        /// <summary>
        /// Begin input (aqruire input from all controlls)
        /// </summary>
        public void BeginInputProcessing(bool singlePlayer)
        {
            input.Update();
        }

        /// <summary>
        /// End input (save current input to last frame input)
        /// </summary>
        public void EndInputProcessing()
        {
            input.Update();
        }

        /// <summary>
        /// Get the current input state
        /// </summary>
        public InputState CurrentState
        {
            get { return currentState; }
        }

        public InputState LastState
        {
            get { return lastState; }
        }

        /// <summary>
        /// Check if a key is down in current frame for a given player
        /// </summary>
        public bool IsKeyDown(int player, Keys key)
        {
            return input.CurrentKeyboardStates[player].IsKeyDown(key);
        }

        /// <summary>
        /// Check if a key was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsKeyPressed(int player, Keys key)
        {
            return input.CurrentKeyboardStates[player].IsKeyDown(key) &&
                input.LastKeyboardStates[player].IsKeyUp(key);
        }

        /// <summary>
        /// Return left stick position in a Vector2
        /// </summary>
        public Vector2 LeftStick(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Left;
        }

        /// <summary>
        /// Return right stick position in a Vector2
        /// </summary>
        public Vector2 RightStick(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Right;
        }

        /// <summary>
        /// Check if left trigger was pressed in this frame for a given player
        /// (positive this frame and zero in last frame)
        /// </summary>
        public bool IsTriggerPressedLeft(int player)
        {
            return input.CurrentGamePadStates[player].Triggers.Left > 0 &&
                input.LastGamePadStates[player].Triggers.Left == 0;
        }

        /// <summary>
        /// Check if right trigger was pressed in this frame for a given player
        /// (positive this frame and zero in last frame)
        /// </summary>
        public bool IsTriggerPressedRigth(int player)
        {
            return input.CurrentGamePadStates[player].Triggers.Right > 0 &&
                input.LastGamePadStates[player].Triggers.Right == 0;
        }

        /// <summary>
        /// Check if back button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedBack(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.Back == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.Back == ButtonState.Released;
        }

        /// <summary>
        /// Check if start button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedStart(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.Start == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.Start == ButtonState.Released;
        }

        /// <summary>
        /// Check if dpad left button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedDPadLeft(int player)
        {
            return input.CurrentGamePadStates[player].DPad.Left == ButtonState.Pressed &&
                input.LastGamePadStates[player].DPad.Left == ButtonState.Released;
        }

        /// <summary>
        /// Check if dpad right button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedDPadRight(int player)
        {
            return input.CurrentGamePadStates[player].DPad.Right == ButtonState.Pressed &&
                input.LastGamePadStates[player].DPad.Right == ButtonState.Released;
        }

        /// <summary>
        /// Check if dpad up button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedDPadUp(int player)
        {
            return input.CurrentGamePadStates[player].DPad.Up == ButtonState.Pressed &&
                input.LastGamePadStates[player].DPad.Up == ButtonState.Released;
        }

        /// <summary>
        /// Check if dpad down button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedDPadDown(int player)
        {
            return input.CurrentGamePadStates[player].DPad.Down == ButtonState.Pressed &&
                input.LastGamePadStates[player].DPad.Down == ButtonState.Released;
        }

        /// <summary>
        /// Check if A button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedA(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.A == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.A == ButtonState.Released;
        }

        /// <summary>
        /// Check if B button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedB(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.B == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.B == ButtonState.Released;
        }

        /// <summary>
        /// Check if X button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedX(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.X == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.X == ButtonState.Released;
        }

        /// <summary>
        /// Check if A button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedY(int player)
        {
            return input.CurrentGamePadStates[player].Buttons.Y == ButtonState.Pressed &&
                input.LastGamePadStates[player].Buttons.Y == ButtonState.Released;
        }

        /// <summary>
        /// Check if left shoulder button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedLeftShoulder(int player)
        {
            return
                (input.CurrentGamePadStates[player].Buttons.LeftShoulder ==
                    ButtonState.Pressed) &&
                (input.LastGamePadStates[player].Buttons.LeftShoulder ==
                    ButtonState.Released);
        }

        /// <summary>
        /// Check if right shoulder button was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedRightShoulder(int player)
        {
            return
                (input.CurrentGamePadStates[player].Buttons.RightShoulder ==
                    ButtonState.Pressed) &&
                (input.LastGamePadStates[player].Buttons.RightShoulder ==
                    ButtonState.Released);
        }

        /// <summary>
        /// Check if left stick was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedLeftStick(int player)
        {
            return
                (input.CurrentGamePadStates[player].Buttons.LeftStick ==
                    ButtonState.Pressed) &&
                (input.LastGamePadStates[player].Buttons.LeftStick ==
                    ButtonState.Released);
        }

        /// <summary>
        /// Check if right stick was pressed in this frame for a given player
        /// (down in this frame and up in last frame)
        /// </summary>
        public bool IsButtonPressedRightStick(int player)
        {
            return
                (input.CurrentGamePadStates[player].Buttons.RightStick ==
                    ButtonState.Pressed) &&
                (input.LastGamePadStates[player].Buttons.RightStick ==
                    ButtonState.Released);
        }

        /// <summary>
        /// Check left stick as a button for up press
        /// </summary>
        public bool IsButtonPressedLeftStickUp(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Left.Y > 0.5f &&
                input.LastGamePadStates[player].ThumbSticks.Left.Y <= 0.5f;
        }

        /// <summary>
        /// Check left stick as a button for down press
        /// </summary>
        public bool IsButtonPressedLeftStickDown(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Left.Y < -0.5f &&
                input.LastGamePadStates[player].ThumbSticks.Left.Y >= -0.5f;
        }

        /// <summary>
        /// Check left stick as a button for left press
        /// </summary>
        public bool IsButtonPressedLeftStickLeft(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Left.X < -0.5f &&
                input.LastGamePadStates[player].ThumbSticks.Left.X >= -0.5f;
        }

        /// <summary>
        /// Check left stick as a button for right press
        /// </summary>
        public bool IsButtonPressedLeftStickRight(int player)
        {
            return input.CurrentGamePadStates[player].ThumbSticks.Left.X > 0.5f &&
                input.LastGamePadStates[player].ThumbSticks.Left.X <= 0.5f;
        }
    }
}
