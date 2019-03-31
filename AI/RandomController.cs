using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class RandomController : IController
    {
        public bool WantsJumpBoost(ScreenInputSignal screenInputSignal)
        {
            return false;
        }

        public bool WantsSlowFall(ScreenInputSignal screenInputSignal)
        {
            return false;
        }

        public bool WantsToJump(ScreenInputSignal screenInputSignal)
        {
            return Statics.Random.Next(100) < 5;
        }
    }
}
