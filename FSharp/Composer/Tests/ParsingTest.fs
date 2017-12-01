module ParsingTest

open Xunit
open Parsing


type ``when parsing a score`` ()=

  [<Fact>]
  member this.``it should parse a simple score`` ()=
      let score = "32.#d3 16-"
      let result = parse score
      let assertFirstToken token =
        Assert.Equal({fraction = Thirtyseconth; extended = true}, token.length)
        Assert.Equal(Tone (DSharp,Three), token.sound)
      let assertSecondToken token =
          Assert.Equal({fraction = Sixteenth; extended = false}, token.length)
          Assert.Equal(Rest, token.sound)

      match result with
          | Choice2Of2 errorMsg -> Assert.True(false, errorMsg)
          | Choice1Of2 tokens ->
              Assert.Equal(2, List.length tokens)
              printfn "%A" tokens
              List.head tokens |> assertFirstToken