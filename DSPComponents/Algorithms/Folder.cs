using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }   //  2 5 6 4  0  8 9 6 7
                                                 //   7 6 9 8  0  4 6 5 2
        public Signal OutputFoldedSignal { get; set; }


        public override void Run()
        {
            List<float> res = new List<float>();
            List<int> index = new List<int>();
            for (int i=0; i < InputSignal.Samples.Count(); i++)
            {
                InputSignal.SamplesIndices[i]= -1*InputSignal.SamplesIndices[i];
                
            }
            for (int i = InputSignal.Samples.Count()-1; i >= 0; i--)
            {
                index.Add(InputSignal.SamplesIndices[i]);
                res.Add(InputSignal.Samples[i]);
            }
            OutputFoldedSignal = new Signal(res, index, !InputSignal.Periodic);
        }
    }
}

