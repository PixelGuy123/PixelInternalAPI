using HarmonyLib;
using System.Collections.Generic;

using UnityEngine;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch]
	internal class DijkstraMapPatches
	{
		[HarmonyPatch(typeof(DijkstraMap), "UpdateIsNeeded")]
		[HarmonyPatch(typeof(DijkstraMap), "Calculate", [])]
		[HarmonyPrefix]
		private static void CheckForNullTargets(ref List<Transform> ___targets)
		{
			for (int i = 0; i < ___targets.Count; i++)
			{
				if (___targets[i] == null)
					___targets.RemoveAt(i--);
			}
		}
	}
}
