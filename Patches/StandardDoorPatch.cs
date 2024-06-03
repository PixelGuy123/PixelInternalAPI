using HarmonyLib;
namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(StandardDoor), "ItemFits")]
	internal class StandardDoorPatch
	{
		static bool Prefix() => false;
		static void Postfix(ref bool __result, Items item, bool ___locked) =>
			__result = ___locked && ResourceManager._unlockableItems.Contains(item);
	}
}
