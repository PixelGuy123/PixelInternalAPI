using HarmonyLib;

namespace PixelInternalAPI.Extensions
{
	[HarmonyPatch]
	public class PrivateCallExtensions
	{
		[HarmonyReversePatch]
		[HarmonyPatch(typeof(NPC), "SetGuilt")]
		public static void SetGuilt(object instance, float time, string rule) =>
			throw new System.NotImplementedException("Stub");
	}
}
