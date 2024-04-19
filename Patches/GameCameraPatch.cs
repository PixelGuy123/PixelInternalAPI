using HarmonyLib;
using PixelInternalAPI.Components;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	[HarmonyPatch(typeof(GameCamera))]
	internal class GameCameraPatch
	{
		[HarmonyPatch("Awake")]
		private static void Prefix(GameCamera __instance) =>
			__instance.gameObject.AddComponent<CustomPlayerCameraComponent>();


		[HarmonyPatch("LateUpdate")]
		private static void Postfix(Camera ___camCom, Camera ___billboardCam, Matrix4x4 ____billboardCullingMatrix) // Should fix the issue with bill board cam broken
		{
			___billboardCam.fieldOfView = ___camCom.fieldOfView;
			___billboardCam.cullingMatrix = ____billboardCullingMatrix;
		}
	}
}
