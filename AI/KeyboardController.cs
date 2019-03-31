using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class KeyboardController : IController
    {
        public bool WantsJumpBoost(ScreenInputSignal screenInputSignal)
        {
            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.D1))
                return true;

            return false;
        }

        public bool WantsSlowFall(ScreenInputSignal screenInputSignal)
        {
            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.D2))
                return true;
            return false;
        }

        public bool WantsToJump(ScreenInputSignal screenInputSignal)
        {
            if (Statics.MANAGER_INPUT.IsKeyPressed(Keys.Space) || Statics.MANAGER_INPUT.IsLeftMouseClicked())
                return true;

            if (Statics.MANAGER_INPUT.CurrentGamePadState().DPad.Up == ButtonState.Pressed)
                return true;

            if (Statics.MANAGER_INPUT.IsGamepadPressed(Buttons.A))
                return true;
            return false;
        }
    }
}
