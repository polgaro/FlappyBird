using FlappyBird.Entities;
using SharpNeat.Core;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class BirdEvaluator
    {
        public FitnessInfo Evaluate(Bird bird)
        {
            double fitness = bird.GameTime.TotalSeconds + bird.Points * 100;

            return new FitnessInfo(fitness, fitness);
        }
    }
}
