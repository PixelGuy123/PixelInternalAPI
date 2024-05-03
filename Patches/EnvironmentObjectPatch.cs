using HarmonyLib;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(EnvironmentObject), "GiveController")]
	internal class EnvironmentObjectPatch
	{
		private static void Prefix(Transform currentObject) =>
			currentObject.transform.localPosition = Vector3.zero; // Should fix any EnvironmentObject created by mods spawning incorrectly
	}
}
