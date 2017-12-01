module Parsing

open FParsec

let test p str = 
  match run p str with
  | Success(result, _, _)   -> printf "Success: %A" result
  | Failure(errorMsg, _, _) -> printf "Failure: %s" errorMsg

type MeasureFaction = Half | Quarter | Eighth | Sixteenth | Thirtyseconth
type Length = { fraction: MeasureFaction; extended: bool }
type Note = A | ASharp | B | C | CSharp | D | DSharp | E | F | FSharp | G | GSharp
type Octave = One | Two | Three
type Sound = Rest | Tone of note: Note * octave: Octave
type Token = { length: Length; sound: Sound }

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
    (fun t e -> { fraction = t; extended = e })

let pnotsharpablenote = anyOf "be" |>> (function 
  | 'b' -> B
  | 'e' -> E
  | unknown -> sprintf "Unknown note %c" unknown |> failwith)

let psharp = (stringReturn "#" true) <|> (stringReturn "" false)

let psharpnote = pipe2 
                  psharp 
                  (anyOf "acdfg") 
                  (fun isSharp note -> 
                    match (isSharp, note) with
                    | (false, 'a') -> A
                    | (true, 'a') -> ASharp
                    | (false, 'c') -> C
                    | (true, 'c') -> CSharp
                    | (false, 'd') -> D
                    | (true, 'd') -> DSharp
                    | (false, 'f') -> F
                    | (true, 'f') -> FSharp
                    | (false, 'g') -> G
                    | (true, 'g') -> GSharp
                    | (_, unknown) -> sprintf "Unknow note %c" unknown |> failwith)

let pnote = pnotsharpablenote <|> psharpnote

let poctave = anyOf "123" |>> (function
  | '1' -> One
  | '2' -> Two
  | '3' -> Three
  | unknown -> sprintf "Unknown octive %c" unknown |> failwith)

let ptone = pipe2 pnote poctave (fun n o -> Tone(note = n, octave = o))

let prest = stringReturn "-" Rest

let ptoken = pipe2 plength (prest <|> ptone) (fun l t -> { length = l; sound= t })

let pscore = sepBy ptoken (pstring " ")

let parse (score:string) =    
  match score.Trim() |> run pscore with
    | Success(result, _, _)   -> Choice1Of2 result
    | Failure(errorMsg, _, _) -> Choice2Of2 errorMsg