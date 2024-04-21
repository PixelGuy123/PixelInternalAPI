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

		void Update()
		{
			if (fovModifiers.Count == 0)
			{
				cam.camCom.fieldOfView = defaultFov;
				return;
			}

			float fov = defaultFov;

			for (int i = 0; i < fovModifiers.Count; i++)
					fov += fovModifiers[i].Mod;

			cam.camCom.fieldOfView = Mathf.Clamp(fov, minFov, maxFov);
		}

		public void AddModifier(BaseModifier mod) =>
			fovModifiers.Add(mod);
		

		public void RemoveModifier(BaseModifier mod) =>
			fovModifiers.Remove(mod);
		

		internal void ResetFov() =>
			fovModifiers.Clear();


		// ******** IEnumerators *********

		public Coroutine ReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier =>
			StartCoroutine(InternalReverseSlideFOVAnimation(instance, offset, smoothness, referenceFrameRate));

		IEnumerator InternalReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
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

				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		public Coroutine SlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier =>
			StartCoroutine(InternalSlideFOVAnimation(instance, offset, smoothness, referenceFrameRate));

		IEnumerator InternalSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			float off = Mathf.Clamp(instance.Mod + offset, -minFov, maxFov);
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


				yield return null;
			}

			instance.Mod = off;

			yield break;
		}

		public Coroutine ResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier =>
			StartCoroutine(InternalResetSlideFOVAnimation(instance, smoothness, referenceFrameRate));
		IEnumerator InternalResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : BaseModifier
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


				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		public Coroutine ShakeFOVAnimation<T>(T instance, float smoothness = 1f, float intensity = 1f, float shakeCooldown = 1f, int shakePerFrames = 2, float referenceFrameRate = 30f) where T : BaseModifier =>
			StartCoroutine(InternalShakeFOVAnimation(instance, smoothness, intensity, shakeCooldown, shakePerFrames, referenceFrameRate));
		IEnumerator InternalShakeFOVAnimation<T>(T instance, float smoothness = 1f, float intensity = 1f, float shakeCooldown = 1f, int shakePerFrames = 2, float referenceFrameRate = 30f) where T : BaseModifier
		{
			if (smoothness < 1f)
			{
				Debug.LogWarning("Smoothness is less than 1f");
				yield break;
			}
			if (intensity < 0f)
			{
				Debug.LogWarning("intensity is less than 1f");
				yield break;
			}

			if (shakeCooldown < 0f)
			{
				Debug.LogWarning("shake cooldown is less than 1f");
				yield break;
			}

			float offset = Random.Range(-intensity, intensity);

			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			int frameMax = shakePerFrames;

			while (shakeCooldown > 0f)
			{
				instance.Mod += (offset - instance.Mod) / smoothness * referenceFrameRate * Time.deltaTime;
				shakeCooldown -= Time.deltaTime;
				if (--shakePerFrames < 0f)
				{
					shakePerFrames = frameMax;
					offset = Random.Range(-intensity, intensity);
				}


				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		private GameCamera cam;

		private float defaultFov;

		readonly List<BaseModifier> fovModifiers = [];

		const float minFov = 20f, maxFov = 125f;
	}
}
