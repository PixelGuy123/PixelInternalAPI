using HarmonyLib;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(PlayerMovement))]
	internal class PlayerMovementPatch
	{
		[HarmonyPatch("AddStamina", [typeof(float), typeof(bool)])]
		[HarmonyPostfix]
		static void FixStaminaBelowZero(ref float ___stamina)
		{
			if (___stamina < 0f)
				___stamina = 0f; // Why isn't this implemented in base BB+ >:(
		}
	}
}
