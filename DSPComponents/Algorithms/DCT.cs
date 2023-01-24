using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            //
            List<float> res = new List<float>();
            int N = InputSignal.Samples.Count;
            for (int k = 0; k < InputSignal.Samples.Count; k++)
            {
                float sum = 0;
                for (int  n= 0; n < InputSignal.Samples.Count; n++)
                {
                    float x = (float)Math.PI / (4 * N);
                    sum += InputSignal.Samples[n] * (float)Math.Cos(x*(2*n -1)*(2*k -1));
                }
                res.Add((float)Math.Sqrt(2.0/N)*sum);
            }
            OutputSignal = new Signal(res,false);
        }
    }
}
