using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<float> Non_normalized = new List<float>();
            List<float> Normalized = new List<float>();
            if (InputSignal2 == null)
            {
                //Auto_Correlation
                List<float> x = new List<float>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    x.Add(InputSignal1.Samples[i]);
                }
                if (!InputSignal1.Periodic)
                {
                    //Auto_Correlation , Non_Normalized, Non_periodic

                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;
                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(0);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                    }
                    OutputNonNormalizedCorrelation = new List<float>(Non_normalized);
                    //Auto_Correlation , Normalized, Non_periodic
                    float sum1 = 0.0f;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                    }
                    float normalized = (float)Math.Sqrt(sum1 * sum1) / InputSignal1.Samples.Count;
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;

                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(0);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                        Normalized.Add(Non_normalized[j] / normalized);

                    }
                    OutputNormalizedCorrelation = new List<float>(Normalized);
                }
                else if (InputSignal1.Periodic)
                {
                    //Auto_Correlation , Non_Normalized, periodic
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;
                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(x[0]);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                    }
                    OutputNonNormalizedCorrelation = new List<float>(Non_normalized);
                    //Auto_Correlation , Normalized, periodic
                    float sum1 = 0.0f;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                    }
                    float normalized = (float)Math.Sqrt(sum1 * sum1) / InputSignal1.Samples.Count;
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;

                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(x[0]);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                        Normalized.Add(Non_normalized[j] / normalized);

                    }
                    OutputNormalizedCorrelation = new List<float>(Normalized);
                }
            }
            else
            {
                //Cross_Correlation
                List<float> x = new List<float>();
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    x.Add(InputSignal2.Samples[i]);
                }
                if (!InputSignal2.Periodic)
                {
                    //Cross_Correlation , Non_Normalized, Non_periodic 
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;
                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(0);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                    }
                    OutputNonNormalizedCorrelation = new List<float>(Non_normalized);
                    //Cross_Correlation , Normalized, Non_periodic
                    float sum1 = 0.0f;
                    float sum2 = 0.0f;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                        sum2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
                    }
                    float normalized = (float)Math.Sqrt(sum1 * sum2) / InputSignal1.Samples.Count;
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;

                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(0);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                        Normalized.Add(Non_normalized[j] / normalized);

                    }
                    OutputNormalizedCorrelation = new List<float>(Normalized);
                }
                else if (InputSignal2.Periodic)
                {
                    //Cross_Correlation , Non_Normalized, periodic
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;
                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(x[0]);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                    }
                    OutputNonNormalizedCorrelation = new List<float>(Non_normalized);
                    //Cross_Correlation , Normalized, periodic
                    float sum1 = 0.0f;
                    float sum2 = 0.0f;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                        sum2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
                    }
                    float normalized = (float)Math.Sqrt(sum1 * sum2) / InputSignal1.Samples.Count;
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        float sum = 0.0f;

                        for (int n = 0; n < InputSignal1.Samples.Count; n++)
                        {
                            sum += x[n] * InputSignal1.Samples[n];
                        }
                        sum = sum / InputSignal1.Samples.Count;
                        x.Add(x[0]);
                        x.RemoveAt(0);
                        Non_normalized.Add(sum);
                        Normalized.Add(Non_normalized[j] / normalized);
                    }
                    OutputNormalizedCorrelation = new List<float>(Normalized);
                }
            }
        }
    }
}
