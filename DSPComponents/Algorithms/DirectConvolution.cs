using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            // throw new NotImplementedException();
            int start , end ;
            start = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            end = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
            List<float> res = new List<float>();
            List<int> index = new List<int>();
            for (int i = start; i <= end; i++) 
            {
                float n = 0;
                for (int j = start; j < InputSignal1.Samples.Count(); j++) 
                {
                    if (i - j >= InputSignal2.Samples.Count())
                    {
                        continue;
                    }
                    if (i - j < InputSignal2.SamplesIndices.Min() || i - j > InputSignal2.SamplesIndices.Max())
                    {
                        continue;
                    }
                    if (j < InputSignal1.SamplesIndices.Min() || j > InputSignal1.SamplesIndices.Max())
                    {
                        continue;
                    }
                    int in1 = InputSignal1.SamplesIndices.IndexOf(j);
                    int in2 = InputSignal2.SamplesIndices.IndexOf(i-j);
                   n += InputSignal1.Samples[in1] * InputSignal2.Samples[in2];
                }
                if (end == i && n == 0.0) break;
                res.Add(n);
                index.Add(i);
            }
            OutputConvolvedSignal = new Signal(res, index,false);
        }
    }
}
