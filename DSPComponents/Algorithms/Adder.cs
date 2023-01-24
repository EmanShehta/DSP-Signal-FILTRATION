using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        { 
            int length = InputSignals.Count();
            List<float> res = new List<float>();
            float x = 0;
           // int max_num_sampels = 0;
            //for (int i = 0; i < length; i++)
            //{
            //     max_num_sampels = Math.Max(InputSignals[i].Samples.Count(), max_num_sampels);
            //}
            //for (int i = 0; i < length; i++)
            //{
            //    while(InputSignals[i].Samples.Count < max_num_sampels) {
            //        InputSignals[i].Samples.Add(0);
            //    }
            //}
            int sampels_length = InputSignals[0].Samples.Count();
            for (int i = 0; i < sampels_length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    x += InputSignals[j].Samples[i];
                }
                    res.Add(x);
                x = 0;
            }
            OutputSignal = new Signal(res, false);
        }
    }
}