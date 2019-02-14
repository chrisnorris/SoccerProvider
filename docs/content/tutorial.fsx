(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
(**
Soccer Type Provider
====================

*)
#r @"C:\Git\fsharp\SoccerProvider\src\SoccerTypeProvider\bin\Debug\SoccerTypeProvider.dll"
#r @"C:\Git\fsharp\SoccerProvider\src\SoccerTypeProvider\bin\Debug\FSharp.Data.dll"

open FootballTeams.TypeProvider
open System
open FSharp.Data

FootballTypeProvider.AllLeagues

//type Football = FootballTypeProvider<"league-two">

//Football.Leagues.``Scottish Championship``.``Inverness Caledonian Thistle``.``Goal-difference``

//Football.Leagues.``Europa League``.Akhisarspor.``Goal-difference``
//Football.Leagues.``Champions League``.``Paris Saint Germain``.``Goal-difference``
//Football.Leagues.``Premier League``.``Manchester United``.Position
