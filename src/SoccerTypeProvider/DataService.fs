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

let leaguesCups = HtmlDocument.Load("http://www.bbc.co.uk/sport/football/leagues-cups")

let leaguesCupsList() =
    let baseHtmlDocumentContent = leaguesCups
    baseHtmlDocumentContent.CssSelect ("ul[data-reactid] a")
    |> Seq.map (fun node -> (node.AttributeValue("href"), node.DirectInnerText()))
    |> Seq.sortBy(fun (_, name) -> name)

let retrieveGroupNames() =
    leaguesCupsList()
    
    |> Seq.fold (fun state group -> String.Concat(snd group, "\n", state)) String.Empty

let getLeaguesForGroup group () = 
    ["league1"; "league2"]
    //let allGroups = groupLeagueMap()
    //allGroups.Item(group)
    //|> Map.toSeq
    //|> Seq.map fst
    //|> List.ofSeq

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
        //int (leagueSourceMap.Item(leagueName)) |> codeToUrl
        "https://www.bbc.co.uk/sport/football/championship/table"
    
    let stripValue (a : seq<HtmlNode>) = 
        a
        |> Seq.map (fun e -> e.InnerText())
        |> Seq.head
    
    let source = HtmlDocument.Load url
    let filterTeamSlug = Seq.filter (fun (e : HtmlNode) -> e.TryGetAttribute(dataTeamSlug) |> Option.isSome)
    (["1"], 
     [
            ({ TeamName = "Yeading AFC"
               Position = 
                   
                   { DirectionOfMove = "up"
                     PositionValue   = 10}
               Played         = 1
               Won            = 1
               Drawn          = 1
               Lost           = 1
               Pro            = 1
               Against        = 1
               GoalDifference = 1
               Points         = 1
               Last10Games    = [|"Game1";"Game2"|]})])
     
