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
            //HACK: it should be time + points * 100

            double fitness = bird.Points;

            return new FitnessInfo(fitness, fitness);
        }
    }
}
