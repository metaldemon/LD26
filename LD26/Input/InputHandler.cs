using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace LD26
{
    public struct CustomMouseState
    {
        public bool IsLeftButtonDown;
        public bool IsRightButtonDown;
        public bool IsMiddleButtonDown;
        public int MouseWheel;
        public int X;
        public int Y;
    }
    class InputHandler
    {
        static InputHandler()
        {
            refWindow = MyGameWindow.Instance;
            mouse = refWindow.Mouse;
        }

        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            MouseState msref = Mouse.GetState();
            CustomMouseState msfinal = new CustomMouseState();
            msfinal.IsLeftButtonDown = msref.LeftButton == ButtonState.Pressed;
            msfinal.IsRightButtonDown = msref.RightButton == ButtonState.Pressed;
            msfinal.IsMiddleButtonDown = msref.MiddleButton == ButtonState.Pressed;
            if (
                refWindow.WindowState == WindowState.Fullscreen &&
                refWindow.WindowBorder == WindowBorder.Hidden)
            {
                msfinal.X = msref.X;
                msfinal.Y = msref.Y;
            }
            else
            {
                msfinal.X = mouse.X;
                msfinal.Y = mouse.Y;
            }
            msfinal.MouseWheel = mouse.Wheel;
            CurrentMouseState = msfinal;
        }
        private static GameWindow refWindow;
        private static MouseDevice mouse;
        public static KeyboardState CurrentKeyboardState { get; private set; }
        public static KeyboardState PreviousKeyboardState { get; private set; }
        public static CustomMouseState CurrentMouseState { get; private set; }
        public static CustomMouseState PreviousMouseState { get; private set; }
    }
}
