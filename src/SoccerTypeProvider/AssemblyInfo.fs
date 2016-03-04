namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("SoccerTypeProvider")>]
[<assembly: AssemblyProductAttribute("SoccerTypeProvider")>]
[<assembly: AssemblyDescriptionAttribute("type provider for football leagues")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
