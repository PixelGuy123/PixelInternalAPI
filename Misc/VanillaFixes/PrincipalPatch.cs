using HarmonyLib;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(Principal), "SendToDetention")]
	internal class PrincipalPatch
	{
		static void Postfix(Navigator ___navigator, EnvironmentController ___ec, PlayerManager ___targetedPlayer)
		{
			if (___ec.offices.Count > 0)
				___navigator.Entity.Teleport(___targetedPlayer.transform.position + ___targetedPlayer.transform.forward * 10f); // Use .Teleport() instead of directly setting the position
		}
	}
}
