using HarmonyLib;
using UnityEngine;
using System.Collections;

namespace PixelInternalAPI.Misc.VanillaFixes
{
	[HarmonyPatch(typeof(RandomEvent))]
	internal class RandomEventPatch
	{
		[HarmonyPatch("End")]
		[HarmonyPostfix]
		static void FixEndPhase(RandomEvent __instance, EnvironmentController ___ec)
		{
			if (__instance.GetType() == typeof(FloodEvent))
			{
				foreach (var r in ___ec.rooms)
				{
					foreach (var door in r.doors)
					{
						door.Open(true, false);
						door.OpenTimed(Random.Range(3f, 5f), false);
					}
				}
			}
		}

		[HarmonyPostfix]
		[HarmonyPatch("End")]
		static void EventTimerStop(RandomEvent __instance, ref IEnumerator ___eventTime) // Basically, if the event is forced to end earlier, the timer is stopped
		{
			if (___eventTime != null)
				__instance.StopCoroutine(___eventTime);
		}
	}
}
