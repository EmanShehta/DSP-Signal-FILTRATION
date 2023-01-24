﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), new List<int>(), false);
            // throw new NotImplementedException();
            if (L != 0 && M ==0)
            {
                var up = interbolation(InputSignal);
                OutputSignal = filter(up);
                
            }
            else if (L == 0 && M != 0) 
            {

                var down = filter(InputSignal);
                OutputSignal = decemination(down);
            }
            else if (L != 0 && M != 0)
            {
                var up = interbolation(InputSignal);
                var filt = filter(up);
                OutputSignal = decemination(filt);
            }
            
        }
        public Signal filter(Signal s)
        {
            FIR f = new FIR
            {
                InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW,
                InputFS = 8000,
                InputStopBandAttenuation = 50,
                InputCutOffFrequency = 1500,
                InputTransitionBand = 500,
                InputTimeDomainSignal = s
            };
            f.Run();
            //f.OutputYn.Samples.RemoveAt(f.OutputYn.Samples.Count-1);
            return f.OutputYn;
        }
        public Signal decemination(Signal s)
        {
            List<float> down = new List<float>();
            var res = new Signal(new List<float>(), new List<int>(), false);
            int count = s.Samples.Count;
            int indx = 0;
            for(int i = 0; i < count; i+=M)
            {   
                res.Samples.Add(s.Samples[i]);
                res.SamplesIndices.Add(indx);
                indx++;
            }
            return res;
        }
        public Signal interbolation(Signal s)
        {
            int count = s.Samples.Count;
            var res =new Signal(new List<float>(), new List<int>(), false);
            int indx=0;
            for (int i = 0; i < count; i++)
            {
                res.Samples.Add(s.Samples[i]);
                res.SamplesIndices.Add(indx);
                indx++;
                for (int j = 0 ; j < L-1 ; j++)
                {
                    
                    res.Samples.Add(0);
                    res.SamplesIndices.Add(indx);
                    indx++;
                }
                
            }
            return res;
        }
    }

}