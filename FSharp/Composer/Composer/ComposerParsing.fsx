#r @"..\packages\FParsec.1.0.3\lib\net40-client\FParsecCS.dll"
#r @"..\packages\FParsec.1.0.3\lib\net40-client\FParsec.dll"

open FParsec

let test p str = 
  match run p str with
  | Success(result, _, _)   -> printf "Success: %A" result
  | Failure(errorMsg, _, _) -> printf "Failure: %s" errorMsg

type MeasureFaction = Half | Quarter | Eighth | Sixteenth | Thirtyseconth
type Length = { faction: MeasureFaction; extended: bool }
type Note = A | ASharp | B | C | CSharp | DSharp | E | F | FSharp | G | GSharp
type Octave = One | Two | Three
type Sound = Rest | Tone of node: Note * octave: Octave
type Token = { lenght: Length; sound: Sound }

let aspiration = "32.#d3"

let pmeasurefaction = 
  (stringReturn "2" Half)
  <|> (stringReturn "4" Quarter)
  <|> (stringReturn "8" Eighth)
  <|> (stringReturn "16" Sixteenth)
  <|> (stringReturn "32" Thirtyseconth)

let pextendedparser = (stringReturn "." true) <|> (stringReturn "" false)

let plength = 
  pipe2
    pmeasurefaction
    pextendedparser
    (fun t e -> { faction = t; extended = e })

test plength aspiration
