# Soccer Type Provider

  Provides typed access to soccer tables, teams and statistics from the [BBC Sports website.][2]  
  Inspired by [Phil Trelford's demo of the HTML Type provider.][1]  
  Please use msbuild via Visual Studio or equiv., yet to implement FAKE.

## Getting started

```F#
#r "./bin/debug/SoccerTypeProvider.dll"

open FootballTeams.TypeProvider

FootballTypeProvider.AvailableGroups
> val it : string =
  "Women
   Welsh
   Scottish
   Irish
   International
   European & World
   English"

type Football = FootballTypeProvider<Group="English">
Football.Leagues.``Premier League``.Leicester.Points
> val it : int 57

Football.Leagues.``National League South``.``Hayes & Yeading``.Played
> val it : int 6

type Football = FootballTypeProvider<Group="Women">
Football.Leagues.``Women's Premier South``.``Forest Green Rovers LFC``.Position
> val it : int = 11

type Football = FootballTypeProvider<Group="Welsh">
Football.Leagues.``Welsh Premier League``.``Airbus UK Broughton``.``Direction of Move``
> val it : string = "No movement"

Football.Leagues.``Welsh Premier League``.``Gap Connah's Quay``.``Goal-difference``
> val it : int = 9

type Football = FootballTypeProvider<Group="Scottish">
Football.Leagues.``Lowland League``.``Dalbeattie Star``.Last10Games
> val it : string [] =
  [|"Loss v Edinburgh City 5 - 2 on 3rd October 2015";
    "Win v Threave Rovers 7 - 1 on 10th October 2015";
    "Win v Edinburgh City 1 - 0 on 17th October 2015";
    "Loss v Cumbernauld Colts 0 - 2 on 31st October 2015";
    "Win v Edinburgh University 1 - 2 on 7th November 2015";
    "Draw v Edinburgh University 2 - 2 on 21st November 2015";
    "Loss v Selkirk 2 - 3 on 13th February 2016";
    "Draw v East Kilbride 2 - 2 on 20th February 2016";
    "Win v Threave Rovers 3 - 4 on 24th February 2016";
    "Loss v Cumbernauld Colts 4 - 0 on 27th February 2016"|]

type Football = FootballTypeProvider<Group="International">
Football.Leagues.``European Championship Qualifying``.Armenia.Lost
> val it : int = 6

type Football = FootballTypeProvider<Group="European & World">
Football.Leagues.``Danish Superliga``.``FC Nordsjælland``.Last10Games
> val it : string [] =
  [|"Win v AGF Aarhus 2 - 0 on 27th September 2015";
    "Loss v Odense Boldklub 1 - 5 on 2nd October 2015";
    "Loss v AGF Aarhus 3 - 0 on 18th October 2015";
    "Win v Aalborg BK 3 - 0 on 25th October 2015";
    "Loss v Esbjerg fB 2 - 1 on 30th October 2015";
    "Win v Hobro IK 0 - 1 on 7th November 2015";
    "Loss v SønderjyskE 1 - 2 on 21st November 2015";
    "Loss v Randers FC 1 - 0 on 28th November 2015";
    "Loss v Brøndby IF 0 - 2 on 6th December 2015";
    "Draw v Viborg FF 1 - 1 on 28th February 2016"|]
```

[1]: https://vimeo.com/131640714
[2]: http://www.bbc.co.uk/sport/football/tables
## Build Status

Mono | .NET
---- | ----

## Maintainer(s)

- [@chrisnorris](https://github.com/chrisnorris)

