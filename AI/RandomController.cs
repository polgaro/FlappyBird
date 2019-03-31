using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class RandomController : IController
    {
        public bool WantsJumpBoost()
        {
            return false;
        }

        public bool WantsSlowFall()
        {
            return false;
        }

        public bool WantsToJump()
        {
            return new Random().Next(100) < 5;
        }
    }
}
