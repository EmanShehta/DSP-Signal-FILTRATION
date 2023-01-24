using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            List<float> res = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count() - InputWindowSize +1; i++)
            {
                float sum = 0;
                for (int j = i; j < InputWindowSize + i ; j++) {
                    sum += InputSignal.Samples[j];
                }
                res.Add(sum / InputWindowSize);
            }
            OutputAverageSignal = new Signal(res,InputSignal.SamplesIndices,false);
        }
    }
}
