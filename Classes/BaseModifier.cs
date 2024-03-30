using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace PixelInternalAPI.Classes
{
	public class BaseModifier(float modAdder = 0f) // Feels like just a float held by reference
	{
		[UnityEngine.SerializeField]
		public float Mod = modAdder;
		public override string ToString() =>
			$"{Mod}";
		public override int GetHashCode() =>
			Mod.GetHashCode();

		public IEnumerator ReverseSlideFOVAnimation<T>(IList<T> collection, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			var instance = (T)this;

			instance.Mod += offset;
			if (!collection.Contains(instance))
				collection.Add(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += (1f - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			collection.Remove(instance);

			yield break;
		}

		public IEnumerator SlideFOVAnimation<T>(IList<T> collection, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			var instance = (T)this;
			float off = Mathf.Clamp(instance.Mod + offset, 0f, 125f);

			if (!collection.Contains(instance))
				collection.Add(instance);

			while (!instance.Mod.CompareFloats(off))
			{
				instance.Mod += (off - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			collection.Remove(instance);

			yield break;
		}

		public IEnumerator ResetSlideFOVAnimation<T>(IList<T> collection, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			var instance = (T)this;
			if (!collection.Contains(instance))
				collection.Add(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += -instance.Mod / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			collection.Remove(instance);

			yield break;
		}
	}
}
