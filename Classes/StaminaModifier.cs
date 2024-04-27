using UnityEngine;

namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// A modifier for the stamina of the player (by multiplying).
	/// </summary>
	/// <param name="staminaMaxMod"></param>
	/// <param name="staminaRiseMod"></param>
	/// <param name="staminaDropMod"></param>
	public class StaminaModifier(float staminaMaxMod = 1f, float staminaRiseMod = 1f, float staminaDropMod = 1f)
	{
		/// <summary>
		/// Modifies the max of the stamina.
		/// </summary>
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaMaxMod = staminaMaxMod;
		/// <summary>
		/// Modifies the rising value of the stamina.
		/// </summary>
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaRiseMod = staminaRiseMod;
		/// <summary>
		/// Modifies the drop value of the stamina.
		/// </summary>
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaDropMod = staminaDropMod;

		/// <summary>
		/// Represents the <see cref="StaminaModifier"/> in a string.
		/// </summary>
		/// <returns>A string representing the <see cref="StaminaModifier"/> values</returns>
		public override string ToString() =>
			$"Max: {StaminaMaxMod}; Rise: {StaminaRiseMod}; Drop: {StaminaDropMod}";
		/// <summary>
		/// Returns a Hashcode from all the values of <see cref="StaminaModifier"/>
		/// </summary>
		/// <returns>A Hashcode from all the values of <see cref="StaminaModifier"/></returns>
		public override int GetHashCode() =>
			StaminaMaxMod.GetHashCode() + StaminaRiseMod.GetHashCode() + StaminaDropMod.GetHashCode();

	}
}
