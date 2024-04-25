using HarmonyLib;
using PixelInternalAPI.Classes;
using UnityEngine;

namespace PixelInternalAPI.Patches
{
	
	[HarmonyPatch(typeof(AudioListener))]
	internal class AudioListenerPatch
	{
		[HarmonyPatch("pause", MethodType.Setter)]
		private static void Prefix(ref bool value) // Very crazy patch, hopefully it doesn't fk anything up
		{
			GlobalAudioListenerModifier.PauseListener(value);
			value = GlobalAudioListenerModifier.ListenerIsPaused;
		}

		[HarmonyPatch("pause", MethodType.Getter)]
		private static void Postfix(ref bool __result) => // Very crazy patch
			__result = GlobalAudioListenerModifier.ListenerIsPaused;
		
	}
	
}
