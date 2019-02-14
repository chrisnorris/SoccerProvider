module internal FootballTypeProvider.DataService

open FSharp.Data
open FSharp.Collections
open System
open FootballTypeProvider.Types
open FootballTypeProvider

let groupLeagueMap() : Map<string, Map<string, string>> =
    let baseHtmlDocumentContent =
        HtmlDocument.Load(TextHelper.LeagueCupsUrl)
    baseHtmlDocumentContent.Descendants [ "optgroup" ]
    |> Seq.map (fun node ->
           (node.AttributeValue( "label" ),
            node.Descendants [ "option" ]
            |> Seq.map
                   (fun node ->
                   (node.InnerText().Trim(),
                    Seq.last <| node.AttributeValue("value").Split('-')))
            |> Map.ofSeq))
    |> (Seq.cache >> Map.ofSeq)

let leaguesList() =
    let leagueData =
        HtmlDocument.Load("https://www.bbc.co.uk/sport/football/teams")
    leagueData.CssSelect("div[data-reactid*=group]")
    |> Seq.map (fun (e : HtmlNode) -> e.CssSelect("h2").[0].InnerText())

let retrieveGroupNames() =
    let leagues = leaguesList()
    leagues
    |> Seq.zip ([ 1..(Seq.length leagues) ])
    |> Seq.sortBy (fst)
    |> Seq.fold (fun state group ->
           String.Concat(sprintf "\n%d> %s" (fst group) (snd group), state))
           String.Empty

let leaguesCupsList() =
    let baseHtmlDocumentContent =
        HtmlDocument.Load("http://www.bbc.co.uk/sport/football/leagues-cups")
    baseHtmlDocumentContent.CssSelect("ul[data-reactid] a")
    |> Seq.map
           (fun node -> (node.AttributeValue("href"), node.DirectInnerText()))
    |> Seq.sortBy (fun (_, name) -> name)

type TeamRowStats(rows : HtmlNode) =
    member __.rows = rows

    member __.getNthReactRow ix =
        let a =
            rows.CssSelect(sprintf "[data-reactid*='td-%d']" ix) |> List.head
        a.DirectInnerText() |> int

    member __.NotSure = [ 3..10 ] |> List.map (__.getNthReactRow)
    member __.Played = __.getNthReactRow 3
    member __.Won = __.getNthReactRow 4
    member __.Drawn = __.getNthReactRow 5
    member __.Lost = __.getNthReactRow 6
    member __.Pro = __.getNthReactRow 7
    member __.Against = __.getNthReactRow 8
    member __.GoalDifference = __.getNthReactRow 9
    member __.Points = __.getNthReactRow 10

let getLeaguesForGroup group () =
    leaguesCupsList()
    |> Seq.map (fun e ->
           { LeagueUrlName = (fst e).Split('/') |> Array.last
             LeagueName = snd e })
    |> List.ofSeq

let getLeaguesForGroup2 group () =
    let allGroups = groupLeagueMap()
    allGroups.Item(group)
    |> Map.toSeq
    |> Seq.map fst
    |> List.ofSeq

let getNthReactRow (node : HtmlNode) ix =
    let a = node.CssSelect(sprintf "[data-reactid*='td-%d']" ix) |> List.head
    a.DirectInnerText() |> int

let retrieveTeams leagueNamegroupName leagueName () =
    let url = sprintf "https://www.bbc.co.uk/sport/football/%s/table" leagueName
    try
        let source = HtmlDocument.Load url
        let r = source.CssSelect("tbody tr")

        let teams =
            (r
             |> List.map (fun (e : HtmlNode) ->
                    let dataColumns = e.Descendants(fun e -> e.HasName("abbr"))
                    dataColumns |> Seq.map (fun f -> f.AttributeValue("title"))))
            |> Seq.concat

        let r2 : seq<TeamData> =
            seq {
                for e in r do
                    let x = TeamRowStats e
                    yield ({ TeamName =
                                 e.Descendants(fun e -> e.HasName("abbr"))
                                 |> Seq.map (fun f -> f.AttributeValue("title"))
                                 |> Seq.head
                             Position =
                                 { DirectionOfMove = "up"
                                   PositionValue = 10 }
                             Played = x.Played
                             Won = getNthReactRow e 4
                             Drawn = getNthReactRow e 5
                             Lost = getNthReactRow e 6
                             Pro = getNthReactRow e 7
                             Against = getNthReactRow e 8
                             GoalDifference = getNthReactRow e 9
                             Points = getNthReactRow e 10
                             Last10Games = [| "Game1"; "Game2" |] })
            }

        Some(teams |> List.ofSeq, r2 |> List.ofSeq)
    with _ -> None
