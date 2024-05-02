using HarmonyLib;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(GameInitializer), "Initialize")]
	internal class GameInitializerPatch
	{
		[HarmonyPostfix]
		static void ActivateThem(GameInitializer __instance, SceneObject ___sceneObject) // This is where it should be
		{
			if (___sceneObject != null && ___sceneObject)
				ResourceManager.RaiseGameStart(__instance);
		}
	}
}
