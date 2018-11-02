(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Introducing your project
========================

Say more

*)
#r @"C:\Git\fsharp\SoccerProvider\src\SoccerTypeProvider\bin\Debug\SoccerTypeProvider.dll"
#r @"C:\Git\fsharp\SoccerProvider\src\SoccerTypeProvider\bin\Debug\FSharp.Data.dll"

open FootballTeams.TypeProvider


FootballTypeProvider.AvailableGroups



open System
open FSharp.Data

type Football = FootballTypeProvider<Group="English">

Football.Leagues
