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

		public IEnumerator ReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			instance.Mod += offset;
			if (!fovModifiers.Contains(instance))
				fovModifiers.Add(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += (1f - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			fovModifiers.Remove(instance);

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

			if (!fovModifiers.Contains(instance))
				fovModifiers.Add(instance);

			while (!instance.Mod.CompareFloats(off))
			{
				instance.Mod += (off - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			fovModifiers.Remove(instance);

			yield break;
		}

		public IEnumerator ResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			if (!fovModifiers.Contains(instance))
				fovModifiers.Add(instance);

			while (!instance.Mod.CompareFloats(0f))
			{
				instance.Mod += -instance.Mod / smoothness * referenceFrameRate * Time.deltaTime;
				yield return null;
			}

			fovModifiers.Remove(instance);

			yield break;
		}

		private GameCamera cam;

		private float defaultFov;

		readonly List<BaseModifier> fovModifiers = [];
	}
}
