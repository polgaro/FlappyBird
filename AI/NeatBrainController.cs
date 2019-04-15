using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class NeatBrainController : IController
    {
        IBlackBox _brain;
        NeatGenome _genome;

        public NeatBrainController(IBlackBox brain, NeatGenome genome)
        {
            _brain = brain;
            _genome = genome;
        }

        public IBlackBox Brain
        {
            get
            {
                return _brain;
            }
        }

        public NeatGenome Genome
        {
            get
            {
                return _genome;
            }
        }

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
            // Clear the network
            _brain.ResetState();

            // Convert the game board into an input array for the network
            SetInputSignalArray(_brain.InputSignalArray, screenInputSignal);

            // Activate the network
            _brain.Activate();

            return _brain.OutputSignalArray[0] == 1;
        }

        private void SetInputSignalArray(ISignalArray inputSignalArray, ScreenInputSignal screenInputSignal)
        {
            inputSignalArray[0] = screenInputSignal.YTopBirdCoordinate;
            inputSignalArray[1] = screenInputSignal.YBottomBirdCoordinate;
            inputSignalArray[2] = screenInputSignal.DistanceToNextObstacle;
            inputSignalArray[3] = screenInputSignal.ObstacleBoundary1;
            inputSignalArray[4] = screenInputSignal.ObstacleBoundary2;
            inputSignalArray[5] = screenInputSignal.IsBirdDead ? 1 : 0;
        }
    }
}
