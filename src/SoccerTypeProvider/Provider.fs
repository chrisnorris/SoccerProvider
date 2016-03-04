module Provider

open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open System.Reflection
open System
open FootballTypeProvider.DataService
open FootballTypeProvider.TypeFactory

[<TypeProvider>]
type FootballTypeProvider(config : TypeProviderConfig) as this = 
    inherit TypeProviderForNamespaces()
    let namespaceName = "FootballTeams.TypeProvider"
    let providerName = "FootballTypeProvider"
    let asm = Assembly.GetExecutingAssembly()
    let baseTy = typeof<obj>
    
    let createTypes() = 
        let footballType = 
            ProvidedTypeDefinition(asm, namespaceName, providerName, Some typeof<obj>, HideObjectMethods = true)
        let parameters = [ ProvidedStaticParameter("Group", typeof<string>, String.Empty) ]
        do footballType.DefineStaticParameters(parameters, 
                                               (fun typeName parameterValues -> 
                                               match parameterValues with
                                               | [| :? string as groupName |] -> 
                                                   let myType = 
                                                       ProvidedTypeDefinition
                                                           (asm, namespaceName, typeName, baseType = Some baseTy)
                                                   myType.AddMembersDelayed(buildLeagues groupName)
                                                   myType
                                               | _ -> failwith "unexpected parameter values"))
        let innerState = 
            let groupNames = retrieveGroupNames()
            ProvidedProperty
                ("AvailableGroups", typeof<string>, IsStatic = true, 
                 GetterCode = fun _ -> <@@ ((groupNames) :> obj) :?> string @@>)
        footballType.AddMember(innerState)
        [ footballType ]
    
    do this.AddNamespace(namespaceName, createTypes())

[<assembly:TypeProviderAssembly>]
do ()
