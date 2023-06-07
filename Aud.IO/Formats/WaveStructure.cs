using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Aud.IO.Formats
{
    /// <summary>
    /// A structure that represents a Wave file, with the correct layout in memory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct WaveStructure
    {
        /// <summary>
        /// Magic header, used to identify header.
        /// Should spell out: "RIFF" in ASCII.
        /// It spells "RIFX" instead, if the file is written with big-endianess.
        /// Big-endian ASCII-string.
        /// </summary>
        public readonly uint ChunkID;

        /// <summary>
        /// The total size of all fields (including subchunks) beyond this field.
        /// </summary>
        public readonly uint ChunkSize;

        /// <summary>
        /// Magic indicator, used to idenity the subtype of audio format.
        /// Should always spell the word "WAVE" in ASCII.
        /// Big-endian ASCII-string.
        /// </summary>
        public readonly uint Format;

        /// <summary>
        /// Format sub-chunk.
        /// </summary>
        public readonly FormatSubchunk Subchunk1;

        /// <summary>
        /// Data sub-chunk.
        /// </summary>
        public readonly DataSubchunk Subchunk2;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveStructure"/> struct.
        /// Create structure only using explicitly needed parameters.
        /// </summary>
        /// <param name="numChannels">The amount of channels, 1 corresponds to mono, 2 corresponds to stereo.</param>
        /// <param name="sampleRate">The amount of samples provided per second.</param>
        /// <param name="bitsPerSample">The amount of bits a single sample uses.</param>
        /// <param name="data">PCM data.</param>
        public WaveStructure(ushort numChannels, uint sampleRate, ushort bitsPerSample, byte[] data)
        {
            ChunkID = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("RIFF"), 0);
            Format = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("WAVE"), 0);

            Subchunk1 = new FormatSubchunk(numChannels, sampleRate, bitsPerSample);
            Subchunk2 = new DataSubchunk(data);

            ChunkSize = sizeof(uint) + (8 + Subchunk1.Size) + (8 + Subchunk2.Size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveStructure"/> struct.
        /// Create structure by providing subchunks.
        /// </summary>
        /// <param name="formatSubchunk">The subchunk containing metadata.</param>
        /// <param name="dataSubchunk">The subchunk containing PCM data.</param>
        public WaveStructure(FormatSubchunk formatSubchunk, DataSubchunk dataSubchunk)
        {
            ChunkID = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("RIFF"), 0);
            Format = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("WAVE"), 0);

            Subchunk1 = formatSubchunk;
            Subchunk2 = dataSubchunk;

            ChunkSize = sizeof(uint) + (8 + Subchunk1.Size) + (8 + Subchunk2.Size);
        }

        /// <summary>
        /// Returnerer den samlede størrelse af WaveStruktur.
        /// </summary>
        public uint TotalSize => sizeof(uint) + (8 + Subchunk1.Size) + (8 + Subchunk2.Size);
    }

    /// <summary>
    /// Describes the audio format.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public readonly struct FormatSubchunk
    {
        /// <summary>
        /// Contains the letters "fmt " in ASCII.
        /// Big-endian ASCII-string.
        /// </summary>
        [FieldOffset(0)]
        public readonly uint ID;

        /// <summary>
        /// The total size of all fields beyond this field.
        /// </summary>
        [FieldOffset(4)]
        public readonly uint Size;

        /// <summary>
        /// Always 1.
        /// </summary>
        [FieldOffset(8)]
        public readonly ushort AudioFormat;

        /// <summary>
        /// Mono = 1, stereo = 2.
        /// </summary>
        [FieldOffset(10)]
        public readonly ushort NumChannels;

        /// <summary>
        /// Samplerate.
        /// </summary>
        [FieldOffset(12)]
        public readonly uint SampleRate;

        /// <summary>
        /// <c>SampleRate</c> * <c>NumChannels</c> * (<c>BitsPerSample</c> / 8)
        /// </summary>
        [FieldOffset(16)]
        public readonly uint ByteRate;

        /// <summary>
        /// <c>NumChannels</c> * (<c>BitsPerSample</c> / 8)
        /// </summary>
        [FieldOffset(20)]
        public readonly ushort BlockAlign;

        /// <summary>
        /// The size of a single sample in bits.
        /// </summary>
        [FieldOffset(22)]
        public readonly ushort BitsPerSample;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatSubchunk"/> struct.
        /// Create structure only using explicitly needed parameters, the rest is calculated.
        /// </summary>
        /// <param name="numChannels">The amount of channels, 1 corresponds to mono, 2 corresponds to steoro.</param>
        /// <param name="sampleRate">The amount of samples provided per second.</param>
        /// <param name="bitsPerSample">The size of a single sample in bits.</param>
        public FormatSubchunk(ushort numChannels, uint sampleRate, ushort bitsPerSample)
        {
            ID = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("fmt "), 0);
            Size = (sizeof(ushort) * 4) + (sizeof(uint) * 2);
            AudioFormat = 1;
            NumChannels = numChannels;
            SampleRate = sampleRate;
            ByteRate = (uint)(sampleRate * numChannels * (bitsPerSample / 8));
            BlockAlign = (ushort)(numChannels * (bitsPerSample / 8));
            BitsPerSample = bitsPerSample;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatSubchunk"/> struct.
        /// </summary>
        /// <param name="audioFormat">Always 1.</param>
        /// <param name="numChannels">The amount of channels, 1 corresponds to mono, 2 corresponds to steoro.</param>
        /// <param name="sampleRate">The amount of samples provided every second.</param>
        /// <param name="byteRate">The amount of data in bytes provided every second.</param>
        /// <param name="blockAlign"></param>
        /// <param name="bitsPerSample">The size of a single sample in bits.</param>
        public FormatSubchunk(ushort audioFormat, ushort numChannels, uint sampleRate, uint byteRate, ushort blockAlign, ushort bitsPerSample)
        {
            ID = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("fmt "), 0);
            Size = (sizeof(ushort) * 4) + (sizeof(uint) * 2);

            AudioFormat = audioFormat;
            NumChannels = numChannels;
            SampleRate = sampleRate;
            ByteRate = byteRate;
            BlockAlign = blockAlign;
            BitsPerSample = bitsPerSample;
        }
    }

    /// <summary>
    /// Stores the PCM-data.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct DataSubchunk
    {
        /// <summary>
        /// Always contains the text "data" in ASCII.
        /// Big-endian ASCII-string.
        /// </summary>
        [FieldOffset(0)]
        public readonly uint ID;

        /// <summary>
        /// <c>NumSamples</c> * <c>NumChannels</c> * (<c>BitsPerSample</c> / 8)
        /// </summary>
        [FieldOffset(4)]
        public readonly uint Size;

        /// <summary>
        /// PCM-data.
        /// </summary>
        [FieldOffset(8)]
        public readonly byte[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSubchunk"/> struct.
        /// </summary>
        /// <param name="data">PCM-data.</param>
        public DataSubchunk(byte[] data)
        {
            ID = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("data"), 0);
            Size = (uint)data.Length;
            Data = data;
        }
    }
}
