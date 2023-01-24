using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;
namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            float Normalized =0.0f;
            int Length=0;
            float realNumber, imaginaryNumber;
            float amplitude, phase;
            Complex sum,sum2;
            List<Complex> Input_1 = new List<Complex>();
            List<Complex> Input_2 = new List<Complex>();
            List<Complex> Corr_Signal_freq = new List<Complex>();
            List<float> amplitudes = new List<float>();
            List<float> phases = new List<float>();
            List<float> samples = new List<float>();
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            if(InputSignal2 == null)
            {
                Length= InputSignal1.Samples.Count;
                for (int index = 0; index < Length; index++)
                {
                        sum = new Complex(0, 0);
                        for (int num = 0; num < Length; num++)    
                        {
                            // Signal 1 Get DFT
                            float Theta = (float) ((2 * Math.PI * index * num) / Convert.ToDouble(Length));
                            imaginaryNumber  = (float)Math.Sin(Theta); 
                            realNumber = (float)Math.Cos(Theta); 

                            Complex Signal_1 = new Complex(realNumber, -imaginaryNumber); // -J
                            Signal_1= Complex.Multiply(Signal_1, InputSignal1.Samples[num]); // x(n) * e^j
                            sum = Complex.Add(sum, Signal_1);
                       
                        }
                        Input_1.Add(sum);
                }
                
                
            }
            else
            {

                
                int Signal_1_length = InputSignal1.Samples.Count,Signal_2_length = InputSignal2.Samples.Count;
                Length = InputSignal1.Samples.Count;

                if(InputSignal1.Samples.Count != InputSignal2.Samples.Count)
                    Length = Signal_1_length+Signal_2_length -1;

                for(int Signal1=Signal_1_length;Signal1<Length;Signal1++)
                {
                    InputSignal1.Samples.Insert(Signal1,(float)0);
                }
                for(int Signal2=Signal_2_length;Signal2<Length;Signal2++)
                {
                    InputSignal2.Samples.Insert(Signal2,(float)0);
                }
                for (int index = 0; index < Length; index++)
                {
                    sum = new Complex(0, 0);
                    sum2 = new Complex(0, 0);
                    for (int num = 0; num < Length; num++)    
                    {
                        // Signal 1 Get DFT
                        float Theta = (float) ((2 * Math.PI * index * num) / Convert.ToDouble(Length));
                        imaginaryNumber  = (float)Math.Sin(Theta); 
                        realNumber = (float)Math.Cos(Theta); 

                        Complex Signal_1 = new Complex(realNumber, -imaginaryNumber); // -J
                        Signal_1= Complex.Multiply(Signal_1, InputSignal1.Samples[num]); // x(n) * e^j
                        sum = Complex.Add(sum, Signal_1);

                            // Signal 2 Get DFT
                        Complex Signal_2= new Complex(realNumber, -imaginaryNumber); // -J
                        Signal_2 = Complex.Multiply(Signal_2, InputSignal2.Samples[num]); // x(n) * e^j
                        sum2 = Complex.Add(sum2, Signal_2);           
                       
                    }
                    Input_1.Add(sum);
                    Input_2.Add(sum2); 
                }
            }
            


            // Calculate Normalization
            float Sum_signal1=0.0f,Sum_signal2=0.0f;
            for(int i =0;i<Length;i++)
            {
                float value = (float)Math.Pow(InputSignal1.Samples[i],2);
                Sum_signal1+=value;

                if(InputSignal2 != null)
                {
                        
                        float value2 = (float)Math.Pow(InputSignal2.Samples[i],2);
                        Sum_signal2+=value2;
                }
            }
            if(InputSignal2 != null)
                Normalized = (float)Math.Sqrt(Sum_signal1 * Sum_signal2) / Length;
            else
                Normalized = (float)Math.Sqrt(Sum_signal1 * Sum_signal1) / Length;

            // Calculate Correlation In frequence Domain
            for(int Index =0;Index<Length;Index++)
            {
               Complex Signal= new Complex(Input_1[Index].Real,-Input_1[Index].Imaginary);
                if(InputSignal2 == null)
                {
                    Signal = Complex.Multiply(Signal,Input_1[Index]);
                }else
                {
                     Signal = Complex.Multiply(Signal,Input_2[Index]);
                }

                Corr_Signal_freq.Add(Signal);
            }
            // End of Calculation
            

            // Calculate Phase And amplitude
            for (int num = 0; num < Length; num++)
            {
                amplitude = (float)Math.Sqrt(Math.Pow(Corr_Signal_freq[num].Real, 2) + Math.Pow(Corr_Signal_freq[num].Imaginary, 2)); //amp = root real^2 + imag^2
                phase = (float)Math.Atan2(Corr_Signal_freq[num].Imaginary, Corr_Signal_freq[num].Real); // phase = inv tan(imag/real)
                amplitudes.Add(amplitude);
                phases.Add(phase);
            }

            // get IDFT To Back From  Frequence Domain Again 
            for(int k=0; k<Length;k++)
            {
                sum = new Complex(0, 0);
                float summayion=0.0f;
                float realNum,imaginaryNum;
                for (int i = 0; i < Length; i++)
                {
                    realNum = (float) (amplitudes[i] * Math.Cos(phases[i])); //real = amp cos(phase)
                    imaginaryNum = (float) (amplitudes[i] * Math.Sin(phases[i]));//imaginary = amp sin(phase)

                    float θ =(float)((2 * i * Math.PI * k) / Length);
                    float Real = (float)Math.Cos(θ);
                    float I = (float)Math.Sin(θ);
                    Complex  com = new Complex(Real, I);

                    Complex j = new Complex(realNum, imaginaryNum);
                    j = Complex.Multiply(j, com) ; 
                    summayion += (float)(j.Real+j.Imaginary);
                }
                float corr_value = (float)Math.Round(summayion/Length,2) / Length;
                OutputNonNormalizedCorrelation.Add(corr_value);
                OutputNormalizedCorrelation.Add(corr_value/Normalized);
            }
        }
    }
}