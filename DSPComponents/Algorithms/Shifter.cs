using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        
        public override void Run()
        {
            List<int> index = new List<int>();
            List<float> value = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                value.Add(InputSignal.Samples[i]);
                if(InputSignal.Periodic == true)
                index.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                else index.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
            }
            OutputShiftedSignal = new Signal(value, index, InputSignal.Periodic);
        }
    }
}