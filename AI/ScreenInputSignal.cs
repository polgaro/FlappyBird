using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class ScreenInputSignal
    {
        public double YBirdCoordinate { get; set; }
        public double DistanceToNextObstacle { get; set; }
        public double ObstacleBoundary1 { get; set; }
        public double ObstacleBoundary2 { get; set; }
        public bool IsBirdDead { get; set; }
    }
}
