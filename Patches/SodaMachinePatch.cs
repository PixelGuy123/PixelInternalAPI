using HarmonyLib;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(SodaMachine), "ItemFits")]
	internal class SodaMachinePatch
	{
		[HarmonyPrefix]
		private static bool OverrideUse(SodaMachine __instance, ref bool __result, Items item, int ___usesLeft)
		{
			__result = ___usesLeft > 0 && __instance.GetComponent<SodaMachineCustomComponent>().requiredItems.Contains(item);
			return false;
		}
	}
}
