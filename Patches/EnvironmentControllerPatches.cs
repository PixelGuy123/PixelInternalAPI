using HarmonyLib;
using PixelInternalAPI.Components;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(EnvironmentController))]
	internal class EnvironmentControllerPatches
	{
		[HarmonyPatch("SpawnNPC")]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> AddAttributes(IEnumerable<CodeInstruction> i) =>
			new CodeMatcher(i)
			.MatchForward(true, 
				new(OpCodes.Ldarg_1),
				new(OpCodes.Ldarg_0),
				new(OpCodes.Call, AccessTools.PropertyGetter(typeof(Component), "transform")),
				new(OpCodes.Call, name: "NPC Instantiate[NPC](NPC, UnityEngine.Transform)"),
				new(OpCodes.Stloc_0)
				)
			.Advance(1)
			.InsertAndAdvance(
				new(OpCodes.Ldloc_0), // Adds custom components through here (when npcs are instantiated)
				Transpilers.EmitDelegate<Action<NPC>>(x => x.gameObject.AddComponent<NPCAttributesContainer>())
			)

			.InstructionEnumeration();
	}
}
