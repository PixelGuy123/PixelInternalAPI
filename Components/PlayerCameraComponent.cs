﻿using MTM101BaldAPI.Components;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// A player component that manages the Camera's FOV.
	/// </summary>
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
					fov += fovModifiers[i].addend;

			cam.camCom.fieldOfView = Mathf.Clamp(fov, minFov, maxFov);
		}
		/// <summary>
		/// Add a fov modifier.
		/// </summary>
		/// <param name="mod"></param>
		public void AddModifier(ValueModifier mod) =>
			fovModifiers.Add(mod);
		
		/// <summary>
		/// Remove a fov modifier.
		/// </summary>
		/// <param name="mod"></param>
		public void RemoveModifier(ValueModifier mod) =>
			fovModifiers.Remove(mod);
		

		internal void ResetFov() =>
			fovModifiers.Clear();


		// ******** IEnumerators *********

		/// <summary>
		/// Initiates a Reverse Slide animation (basically goes from <c><paramref name="instance"/>.addend + <paramref name="offset"/></c> to 0).
		/// <para><paramref name="smoothness"/> define the smoothness of the animation.</para>
		/// <para><paramref name="referenceFrameRate"/> is the frame rate used as reference (since <see cref="Time.deltaTime"/> is used).</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="offset"></param>
		/// <param name="smoothness"></param>
		/// <param name="referenceFrameRate"></param>
		/// <returns>A <see cref="Coroutine"/> of the animation</returns>
		public Coroutine ReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier =>
			StartCoroutine(InternalReverseSlideFOVAnimation(instance, offset, smoothness, referenceFrameRate));

		IEnumerator InternalReverseSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			instance.addend += offset;
			bool isPositive = instance.addend >= 0f;
			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.addend.CompareFloats(0f))
			{
				instance.addend += -instance.addend / smoothness * referenceFrameRate * Time.deltaTime;
				if (isPositive)
				{
					if (instance.addend < 0f)
						yield break;
				}
				else if (instance.addend > 0f)
					yield break;

				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}
		/// <summary>
		/// A basic slide animation to a selected <paramref name="offset"/> from the <c><paramref name="instance"/>.addend</c>
		/// <para><paramref name="smoothness"/> define the smoothness of the animation.</para>
		/// <para><paramref name="referenceFrameRate"/> is the frame rate used as reference (since <see cref="Time.deltaTime"/> is used).</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="offset"></param>
		/// <param name="smoothness"></param>
		/// <param name="referenceFrameRate"></param>
		/// <returns>A <see cref="Coroutine"/> of the animation</returns>
		public Coroutine SlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier =>
			StartCoroutine(InternalSlideFOVAnimation(instance, offset, smoothness, referenceFrameRate));

		IEnumerator InternalSlideFOVAnimation<T>(T instance, float offset, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}
			float off = Mathf.Clamp(instance.addend + offset, -minFov, maxFov);
			bool isPositive = off <= instance.addend;

			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.addend.CompareFloats(off))
			{
				instance.addend += (off - instance.addend) / smoothness * referenceFrameRate * Time.deltaTime;

				if (isPositive)
				{
					if (instance.addend < off)
						yield break;
				}
				else if (instance.addend > off)
					yield break;


				yield return null;
			}

			instance.addend = off;

			yield break;
		}
		/// <summary>
		/// A slide animation that goes from <c><paramref name="instance"/>.addend</c> to 0.
		/// <para><paramref name="smoothness"/> define the smoothness of the animation.</para>
		/// <para><paramref name="referenceFrameRate"/> is the frame rate used as reference (since <see cref="Time.deltaTime"/> is used).</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="smoothness"></param>
		/// <param name="referenceFrameRate"></param>
		/// <returns><see cref="Coroutine"/></returns>
		public Coroutine ResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier =>
			StartCoroutine(InternalResetSlideFOVAnimation(instance, smoothness, referenceFrameRate));
		IEnumerator InternalResetSlideFOVAnimation<T>(T instance, float smoothness = 2f, float referenceFrameRate = 30f) where T : ValueModifier
		{
			if (smoothness <= 1f)
			{
				Debug.LogWarning("Smoothness is less than or equal to 1f");
				yield break;
			}

			bool isPositive = instance.addend >= 0f;

			if (!fovModifiers.Contains(instance))
				AddModifier(instance);

			while (!instance.addend.CompareFloats(0f))
			{
				instance.addend += -instance.addend / smoothness * referenceFrameRate * Time.deltaTime;

				if (isPositive)
				{
					if (instance.addend < 0f)
						yield break;
				}
				else if (instance.addend > 0f)
					yield break;


				yield return null;
			}

			RemoveModifier(instance);

			yield break;
		}

		/// <summary>
		/// Does a Shaking animation in the fov.
		/// <para><paramref name="intensity"/> is the how strong the shakeness is (cannot be below 0).</para>
		/// <para><paramref name="shakeCooldown"/> is how long the shaking goes.</para>
		/// <para><paramref name="shakePerFrames"/> is the change of FOV after x frames passed by (to give enough time for the smoothness to actually work)</para>
		/// <para><paramref name="smoothness"/> define the smoothness of the animation.</para>
		/// <para><paramref name="referenceFrameRate"/> is the frame rate used as reference (since <see cref="Time.deltaTime"/> is used).</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="smoothness"></param>
		/// <param name="intensity"></param>
		/// <param name="shakeCooldown"></param>
		/// <param name="shakePerFrames"></param>
		/// <param name="referenceFrameRate"></param>
		/// <returns><see cref="Coroutine"/></returns>
		public Coroutine ShakeFOVAnimation<T>(T instance, float smoothness = 1f, float intensity = 1f, float shakeCooldown = 1f, int shakePerFrames = 2, float referenceFrameRate = 30f) where T : ValueModifier =>
			StartCoroutine(InternalShakeFOVAnimation(instance, smoothness, intensity, shakeCooldown, shakePerFrames, referenceFrameRate));
		IEnumerator InternalShakeFOVAnimation<T>(T instance, float smoothness = 1f, float intensity = 1f, float shakeCooldown = 1f, int shakePerFrames = 2, float referenceFrameRate = 30f) where T : ValueModifier
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
				instance.addend += (offset - instance.addend) / smoothness * referenceFrameRate * Time.deltaTime;
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

		readonly List<ValueModifier> fovModifiers = [];

		const float minFov = 20f, maxFov = 125f;
	}
}
