using HarmonyLib;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(SodaMachine))]
	internal class SodaMachinePatch
	{
		[HarmonyPatch("ItemFits")]
		[HarmonyPrefix]
		private static bool OverrideUse(SodaMachine __instance, ref bool __result, Items checkItem, int ___usesLeft)
		{
			__result = ___usesLeft > 0 && __instance.GetComponent<SodaMachineCustomComponent>().requiredItems.Contains(checkItem);
			return false;
		}
		[HarmonyPatch("InsertItem")]
		[HarmonyPrefix]
		private static void OverrideUse(SodaMachine __instance, ref int ___usesLeft)
		{
			if (__instance.GetComponent<SodaMachineCustomComponent>().infiniteUses)
				___usesLeft = 2; // always set to 2 lol
		}
	}
}
