module SignalGeneratorTest

open NUnit.Framework
open SignalGenerator

[<TestFixture>]
type ``When generating 2 seconds at 440Hz`` ()=

  [<Test>]
  member this.``there should be 88200 samples`` ()=
    let samples = generateSamples 2000. 440.
    Assert.AreEqual(88200, Seq.length samples)
  
  [<Test>]
  member this.``all samples should be in range`` ()=
    let limit = 32767s
    let samples = generateSamples 2000. 440.
    samples |> Seq.iter (fun s -> Assert.True(s > (-1s * limit)))

type ``When generating 2 seconds at 0Hs`` ()=
  
  [<Test>]
  member this.``the samples should all be 0`` ()=
    let samples = generateSamples 2000. 0.
    let expected = Seq.init 88200 (fun i -> int16 0)
    Assert.AreEqual(expected,  samples)