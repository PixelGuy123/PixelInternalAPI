using HarmonyLib;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(CoreGameManager))]
	internal class CoreGameManagerPatches
	{
		[HarmonyPatch("SpawnPlayers")]
		[HarmonyPostfix] // Fix for the camera fovs
		private static void ResetCameraFovs(CoreGameManager __instance, ref GameCamera[] ___cameras, ref PlayerManager[] ___players)
		{
			for (int i = 0; i < __instance.setPlayers; i++)
			{
				___cameras[i].GetComponent<CustomPlayerCameraComponent>()?.ResetFov();
				___players[i].gameObject.AddComponent<PlayerAttributesComponent>();
			}
			GlobalAudioListenerModifier.Reset();
		}
	}
}
