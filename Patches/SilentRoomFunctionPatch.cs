using HarmonyLib;
using PixelInternalAPI.Classes;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(SilenceRoomFunction))] // Makes this function use the API listener modifier instead
	internal class SilentRoomFunctionPatch
	{
		[HarmonyPatch("OnPlayerEnter")]
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> OnPlayerEnterPatch(IEnumerable<CodeInstruction> i) =>
			new CodeMatcher(i)
			.MatchForward(true, 
				new(OpCodes.Ldc_R4, 0.0f),
				new(OpCodes.Call, AccessTools.PropertySetter(typeof(AudioListener), "volume"))
				)
			.SetInstruction(CodeInstruction.Call(typeof(GlobalAudioListenerModifier), "AddVolumeModifier", [typeof(float)]))
			.InstructionEnumeration();
		[HarmonyPatch("OnPlayerExit")]
		[HarmonyPatch("OnDestroy")]
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> OnPlayerExitPatch(IEnumerable<CodeInstruction> i) =>
			new CodeMatcher(i)
			.MatchForward(false,
				new(OpCodes.Ldc_R4, 1f),
				new(OpCodes.Call, AccessTools.PropertySetter(typeof(AudioListener), "volume"))
				)
			.SetInstructionAndAdvance(new(OpCodes.Ldc_R4, 0f))
			.SetInstructionAndAdvance(CodeInstruction.Call(typeof(GlobalAudioListenerModifier), "RemoveVolumeModifier", [typeof(float)]))
			.InstructionEnumeration();
	}
}
