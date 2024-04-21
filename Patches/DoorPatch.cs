using HarmonyLib;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(Door))]
	internal class DoorPatch
	{
		[HarmonyPatch("Open")]
		private static void Prefix(bool ___locked, bool cancelTimer, ref float ___shutTime)
		{
			if (!___locked && cancelTimer)
				___shutTime = 0f; // Why isn't this resetted at all??
		}
	}
}
