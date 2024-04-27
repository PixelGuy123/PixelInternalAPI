using UnityEngine;

namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// A generic class to hold a float by reference.
	/// </summary>
	/// <param name="modAdder"></param>
	public class BaseModifier(float modAdder = 0f) // Feels like just a float held by reference
	{
		/// <summary>
		/// The value of the <see cref="BaseModifier"/>
		/// </summary>
		[SerializeField]
		public float Mod = modAdder;
		/// <summary>
		/// Returns a string representing the <see cref="BaseModifier"/> value.
		/// </summary>
		/// <returns>a string representing the <see cref="BaseModifier"/> value.</returns>
		public override string ToString() =>
			$"{Mod}";
		/// <summary>
		/// Returns a hashcode from the <see cref="BaseModifier"/>
		/// </summary>
		/// <returns>A hashcode from the <see cref="BaseModifier"/>.</returns>
		public override int GetHashCode() =>
			Mod.GetHashCode();
	}
}
