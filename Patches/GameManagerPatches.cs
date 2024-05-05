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

		[HarmonyPatch(typeof(BaseGameManager), "Initialize")]
		[HarmonyPostfix]
		private static void PostGen(BaseGameManager __instance) =>
			ResourceManager.RaisePostGen(__instance);

		[HarmonyPatch(typeof(CoreGameManager), "PrepareForReload")]
		[HarmonyPostfix]
		private static void NextLevelCall()
		{
			ResourceManager.RaiseNextLevel(Singleton<BaseGameManager>.Instance, isNextLevel);
			isNextLevel = false;
		}
		[HarmonyPatch(typeof(BaseGameManager), "LoadNextLevel")]
		[HarmonyPrefix]
		private static void NextLevelBooleanSet() =>
			isNextLevel = true;
		[HarmonyPatch(typeof(GameInitializer), "WaitForGenerator")]
		[HarmonyPrefix]
		private static void StartGenerator(GameInitializer __instance, LevelBuilder lb, BaseGameManager gm, SceneObject ___sceneObject) =>
			ResourceManager.RaiseGenStart(__instance, lb, gm, ___sceneObject);

		static bool isNextLevel = false;
	}
}
