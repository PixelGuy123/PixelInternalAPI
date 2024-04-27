using HarmonyLib;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// Extensions to call private methods more easily.
	/// </summary>
	[HarmonyPatch]
	public class PrivateCallExtensions
	{
		/// <summary>
		/// Calls the <see cref="NPC.SetGuilt(float, string)"/> method.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="time"></param>
		/// <param name="rule"></param>
		[HarmonyReversePatch]
		[HarmonyPatch(typeof(NPC), "SetGuilt")]
		public static void SetGuilt(object instance, float time, string rule) =>
			throw new System.NotImplementedException("Stub");
	}
}
