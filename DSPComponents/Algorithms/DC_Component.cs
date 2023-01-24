using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum = 0, average = 0; ;
            List<float> res = new List<float>();
            //  throw new NotImplementedException();
            for (int i = 0; i < InputSignal.Samples.Count(); i++) {
                sum += InputSignal.Samples[i];
            }
            average = sum / InputSignal.Samples.Count();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
               res.Add( InputSignal.Samples[i] - average);
            }
            OutputSignal = new Signal(res, false);
        }
    }
}
