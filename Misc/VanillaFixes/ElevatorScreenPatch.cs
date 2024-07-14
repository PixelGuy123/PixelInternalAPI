using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(ElevatorScreen), "ZoomIntro", MethodType.Enumerator)]
	internal class ElevatorScreenPatch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> i) =>
			new CodeMatcher(i)
			.MatchForward(true,
				new(OpCodes.Ldloc_1),
				new(CodeInstruction.LoadField(typeof(ElevatorScreen), "cursorInitiator")),
				new(OpCodes.Ldc_I4_1),
				new(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Behaviour), "enabled"))
				)
			.Advance(1)
			.InsertAndAdvance(Transpilers.EmitDelegate(() =>
			{
				Singleton<MusicManager>.Instance.StopMidi();
				Singleton<MusicManager>.Instance.StopFile();
			}))
			.InstructionEnumeration();
	}
}
