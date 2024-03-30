using HarmonyLib;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(CoreGameManager))]
	internal class CoreGameManagerPatches
	{
		[HarmonyPatch("SpawnPlayers")]
		[HarmonyPostfix] // Fix for the camera fovs
		private static void ResetCameraFovs(CoreGameManager __instance, ref GameCamera[] ___cameras)
		{
			for (int i = 0; i < __instance.setPlayers; i++)
			{
				___cameras[i].GetComponent<CustomPlayerCameraComponent>()?.fovModifiers.Clear();
			}
		}
	}
}
