using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            int N = InputTimeDomainSignal.Samples.Count;
            double Pi = Math.PI;
            float R, I;
            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();
            List<float> Frequencies = new List<float>();
            // x(n)(Cos(θ) - j Sin(θ))
            // R - j I
            // Amp = (R^2 + I^2 )^1/2
            // Φ = tan^-1 (I / R)
            for (int k = 0; k < N; k++)
            {
                R = 0;
                I = 0;
                for (int n = 0; n < N; n++)
                {
                    double th = (2 * k * Pi * n) / N;
                    R += (float)Math.Cos(th) * InputTimeDomainSignal.Samples[n];
                    I += (float)Math.Sin(th) * -1 * InputTimeDomainSignal.Samples[n];
                }
                               
                double Amp2 = (Math.Pow(R, 2) + Math.Pow(I, 2));
                double Amp = Math.Sqrt(Amp2);
                double theta = Math.Atan2(I, R);
                Amplitudes.Add((float)Amp);
                PhaseShifts.Add((float)theta);
            }
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, true);
            OutputFreqDomainSignal.FrequenciesAmplitudes = Amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = PhaseShifts;
          //  OutputFreqDomainSignal.Frequencies = Frequencies;
            //for (int num = 0; num < length; num++)
            //{
            //    float freq = 2 * (float)((Math.PI / (length * (1 / InputSamplingFrequency))) * num);
            //    Frequencies.Add((float)Math.Round(freq, 1));
            //}
        }
    }
}