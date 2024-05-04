using HarmonyLib;
using PixelInternalAPI.Components;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(LevelBuilder), "InstatiateEnvironmentObject", [typeof(GameObject), typeof(Cell), typeof(Direction)])]
	internal class LevelBuilderPatch
	{
		private static void Postfix(GameObject __result, Cell cell, Direction dir) =>
			__result.GetComponents<ModdedEnvironmentObject>().Do(x =>
			{
				x.definedCell = cell;
				x.definedDir = dir;
			});
		
	}
}
