using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
       
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //int n = InputSignal2.Samples.Count + InputSignal1.Samples.Count - 1

            int Signal_1_length = InputSignal1.Samples.Count,Signal_2_length = InputSignal2.Samples.Count;
            int Total_length = Signal_1_length+Signal_2_length -1;
             for(int Signal1=Signal_1_length;Signal1<Total_length;Signal1++)
                {
                    InputSignal1.Samples.Insert(Signal1,(float)0);
                     InputSignal1.SamplesIndices.Add(Signal1);
                }
            for(int Signal2=Signal_2_length;Signal2<Total_length;Signal2++)
                {
                 InputSignal2.Samples.Insert(Signal2,(float)0);
                    InputSignal2.SamplesIndices.Add(Signal2);
                }

        
            
            DiscreteFourierTransform DF1 = new DiscreteFourierTransform();
            DiscreteFourierTransform DF2 = new DiscreteFourierTransform();
            DF1.InputTimeDomainSignal = InputSignal1;
            DF2.InputTimeDomainSignal = InputSignal2;
            DF1.Run();
            DF2.Run();
            Signal s1 = DF1.OutputFreqDomainSignal;
            Signal s2 = DF2.OutputFreqDomainSignal;

            List<float> Amplitudes1 = s1.FrequenciesAmplitudes;
            List<float> PhaseShifts1 = s1.FrequenciesPhaseShifts;

            List<float> Amplitudes2 =s2.FrequenciesAmplitudes;
            List<float> PhaseShifts2 =s2.FrequenciesPhaseShifts;

            int N1 = DF1.InputTimeDomainSignal.Samples.Count;
            int N2 = DF2.InputTimeDomainSignal.Samples.Count;
            float Pi = (float)Math.PI;
            List<Complex> X1 = new List<Complex>();
            List<Complex> X2 = new List<Complex>();
        
        
            float R, I, Real1, Imag1, Real2, Imag2;
            for (int i = 0; i < N1; i++)
            {
                Real1 = Amplitudes1[i] * (float)Math.Cos(PhaseShifts1[i]);
                Imag1 = Amplitudes1[i] * (float)Math.Sin(PhaseShifts1[i]);
                X1.Add(new Complex(Real1, Imag1));


                Real2 = Amplitudes2[i] * (float)Math.Cos(PhaseShifts2[i]);
                Imag2 = Amplitudes2[i] * (float)Math.Sin(PhaseShifts2[i]);
                X2.Add(new Complex(Real2, Imag2));
            }

            List<Complex> out_conv_freq = new List<Complex>();
            List<float> index= new List<float>();

            for(int i = 0 ; i <Total_length;i++)
             {
                   Complex Sum = new Complex();
                   
                    Sum = Complex.Multiply(X1[i],X2[i]);

                out_conv_freq.Add(Sum);
             }
       

            List<float> out_amp = new List<float>();
            List<float> out_phase = new List<float>();

            R = 0; I=0;
                for (int n = 0; n < out_conv_freq.Count; n++)
                {
                     R = (float)out_conv_freq[n].Real;
                     I = (float)out_conv_freq[n].Imaginary;
                    float Amp2 = (float)(Math.Pow(R, 2) + Math.Pow(I, 2));
                    float Amp = (float)Math.Sqrt(Amp2);
                    float Φ = (float)Math.Atan2(I, R);
                    out_amp.Add(Amp);
                    out_phase.Add(Φ);
                    index.Add(n);
            }
               
              
            
            Signal Input_IDF = new Signal(true, index, out_amp,out_phase);
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = Input_IDF;
            IDFT.Run();
            OutputConvolvedSignal = new Signal(IDFT.OutputTimeDomainSignal.Samples,false);
        }
    }
}