module WavePackerTest
open Xunit
open WavePacker
open System.IO
open SignalGenerator
open System.Text

type ``when packing an audio file`` ()=
  let getFile millisecods =
    generateSamples millisecods 440.
    |> Array.ofSeq
    |> pack
    |> (fun ms ->
          ms.Seek(0L, SeekOrigin.Begin) |> ignore
          ms)
  
  [<Fact>]
  member this.``the stream should start with 'RIFF'`` ()=
    let file = getFile 2000.
    let bucket = Array.zeroCreate 4
    file.Read(bucket, 0, 4) |> ignore
    let first4Chars = Encoding.ASCII.GetString(bucket)
    Assert.Equal("RIFF", first4Chars)
  
  [<Fact>]
  member this.``file size is corrent`` ()=
    let formatOverhead = 44.
    let audioLengths = [2000.; 50.; 1500.; 3000.]
    let files = List.zip audioLengths (List.map getFile audioLengths)
    
    let assertLength (length, file:MemoryStream) =
      let expected = (length/1000.) * 44100. * 2. + formatOverhead
      Assert.Equal(float expected, float file.Length)

    List.iter assertLength files
