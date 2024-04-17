using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class CustomPlayerCameraComponent : MonoBehaviour
	{
		private void Awake()
		{
			cam = GetComponent<GameCamera>();

			if (cam == null)
			{
				Debug.LogError("PixelInternalAPI: Failed to apply CustomPlayerCameraComponent into the GameCamera, the component doesn't exist");
				DestroyImmediate(gameObject);
				return;
			}

			defaultFov = cam.camCom.fieldOfView;
		}

		void UpdateFov()
		{
			float fov = defaultFov;

			for (int i = 0; i < fovModifiers.Count; i++)
					fov += fovModifiers[i].Mod;

			cam.camCom.fieldOfView = Mathf.Clamp(fov, 0f, 125f);
		}

		public void AddModifier(BaseModifier mod)
		{
			fovModifiers.Add(mod);
			UpdateFov();
		}

		public void RemoveModifier(BaseModifier mod)
		{
			fovModifiers.Remove(mod);
			UpdateFov();
		}

		internal void ResetFov()
		{
			fovModifiers.Clear();
			UpdateFov();
		}

		// ******** IEnumerators *********
		// Note: Any fov animation REQUIRES to be in this IEnumerator to update the fov properly

		public IEnumerator ReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			instance.Mod += offset;
			bool isPositive = instance.Mod >= 0f;
			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += -instance.Mod / smoothness * referenceFrameRate * Time.deltaTime;
				if (isPositive)
				{
					if (instance.Mod < 0f)
						yield break;
				}
				else if (instance.Mod > 0f)
					yield break;

				UpdateFov();
				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		public IEnumerator SlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			float off = Mathf.Clamp(instance.Mod + offset, 0f, 125f);
			bool isPositive = off <= instance.Mod;

			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.Mod.CompareFloats(off))
			{
				instance.Mod += (off - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;

				if (isPositive)
				{
					if (instance.Mod < off)
						yield break;
				}
				else if (instance.Mod > off)
					yield break;

				UpdateFov();
				yield return null;
			}

			yield break;
		}

		public IEnumerator ResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			bool isPositive = instance.Mod >= 0f;

			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += -instance.Mod / smoothness * referenceFrameRate * Time.deltaTime;

				if (isPositive)
				{
					if (instance.Mod < 0f)
						yield break;
				}
				else if (instance.Mod > 0f)
					yield break;

				UpdateFov();
				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		private GameCamera cam;

		private float defaultFov;

		readonly List<BaseModifier> fovModifiers = [];
	}
}
