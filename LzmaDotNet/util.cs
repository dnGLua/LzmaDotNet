using System.IO;
using LzmaDotNet.Compress.LZMA;

namespace LzmaDotNet
{
  public static class util
  {
    // Reverse-engineered values (February 2021):
    /*
    int level;          // 0 <= level <= 9; default 5
    uint dictSize;      // (1 << 12) <= dictSize <= (1 << 27) for 32-bit version
                        // (1 << 12) <= dictSize <= (1 << 30) for 64-bit version
                        // default = (1 << 16)
    int lc;             // number of literal context bits; 0 <= lc <= 8; default = 3
    int lp;             // number of literal pos bits; 0 <= lp <= 4; default = 0
    int pb;             // number of pos bits; 0 <= pb <= 4; default = 2
    int algo;           // 0 - fast, 1 - normal, 2 - ???; default = 1
    int fb;             // number of fast bytes; 5 <= fb <= 273; default = 32
    int btMode;         // 0 - HashChain mode, 1 - BinaryTree mode; default = 1
    int numHashBytes;   // 2, 3 or 4; default = 4
    uint mc;            // 1 <= mc <= (1 << 30); default = 32
    bool eos;           // write end-of-stream marker; 0 - do not write EOPM, 1 - write EOPM; default = 0
    int numThreads;     // 1 or 2; default = 2 (multi-threaded)
    */
    private const int dictSize = 1 << 16; // 0x10000
    private const int pb = 2;
    private const int lc = 3;
    private const int lp = 0;
    private const int algo = 1; // Note: GMod uses 2 for this, but we gotta use 1.
    private const int fb = 32;
    private const string mf = "bt4";

    private const bool eos = false;
    //const int numThreads = 2;

    public static bool Compress(Stream inStream, Stream outStream)
    {
      CoderPropID[] propIDs =
      {
        CoderPropID.DictionarySize,
        CoderPropID.PosStateBits,
        CoderPropID.LitContextBits,
        CoderPropID.LitPosBits,
        CoderPropID.Algorithm,
        CoderPropID.NumFastBytes,
        CoderPropID.MatchFinder,
        CoderPropID.EndMarker
        //CoderPropID.NumThreads
      };

      object[] properties =
      {
        dictSize,
        pb,
        lc,
        lp,
        algo,
        fb,
        mf,
        eos
        //numThreads
      };

      var encoder = new Encoder();
      encoder.SetCoderProperties(propIDs, properties);
      encoder.WriteCoderProperties(outStream);

      var fileSize = inStream.Length;
      for (var i = 0; i < 8; i++) outStream.WriteByte((byte) (fileSize >> (8 * i)));

      encoder.Code(inStream, outStream, -1, -1, null);

      return true;
    }

    public static bool Decompress(Stream input, Stream output, bool isInputClose = true, bool isOutputClose = true)
    {
      var decoder = new Decoder();

      var properties = new byte[5];
      if (input.Read(properties, 0, 5) != 5)
        return false; //throw new Exception("input .lzma is too short");
      decoder.SetDecoderProperties(properties);

      var outSize = 0L;
      for (var i = 0; i < 8; i++)
      {
        var v = input.ReadByte();
        if (v < 0)
          return false; //throw new Exception("Can't read 1");
        outSize |= (long) (byte) v << (8 * i);
      }

      var compressedSize = input.Length - input.Position;
      decoder.Code(input, output, compressedSize, outSize, null);

      if (isInputClose) input.Close();
      if (isOutputClose) output.Close();

      return true;
    }
  }
}