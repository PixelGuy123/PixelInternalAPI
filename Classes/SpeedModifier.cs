using UnityEngine;

namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// A modifier for the speed of the player.
	/// </summary>
	/// <param name="walkSpeed"></param>
	/// <param name="runSpeed"></param>
	/// <param name="isAnAdder"></param>
	public class SpeedModifier(float walkSpeed, float runSpeed, bool isAnAdder = false)
	{
		/// <summary>
		/// Modifies the walk speed of the player.
		/// </summary>
		[SerializeField]
		public float WalkSpeed = walkSpeed;
		/// <summary>
		/// Modifies the run speed of the player.
		/// </summary>
		[SerializeField]
		public float RunSpeed = runSpeed;
		/// <summary>
		/// If the modifier should add or multiply the speed.
		/// </summary>
		[SerializeField]
		public bool Adder = isAnAdder;

		/// <summary>
		/// Represents a <see cref="SpeedModifier"/> in a string.
		/// </summary>
		/// <returns>A string representing  the <see cref="SpeedModifier"/> values.</returns>
		public override string ToString() =>
			$"{WalkSpeed};{RunSpeed} / Is an adder: {Adder}";
		/// <summary>
		/// Returns a HashCode from all the values from the <see cref="SpeedModifier"/>.
		/// </summary>
		/// <returns>A HashCode from all the values from the <see cref="SpeedModifier"/>.</returns>
		public override int GetHashCode() =>
			WalkSpeed.GetHashCode() + RunSpeed.GetHashCode() + Adder.GetHashCode();
	}
}
