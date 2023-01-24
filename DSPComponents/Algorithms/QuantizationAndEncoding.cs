using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            float step;
            List<float> res = new List<float>();
            List<float> midpoints = new List<float>();
            List<float> startInterval = new List<float>();
            List<float> endInterval = new List<float>();
            int x = 0;
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();

            // throw new NotImplementedException();
            if (InputLevel > 0)
            {
                step = (InputSignal.Samples.Max() - InputSignal.Samples.Min()) / InputLevel;
                InputNumBits = Convert.ToInt32(Math.Log(InputLevel, 2));
            }
            else
            {
                step = (InputSignal.Samples.Max() - InputSignal.Samples.Min()) / (float)Math.Pow(2, InputNumBits);
                InputLevel = Convert.ToInt32(Math.Pow(2, InputNumBits));
            }

            for (float s = InputSignal.Samples.Min(); s < InputSignal.Samples.Max();)
            {
                startInterval.Add(s);
                s += step;
                endInterval.Add(s);
                midpoints.Add((endInterval[x] + startInterval[x]) / 2);
                x++;
            }
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= startInterval[j] && InputSignal.Samples[i] < endInterval[j]+0.0001)
                    {
                        res.Add(midpoints[j]);
                        OutputIntervalIndices.Add(j + 1);
                        string e = Convert.ToString(OutputIntervalIndices[i] - 1, 2);
                        while (e.Length < InputNumBits)
                        {
                            e = e.Insert(0, "0");
                        }
                        OutputEncodedSignal.Add(e);
                        OutputSamplesError.Add(midpoints[j] - InputSignal.Samples[i]);
                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(res, false);
        }
    }
}
