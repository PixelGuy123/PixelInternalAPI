using HarmonyLib;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// Extensions to call private methods more easily.
	/// </summary>
	[HarmonyPatch]
	public static class PrivateCallExtensions
	{
		/*
		/// <summary>
		/// Calls the <see cref="NPC.SetGuilt(float, string)"/> method.
		/// </summary>
		/// <param name="instance">The <see cref="NPC"/> instance.</param>
		/// <param name="time">The time that rule break lasts to the npc for Principal to notice.</param>
		/// <param name="rule">The rule broken.</param>
		[HarmonyReversePatch]
		[HarmonyPatch(typeof(NPC), "SetGuilt")]
		public static void SetGuilt(object instance, float time, string rule) =>
			throw new System.NotImplementedException("Stub");
		*/
		/// <summary>
		/// Calls the <see cref="NPC.SetGuilt(float, string)"/> method.
		/// </summary>
		/// <param name="npc">The <see cref="NPC"/> instance.</param>
		/// <param name="time">The time that rule break lasts to the npc for Principal to notice.</param>
		/// <param name="rule">The rule broken.</param>
		public static void SetGuilt(this NPC npc, float time, string rule) =>
			npc.SetGuilt(time, rule);
	}
}
