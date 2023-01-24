using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            // throw new NotImplementedException();

            List<float> res = new List<float>();
          //  int max = Math.Max();
            //if (InputSignal1.Samples.Count() > InputSignal2.Samples.Count())
            //{
            //    while (InputSignal2.Samples.Count() < InputSignal1.Samples.Count()) InputSignal2.Samples.Add(0);
            //    for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            //    {
            //        res.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
            //    }
            //}
            //else if (InputSignal1.Samples.Count() < InputSignal2.Samples.Count())
            //{
            //    while (InputSignal2.Samples.Count() > InputSignal1.Samples.Count()) InputSignal1.Samples.Add(0);
            //    for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            //    {
            //        res.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
            //    }
            //}
          //  else
            //{
                 for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                {
                    res.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
                }
           // }

            OutputSignal = new Signal(res, false);
        }
    }
}