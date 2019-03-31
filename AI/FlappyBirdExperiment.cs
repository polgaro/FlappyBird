using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.AI
{
    public class FlappyBirdExperiment : SimpleNeatExperiment
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator
        {
            get { return null; }
        }

        public override int InputCount
        {
            get { return 5; }
        }

        public override int OutputCount
        {
            get { return 1; }
        }

        public override bool EvaluateParents
        {
            get { return true; }
        }

        public override IGenomeListEvaluator<NeatGenome> GetGenomeListEvaluator(IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder)
        {
            return new ZeroListEvaluator<NeatGenome>();
        }
    }
}
