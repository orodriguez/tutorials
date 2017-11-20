#r @"..\packages\FParsec.1.0.3\lib\net40-client\FParsecCS.dll"
#r @"..\packages\FParsec.1.0.3\lib\net40-client\FParsec.dll"

open FParsec

let test p str = 
  match run p str with
  | Success(result, _, _)   -> printf "Success: %A" result
  | Failure(errorMsg, _, _) -> printf "Failure: %s" errorMsg

type MeasureFaction = Half | Quarter | Eighth | Sixteenth | Thirtyseconth
type Length = { faction: MeasureFaction; extended: bool }
type Note = A | ASharp | B | C | CSharp | D | DSharp | E | F | FSharp | G | GSharp
type Octave = One | Two | Three
type Sound = Rest | Tone of note: Note * octave: Octave
type Token = { lenght: Length; sound: Sound }

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

let ptoken = pipe2 plength (prest <|> ptone) (fun l t -> { lenght = l; sound= t })

let pscore = sepBy ptoken (pstring " ")

test pscore "8#g2 8e2 8#g2 8#c3 4a2 4- 8#f2 8#d2 8#f2 8b2 4#g2 8#f2 8e2 4- 8e2 8#c2 4#f2 4#c2 4- 8#f2 8e2 4#g2 4#f2"