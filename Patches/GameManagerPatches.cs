using HarmonyLib;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch]
	internal class GameManagerPatches
	{
		[HarmonyPatch(typeof(CoreGameManager), "SpawnPlayers")]
		[HarmonyPostfix] // Fix for the camera fovs
		private static void ResetCameraFovs(CoreGameManager __instance, ref GameCamera[] ___cameras, ref PlayerManager[] ___players)
		{
			for (int i = 0; i < __instance.setPlayers; i++)
			{
				___cameras[i].GetComponent<CustomPlayerCameraComponent>()?.ResetFov();
				___players[i].gameObject.AddComponent<PlayerAttributesComponent>();
			}
		}

		[HarmonyPatch(typeof(BaseGameManager), "BeginPlay")]
		[HarmonyPostfix] // Fix for the camera fovs
		private static void FixAudioListener() =>
			GlobalAudioListenerModifier.Reset();
	}
}
