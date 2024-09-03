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
        
[<HarmonyPatch(typeof<SimulationGraph>)>]
module SimulationGraphPatcher =
    [<HarmonyPatch(".ctor")>] [<HarmonyPostfix>]
    let PrepareStartOptionsOverride (__instance: SimulationGraph) =
        __instance.MaxDeltaTicks |> Logger.Value.LogInfo
        Traverse.Create(__instance).Field<int>("MaxDeltaTicks").Value <- 1
        
        ()
        