module SignalGeneratorTest

open Xunit
open SignalGenerator

type ``When generating 2 seconds at 440Hz`` ()=

  [<Fact>]
  member this.``there should be 88200 samples`` ()=
    let samples = generateSamples 2000. 440.
    Assert.Equal(88200, Seq.length samples)
