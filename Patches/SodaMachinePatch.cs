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
			if (!comp) // If the soda machine doesn't have this component, it's not intended to use the api
				return true;

			__result = ___usesLeft > 0 && comp.requiredItems.Contains(checkItem);

			return false;
		}
	}
}
