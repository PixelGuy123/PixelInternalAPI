using HarmonyLib;
using UnityEngine;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(ITM_BSODA), "EntityTriggerEnter")]
	internal static class ITMBsodaPatch
	{
		static bool Prefix(Collider other) =>
			!other.GetComponentInChildren<ITM_BSODA>(); // Should only touch if it doesn't contain a bsoda component (itself basically) to avoid infinite speed and potential lag
	}
}
