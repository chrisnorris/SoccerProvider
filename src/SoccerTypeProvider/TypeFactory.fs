module FootballTypeProvider.TypeFactory

open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open System.Reflection
open System
open FootballTypeProvider.DataService
open FootballTypeProvider.Types

let buildLeaguesAndTeamStats groupName (teams, teamStats) league =
    let leagueType = ProvidedTypeDefinition(league.LeagueName, Some typeof<obj>)
    leagueType.AddMembersDelayed(fun () ->
        teamStats
        |> List.map (fun team ->
               let teamType =
                   ProvidedTypeDefinition(team.TeamName, Some typeof<obj>)
               teamType.AddMembersDelayed
                   (fun () ->
                   let directionOfMove = team.Position.DirectionOfMove
                   let last10Games = team.Last10Games
                   [ ProvidedProperty
                         (propertyName = TextHelper.DirectionOfMoveLabel,
                          propertyType = typeof<string>, isStatic = true,
                          getterCode = (fun _ -> <@@ directionOfMove @@>)) ]
                   @ [ ProvidedProperty
                           (propertyName = TextHelper.LastTenGamesLabel,
                            propertyType = typeof<string []>, isStatic = true,
                            getterCode = (fun _ -> <@@ last10Games @@>)) ]
                     @ [ for propertyTuple in (TextHelper.StaticFields
                                               |> List.zip
                                                      [ team.Won; team.Lost;
                                                        team.Drawn; team.Played;
                                                        team.Position.PositionValue;
                                                        team.Pro; team.Against;
                                                        team.GoalDifference;
                                                        team.Points ]) do
                             let propertyValue = fst propertyTuple
                             yield ProvidedProperty
                                       (propertyName = snd propertyTuple,
                                        propertyType = typeof<int>,
                                        isStatic = true,
                                        getterCode = (fun _ ->
                                        <@@ propertyValue @@>)) ])
               teamType))
    leagueType

let buildLeagues groupName () =
    let nestedType =
        ProvidedTypeDefinition
            ("Leagues", Some typeof<obj>, hideObjectMethods = true)
    nestedType.AddMembersDelayed
        (fun () ->
        let leagues = getLeaguesForGroup groupName ()
        leagues
        |> List.map
               (fun league ->
               (retrieveTeams league.LeagueUrlName groupName (), league))
        |> List.filter
               (fun retrievedDataOption -> (fst retrievedDataOption).IsSome)
        |> List.map
               (fun retrievedData ->
               let (teams, teamStats) = (fst retrievedData).Value
               buildLeaguesAndTeamStats groupName (teams, teamStats)
                   (snd retrievedData)))
    [ nestedType ]
