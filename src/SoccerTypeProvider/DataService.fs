module internal FootballTypeProvider.DataService

open FSharp.Data
open System
open FootballTypeProvider.Types

let teamName = "team-name"
let dataTeamSlug = "data-team-slug"
let codeToUrl = sprintf @"http://www.bbc.co.uk/sport/football/tables/partial/%d?structureid=5"
let results() = HtmlDocument.Load("http://www.bbc.co.uk/sport/football/tables")
let statFields = [ "Won"; "Lost"; "Drawn"; "Played"; "Position"; "For"; "Against"; "Goal-difference"; "Points" ]
let directionOfMoveLabel = "Direction of Move"
let last10GamesLabel = "Last10Games"

let groupLeagueMap() : Map<string, Map<string, string>> = 
    let baseHtmlDocumentContent = results()
    baseHtmlDocumentContent.Descendants [ "optgroup" ]
    |> Seq.map (fun node -> 
           (node.AttributeValue("label"), 
            node.Descendants [ "option" ]
            |> Seq.map (fun node -> (node.InnerText().Trim(), Seq.last <| node.AttributeValue("value").Split('-')))
            |> Map.ofSeq))
    |> (Seq.cache >> Map.ofSeq)

let retrieveGroupNames() = 
    groupLeagueMap()
    |> Map.toSeq
    |> Seq.map fst
    |> Seq.fold (fun state group -> String.Concat(group, "\n", state)) String.Empty

let getLeaguesForGroup group () = 
    let allGroups = groupLeagueMap()
    allGroups.Item(group)
    |> Map.toSeq
    |> Seq.map fst
    |> List.ofSeq

let stripValue (nodeList : seq<HtmlNode>) = 
    nodeList
    |> Seq.map (fun e -> e.InnerText())
    |> Seq.head

let extractClassNamed withClassName (t : HtmlNode) = 
    int (t.Descendants(fun e -> e.HasClass(withClassName)) |> stripValue)
let nab a b = not (a = b)

let retrieveTeams leagueName groupName () = 
    let url = 
        let leagueSourceMap = groupLeagueMap().Item(groupName)
        int (leagueSourceMap.Item(leagueName)) |> codeToUrl
    
    let stripValue (a : seq<HtmlNode>) = 
        a
        |> Seq.map (fun e -> e.InnerText())
        |> Seq.head
    
    let source = HtmlDocument.Load url
    let filterTeamSlug = Seq.filter (fun (e : HtmlNode) -> e.TryGetAttribute(dataTeamSlug) |> Option.isSome)
    (source.Descendants [ "tr" ]
     |> filterTeamSlug
     |> Seq.map (fun t -> t.Descendants(fun e -> e.HasClass(teamName)) |> stripValue)
     |> List.ofSeq, 
     source.Descendants [ "tr" ]
     |> filterTeamSlug
     |> Seq.map (fun nodes -> 
            ({ TeamName = (nodes.Descendants(fun e -> e.HasClass(teamName)) |> stripValue)
               Position = 
                   let spans = 
                       nodes.Descendants(fun e -> e.HasClass("position"))
                       |> Seq.map (fun e -> e.Descendants [ "span" ] |> Seq.map (fun e -> e.InnerText()))
                       |> Seq.concat
                   { DirectionOfMove = (Seq.head spans)
                     PositionValue   = int (Seq.last spans) }
               Played         = extractClassNamed "played" nodes
               Won            = extractClassNamed "won" nodes
               Drawn          = extractClassNamed "drawn" nodes
               Lost           = extractClassNamed "lost" nodes
               Pro            = extractClassNamed "for" nodes
               Against        = extractClassNamed "against" nodes
               GoalDifference = extractClassNamed "goal-difference" nodes
               Points         = extractClassNamed "points" nodes
               Last10Games    = 
                   nodes.Descendants [ "li" ]
                   |> Seq.map (fun d -> d.AttributeValue("title"))
                   |> Array.ofSeq }))
     |> List.ofSeq)
