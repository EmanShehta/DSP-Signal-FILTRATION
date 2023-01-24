using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            //   throw new NotImplementedException();
            samples = new List<float>();
            if (type == "sin" && SamplingFrequency >= 2*AnalogFrequency)
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    float result ;
                    result = A * (float)Math.Sin(2 * (float)Math.PI * (AnalogFrequency / SamplingFrequency) * i + PhaseShift);
                    samples.Add(result);
                }

            }
            else if (type == "cos" && SamplingFrequency >= 2 * AnalogFrequency)
            {

                for (int i = 0; i < SamplingFrequency; i++)
                {
                    float res = (float)Math.Round((A * (float)Math.Cos(2 * (float)Math.PI * (AnalogFrequency / SamplingFrequency) * i + PhaseShift)),4);
                    samples.Add(res);
                }
            }

        }
    }
}
