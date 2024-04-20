using UnityEngine;

namespace PixelInternalAPI.Classes
{
	public class StaminaModifier(float staminaMaxMod = 1f, float staminaRiseMod = 1f, float staminaDropMod = 1f)
	{
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaMaxMod = staminaMaxMod;
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaRiseMod = staminaRiseMod;
		[SerializeField]
		[Range(0f, float.MaxValue)]
		public float StaminaDropMod = staminaDropMod;

		public override string ToString() =>
			$"Max: {StaminaMaxMod}; Rise: {StaminaRiseMod}; Drop: {StaminaDropMod}";
		public override int GetHashCode() =>
			StaminaMaxMod.GetHashCode() + StaminaRiseMod.GetHashCode() + StaminaDropMod.GetHashCode();

	}
}
