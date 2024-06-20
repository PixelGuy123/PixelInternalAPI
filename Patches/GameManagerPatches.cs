using HarmonyLib;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using UnityEngine;

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
				___cameras[i].GetCustomCam().ResetFov();
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
		[HarmonyPatch(typeof(CoreGameManager), "RestoreMap")]
		[HarmonyFinalizer]
		private static System.Exception ShutExceptionUp(System.Exception __exception)
		{
			if (__exception != null)
			{
				Debug.LogWarning("There has been an exception thrown but has been shut as a fix!");
				Debug.LogWarning("----------------------------------");
				Debug.LogException(__exception);
				Debug.LogWarning("----------------------------------");
			}
			return null;
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
