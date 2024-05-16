using HarmonyLib;
using UnityEngine;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(ITM_YTPs), "Use")]
	internal class YTPFix
	{
		static void Postfix(ITM_YTPs __instance) =>
			Object.Destroy(__instance.gameObject);
	}
}
