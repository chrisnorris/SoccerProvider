module internal FootballTypeProvider.Types

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
