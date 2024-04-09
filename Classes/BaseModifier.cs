using UnityEngine;

namespace PixelInternalAPI.Classes
{
	public class BaseModifier(float modAdder = 0f) // Feels like just a float held by reference
	{
		[SerializeField]
		public float Mod = modAdder;
		public override string ToString() =>
			$"{Mod}";
		public override int GetHashCode() =>
			Mod.GetHashCode();
	}
}
