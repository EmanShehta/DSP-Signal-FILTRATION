using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            //   throw new NotImplementedException();

            List<float> res = new List<float>();
            float max = InputSignal.Samples.Max();
            float min = InputSignal.Samples.Min();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                float norm = ((InputSignal.Samples[i] - min) / (max - min))*(InputMaxRange-InputMinRange)+InputMinRange;
                res.Add(norm);
            }
            OutputNormalizedSignal = new Signal(res, false);
        }
    }
}
