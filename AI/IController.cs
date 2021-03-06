﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public interface IController
    {
        bool WantsToJump(ScreenInputSignal screenInputSignal);
        bool WantsJumpBoost(ScreenInputSignal screenInputSignal);
        bool WantsSlowFall(ScreenInputSignal screenInputSignal);
    }
}
