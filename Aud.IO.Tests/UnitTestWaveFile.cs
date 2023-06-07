using Aud.IO.Formats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aud.IO.Tests
{
    [TestClass]
    public class UnitTestWaveFile
    {
        [TestMethod]
        [DeploymentItem(@"fixture\440 kHz frequency 441 kHz samplerate sinus new.wav")]
        public void TestWaveFileHeader()
        {
            WaveFile audioFile = null;
            try
            {
                audioFile = new WaveFile(@"fixture\440 kHz frequency 441 kHz samplerate sinus new.wav");
            }
            catch (System.Exception)
            {
                Assert.Fail();
            }

            WaveStructure data = audioFile.GetWaveData();

            // Has a samplerate of 44100 Hz
            Assert.AreEqual<uint>(44100, audioFile.SampleRate);
            // There should be a total of 88200 samples in the file.
            Assert.AreEqual<int>(88200, audioFile.Samples);
            // Verify that it is mono-channel audio.
            Assert.AreEqual<ushort>(1, data.Subchunk1.NumChannels);
            // 16 bits per sample.
            Assert.AreEqual<ushort>(16, data.Subchunk1.BitsPerSample);
        }
    }
}
