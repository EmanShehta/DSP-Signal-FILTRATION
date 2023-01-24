﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            string Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/InputSignal.ds";
            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.WriteLine(0); // time domain
                if (InputSignal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(InputSignal.Samples.Count);
                for(int i = 0; i< InputSignal.Samples.Count; i++)
                {
                    writer.WriteLine(InputSignal.Samples[i]);
                }
            }
            OutputFreqDomainSignal = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            var filter = new FIR
            {
                InputTimeDomainSignal = InputSignal,
                InputFilterType = FILTER_TYPES.BAND_PASS,
                InputF1 = miniF,
                InputF2 = maxF,
                InputStopBandAttenuation = 50,
                InputTransitionBand = 500,
                InputFS = Fs,
            };
            filter.Run();
            var filterd_signal = filter.OutputYn;
            string filted_signal_Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/filteredSignal.ds";
            using (StreamWriter writer = new StreamWriter(filted_signal_Path))
            {
                writer.WriteLine(0); // time domain
                if (filterd_signal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(filterd_signal.Samples.Count);
                for (int i = 0; i < filterd_signal.Samples.Count; i++)
                {
                    writer.WriteLine(filterd_signal.Samples[i]);
                }
            }
            var resampled_signal = filterd_signal;
            if (newFs >= maxF * 2)
            {
                var s = new Sampling
                {
                    InputSignal = filterd_signal,
                    L = L,
                    M = M,
                };
                s.Run();
                resampled_signal = s.OutputSignal;
                Fs = newFs;
            }
            string sampled_signal_Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/resampledSignal.ds";
            using (StreamWriter writer = new StreamWriter(sampled_signal_Path))
            {
                writer.WriteLine(0); // time domain
                if (resampled_signal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(resampled_signal.Samples.Count);
                for (int i = 0; i < resampled_signal.Samples.Count; i++)
                {
                    writer.WriteLine(resampled_signal.Samples[i]);
                }
            }
            var DC = new DC_Component
            {
                InputSignal = resampled_signal
            };
            DC.Run();
            string DC_signal_Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/DCSignal.ds";
            using (StreamWriter writer = new StreamWriter(DC_signal_Path))
            {
                writer.WriteLine(0); // time domain
                if (DC.OutputSignal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(DC.OutputSignal.Samples.Count);
                for (int i = 0; i < DC.OutputSignal.Samples.Count; i++)
                {
                    writer.WriteLine(DC.OutputSignal.Samples[i]);
                }
            }
            var normalized_signal = new Normalizer
            {
                InputSignal = DC.OutputSignal,
                InputMinRange = -1,
                InputMaxRange = 1,
            };
            normalized_signal.Run();
            string normalized_signal_Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/normalizedSignal.ds";
            using (StreamWriter writer = new StreamWriter(normalized_signal_Path))
            {
                writer.WriteLine(0); // time domain
                if (normalized_signal.OutputNormalizedSignal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(normalized_signal.OutputNormalizedSignal.Samples.Count);
                for (int i = 0; i < normalized_signal.OutputNormalizedSignal.Samples.Count; i++)
                {
                    writer.WriteLine(normalized_signal.OutputNormalizedSignal.Samples[i]);
                }
            }
            var Dft = new DiscreteFourierTransform
            {
                InputTimeDomainSignal = normalized_signal.OutputNormalizedSignal,
                InputSamplingFrequency = Fs
            };
            Dft.Run();
            List<float> Frequencies = new List<float>();
            int N = Dft.OutputFreqDomainSignal.FrequenciesAmplitudes.Count;
            for (int i = 0; i < N; i++)
            {
                float omega = 2 * (float)(Math.PI / (N * (1 / Dft.InputSamplingFrequency)));
                omega = (float)Math.Round(omega * i, 1);
                Frequencies.Add(omega);
            }
            Dft.OutputFreqDomainSignal.Frequencies = Frequencies;
            string dft_signal_Path = "E:/DSPToolbox/DSPComponentsUnitTest/TestingSignals/savedSignals/DFTSignal.ds";
            using (StreamWriter writer = new StreamWriter(dft_signal_Path))
            {
                writer.WriteLine(1); // freq domain
                if (Dft.OutputFreqDomainSignal.Periodic) writer.WriteLine(0); // periodic
                else writer.WriteLine(1); // non periodic
                writer.WriteLine(N);
                for (int i = 0; i < N; i++)
                {
                    writer.Write(Dft.OutputFreqDomainSignal.Frequencies[i]);
                    writer.Write("  ");
                    writer.Write(Dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                    writer.Write("  ");
                    writer.WriteLine(Dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                }
            }

            OutputFreqDomainSignal = Dft.OutputFreqDomainSignal;
            
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
