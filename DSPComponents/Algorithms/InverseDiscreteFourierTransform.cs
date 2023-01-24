using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> Amplitudes = InputFreqDomainSignal.FrequenciesAmplitudes;
            List<float> PhaseShifts = InputFreqDomainSignal.FrequenciesPhaseShifts;
            int N = InputFreqDomainSignal.Frequencies.Count;
            float Pi = (float)Math.PI;
            List<Complex> X = new List<Complex>();
            List<float> Res = new List<float>();
            float R, I, Real, Imag;
            for (int i = 0; i < N; i++)
            {
                Real = Amplitudes[i] * (float)Math.Cos(PhaseShifts[i]);
                Imag = Amplitudes[i] * (float)Math.Sin(PhaseShifts[i]);
                X.Add(new Complex(Real, Imag));
            }
            for (int n = 0; n < N; n++)
            {
                Complex com, mul;
                float sum = 0;
                for (int k = 0; k < N; k++)
                {
                    float θ = (2 * k * Pi * n) / N;
                    R = (float)Math.Cos(θ);
                    I = (float)Math.Sin(θ);
                    com = new Complex(R, I);
                    mul = Complex.Multiply(X[k], com);
                    sum += (float)(mul.Real + mul.Imaginary);
                }
                Res.Add(sum / N);
            }
            OutputTimeDomainSignal = new Signal(Res, true);
        }
    }
}