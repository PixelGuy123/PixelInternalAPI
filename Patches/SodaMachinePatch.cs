using HarmonyLib;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(SodaMachine))]
	internal class SodaMachinePatch
	{
		[HarmonyPatch("ItemFits")]
		[HarmonyPrefix]
		private static bool OverrideUse(SodaMachine __instance, ref bool __result, Items checkItem, ref int ___usesLeft)
		{
			var comp = __instance.GetComponent<SodaMachineCustomComponent>();
			__result = (___usesLeft > 0 || comp.infiniteUses) && comp.requiredItems.Contains(checkItem);
			if (comp.infiniteUses)
				___usesLeft = 10;
			return false;
		}
	}
}
