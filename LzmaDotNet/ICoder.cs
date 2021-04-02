using System;
using System.IO;

namespace LzmaDotNet
{
	/// <summary>
	///   The exception that is thrown when an error in input stream occurs during decoding.
	/// </summary>
	internal sealed class DataErrorException : ApplicationException
  {
    public DataErrorException() : base("Data Error")
    {
    }
  }

	/// <summary>
	///   The exception that is thrown when the value of an argument is outside the allowable range.
	/// </summary>
	internal sealed class InvalidParamException : ApplicationException
  {
    public InvalidParamException() : base("Invalid Parameter")
    {
    }
  }

  public interface ICodeProgress
  {
	  /// <summary>
	  ///   Callback progress.
	  /// </summary>
	  /// <param name="inSize">
	  ///   input size. -1 if unknown.
	  /// </param>
	  /// <param name="outSize">
	  ///   output size. -1 if unknown.
	  /// </param>
	  void SetProgress(Int64 inSize, Int64 outSize);
  }

  public interface ICoder
  {
	  /// <summary>
	  ///   Codes streams.
	  /// </summary>
	  /// <param name="inStream">
	  ///   input Stream.
	  /// </param>
	  /// <param name="outStream">
	  ///   output Stream.
	  /// </param>
	  /// <param name="inSize">
	  ///   input Size. -1 if unknown.
	  /// </param>
	  /// <param name="outSize">
	  ///   output Size. -1 if unknown.
	  /// </param>
	  /// <param name="progress">
	  ///   callback progress reference.
	  /// </param>
	  /// <exception cref="DataErrorException">
	  ///   if input stream is not valid
	  /// </exception>
	  void Code(Stream inStream, Stream outStream,
      Int64 inSize, Int64 outSize, ICodeProgress progress);
  }

  /*
  public interface ICoder2
  {
     void Code(ISequentialInStream []inStreams,
        const UInt64 []inSizes, 
        ISequentialOutStream []outStreams, 
        UInt64 []outSizes,
        ICodeProgress progress);
  };
  */

  /// <summary>
  ///   Provides the fields that represent properties identifiers for compressing.
  /// </summary>
  public enum CoderPropID
  {
	  /// <summary>
	  ///   Specifies default property.
	  /// </summary>
	  DefaultProp = 0,

	  /// <summary>
	  ///   Specifies size of dictionary.
	  /// </summary>
	  DictionarySize,

	  /// <summary>
	  ///   Specifies size of memory for PPM*.
	  /// </summary>
	  UsedMemorySize,

	  /// <summary>
	  ///   Specifies order for PPM methods.
	  /// </summary>
	  Order,

	  /// <summary>
	  ///   Specifies Block Size.
	  /// </summary>
	  BlockSize,

	  /// <summary>
	  ///   Specifies number of position state bits for LZMA (0 <= x <= 4).
	  /// </summary>
	  PosStateBits,

	  /// <summary>
	  ///   Specifies number of literal context bits for LZMA (0 <= x <= 8).
	  /// </summary>
	  LitContextBits,

	  /// <summary>
	  ///   Specifies number of literal position bits for LZMA (0 <= x <= 4).
	  /// </summary>
	  LitPosBits,

	  /// <summary>
	  ///   Specifies number of fast bytes for LZ*.
	  /// </summary>
	  NumFastBytes,

	  /// <summary>
	  ///   Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
	  /// </summary>
	  MatchFinder,

	  /// <summary>
	  ///   Specifies the number of match finder cycles.
	  /// </summary>
	  MatchFinderCycles,

	  /// <summary>
	  ///   Specifies number of passes.
	  /// </summary>
	  NumPasses,

	  /// <summary>
	  ///   Specifies number of algorithm.
	  /// </summary>
	  Algorithm,

	  /// <summary>
	  ///   Specifies the number of threads.
	  /// </summary>
	  NumThreads,

	  /// <summary>
	  ///   Specifies mode with end marker.
	  /// </summary>
	  EndMarker
  }

  public interface ISetCoderProperties
  {
    void SetCoderProperties(CoderPropID[] propIDs, object[] properties);
  }

  public interface IWriteCoderProperties
  {
    void WriteCoderProperties(Stream outStream);
  }

  public interface ISetDecoderProperties
  {
    void SetDecoderProperties(byte[] properties);
  }
}