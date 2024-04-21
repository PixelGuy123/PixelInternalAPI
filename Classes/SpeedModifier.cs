using UnityEngine;

namespace PixelInternalAPI.Classes
{
	public class SpeedModifier(float walkSpeed, float runSpeed, bool isAnAdder = false)
	{
		[SerializeField]
		public float WalkSpeed = walkSpeed;
		[SerializeField]
		public float RunSpeed = runSpeed;
		[SerializeField]
		public bool Adder = isAnAdder;

		public override string ToString() =>
			$"{WalkSpeed};{RunSpeed} / Is an adder: {Adder}";

		public override int GetHashCode() =>
			WalkSpeed.GetHashCode() + RunSpeed.GetHashCode() + Adder.GetHashCode();
	}
}
