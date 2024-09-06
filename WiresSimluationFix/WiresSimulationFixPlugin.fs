namespace WiresSimluationFix

open BepInEx
open HarmonyLib
open BepInEx.Logging
open Utils.Operators

module WiresSimluationFix =
    let mutable internal Plugin: BaseUnityPlugin option = None
    let mutable internal Logger: ManualLogSource option = None
    let mutable internal Patcher: Harmony option = None
    
open WiresSimluationFix

[<BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)>]
type WiresSimluationFixPlugin() =
    inherit BaseUnityPlugin()
    
    member this.Awake() =
        Plugin <- Some this
        Logger <- Some this.Logger
        Patcher <- Harmony(this.Info.Metadata.GUID) |> Some
        
        Patcher |??> (_.PatchAll()) |>- ()

        let methods = Patcher |??> (_.GetPatchedMethods()) |??> List.ofSeq
        if methods.IsSome && Logger.IsSome then
            $"Patched {methods.Value.Length} method{(if methods.Value.Length = 1 then ' ' else 's')}" |> Logger.Value.LogInfo
        
[<HarmonyPatch(typeof<SimulationGraph>)>]
module SimulationGraphPatcher =
    [<HarmonyPatch("<Update>g__UpdateCluster|29_1")>] [<HarmonyPrefix>]
    let PrepareStartOptionsOverride (requiredDeltaTicks: int byref, __instance: SimulationGraph) =
        requiredDeltaTicks <- 0
        ()
        