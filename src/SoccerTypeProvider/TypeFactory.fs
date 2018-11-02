module FootballTypeProvider.TypeFactory

open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open System.Reflection
open System
open FootballTypeProvider.DataService

let buildLeaguesAndTeamStats groupName league = 
    let leagueType = ProvidedTypeDefinition(league, Some typeof<obj>)
    let (teams, teamStats) = retrieveTeams league groupName ()
    leagueType.AddMembersDelayed(fun () -> 
        teamStats |> List.map (fun team -> 
                         let teamType = ProvidedTypeDefinition(team.TeamName, Some typeof<obj>)
                         teamType.AddMembersDelayed
                             (fun () -> 
                             let directionOfMove = team.Position.DirectionOfMove
                             let last10Games = team.Last10Games
                             [ ProvidedProperty
                                   (propertyName = directionOfMoveLabel, propertyType = typeof<string>, isStatic = true, 
                                    getterCode = (fun _ -> <@@ directionOfMove @@>)) ] 
                             @ [ ProvidedProperty
                                     (propertyName = last10GamesLabel, propertyType = typeof<string []>, isStatic = true, 
                                      getterCode = (fun _ -> <@@ last10Games @@>)) ] 
                               @ [ for propertyTuple in (statFields 
                                                         |> List.zip 
                                                                [ team.Won; team.Lost; team.Drawn; team.Played; 
                                                                  team.Position.PositionValue; team.Pro; team.Against; 
                                                                  team.GoalDifference; team.Points ]) do
                                       let propertyValue = fst propertyTuple
                                       yield ProvidedProperty
                                                 (propertyName = snd propertyTuple, propertyType = typeof<int>, 
                                                  isStatic = true, getterCode = (fun args -> <@@ propertyValue @@>)) ])
                         teamType))
    leagueType

let buildLeagues groupName () = 
    let nestedType = ProvidedTypeDefinition("Leagues", Some typeof<obj>, hideObjectMethods = true)
    nestedType.AddMembersDelayed(fun () -> 
        let leagues = getLeaguesForGroup groupName ()
        leagues |> List.map (buildLeaguesAndTeamStats groupName))
    [ nestedType ]
