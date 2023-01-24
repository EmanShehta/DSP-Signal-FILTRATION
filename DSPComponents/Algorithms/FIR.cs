using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            String window = "";
            int N = 0;
            int coof = 0;
            if (InputStopBandAttenuation <= 21)
            {
                window = "rectangler";
                N = (int)((0.9f * InputFS) / InputTransitionBand);
                if (N % 2 == 0) N++;
                
            }
            else if (InputStopBandAttenuation > 21 && InputStopBandAttenuation <= 44)
            {
                window = "hanning";
                N = (int)((3.1f * InputFS) / InputTransitionBand);
                if (N % 2 == 0) N++;
                
            }
            else if (InputStopBandAttenuation > 44 && InputStopBandAttenuation <= 53)
            {
                window = "hamming";
                N = (int)((3.3f * InputFS) / InputTransitionBand);
                if (N % 2 == 0) N++;
                
            }
            else if (InputStopBandAttenuation > 53 && InputStopBandAttenuation <= 74)
            {
                window = "blackman";
                N = (int)((5.5f * InputFS) / InputTransitionBand);
                if (N % 2 == 0) N++;
                
            }
            OutputHn = new Signal(new List<float>(), new List<int>(), false);

            int index = -N / 2;
            for (int i = 0; i < N; i++)
            {
                OutputHn.SamplesIndices.Add(index);
                index++;
            }
            
            for (int i = 0; i < N; i++)
            {
                double cutoff, cutt1, cutt2;
                double hd, wd;
                int n = Math.Abs(OutputHn.SamplesIndices[i]);
                if (InputFilterType == FILTER_TYPES.LOW)
                {
                    cutoff = ((double)InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hd = 2 * cutoff;
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                    else
                    {
                        hd = 2 * cutoff * (Math.Sin(n * 2 * Math.PI * cutoff) / (n * 2 * Math.PI * cutoff));
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                }
                else if (InputFilterType == FILTER_TYPES.HIGH)
                {
                    cutoff = ((double)InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hd = 1 - (2 * cutoff);
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                    else
                    {
                        hd = -1 * 2 * cutoff * (Math.Sin(n * 2 * Math.PI * cutoff) / (n * 2 * Math.PI * cutoff));
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                }
                else if (InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    cutt1 = (double)(InputF1 - (InputTransitionBand / 2)) / InputFS;
                    cutt2 = (double)(InputF2 + (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hd = 2 * (cutt2 - cutt1);
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                    else
                    {
                        hd = (2 * cutt2 * (Math.Sin(n * 2 * Math.PI * cutt2) / (n * 2 * Math.PI * cutt2)))
                            - (2 * cutt1 * (Math.Sin(n * 2 * Math.PI * cutt1) / (n * 2 * Math.PI * cutt1)));
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                }
                else if (InputFilterType == FILTER_TYPES.BAND_STOP)
                {
                    cutt1 =(double) (InputF1 + (InputTransitionBand / 2)) / InputFS;
                    cutt2 =(double) (InputF2 - (InputTransitionBand / 2)) / InputFS;
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        hd = 1 - (2 * (cutt2 - cutt1));
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                    else
                    {
                        hd = (2 * cutt1 * (Math.Sin(n * 2 * Math.PI * cutt1) / (n * 2 * Math.PI * cutt1)))
                            - (2 * cutt2 * (Math.Sin(n * 2 * Math.PI * cutt2) / (n * 2 * Math.PI * cutt2)));
                        wd = window_calc(window, n, N);
                        OutputHn.Samples.Add((float)(hd * wd));
                    }
                }
            }
            string coofpath = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/coofiecinet.txt";
            using (StreamWriter writer = new StreamWriter(coofpath))
            {
                writer.WriteLine(OutputHn.Samples.Count);
                for (int i = 0; i < OutputHn.Samples.Count; i++)
                {
                    writer.Write(i + "  ");
                    writer.WriteLine(OutputHn.Samples[i]);
                }
            }

            var c = new DirectConvolution
            {
                InputSignal1 = InputTimeDomainSignal,
                InputSignal2 = OutputHn
            };
            c.Run();
            int coun = c.OutputConvolvedSignal.Samples.Count;
            for (int i = 0; i <coun; i++)
            {
                if (c.OutputConvolvedSignal.Samples[i] == 0) c.OutputConvolvedSignal.Samples.RemoveAt(i);
            }
            OutputYn = c.OutputConvolvedSignal;

        }
        public double window_calc(string w, int n, int N)
        {
            double res = 0;
            if (w == "rectangler") res = 1;
            else if (w == "hanning") res = 0.5f + 0.5f * Math.Cos((2 * Math.PI * n) / N);
            else if (w == "hamming") res = 0.54f + 0.46f * Math.Cos((2 * Math.PI * n) / N);
            else if (w == "blackman")
            {
                //var term1 = ;
               // var term2 = ;
                res = (float)(0.42 + (float)(0.5 * Math.Cos((2 * Math.PI * n) / (N - 1))) + (float)(0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));
            }
            return res;
        }
      
    }
}
