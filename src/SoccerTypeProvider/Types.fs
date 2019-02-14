namespace FootballTypeProvider

module Types =
    type Game =
        { Result : string
          NumericResult : string
          Opposition : string
          Date : string }

    type Games = string []

    type PositionSummary =
        { PositionValue : int
          DirectionOfMove : string }

    type TeamData =
        { TeamName : string
          Position : PositionSummary
          Played : int
          Won : int
          Drawn : int
          Lost : int
          Pro : int
          Against : int
          GoalDifference : int
          Points : int
          Last10Games : Games }

    type League =
        { LeagueUrlName : string
          LeagueName : string }

    type TextHelper() = 
        static member val StaticFields = [ "Won"; "Lost"; "Drawn"; "Played"; "Position"; "For"; "Against";"Goal-difference"; "Points" ]
        static member val DirectionOfMoveLabel = "Direction of Move"
        static member val LastTenGamesLabel = "Last10Games"
        static member val LeagueCupsUrl = "https://www.bbc.co.uk/sport/football/leagues-cups"
  

[<AutoOpen>]
[<RequireQualifiedAccess>]
module Array =
    let inline last (arr : _ []) = arr.[arr.Length - 1]
