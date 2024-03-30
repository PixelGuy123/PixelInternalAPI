using PixelInternalAPI.Classes;
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

		private void Update()
		{
			float fov = defaultFov;

			if (fovModifiers.Count != 0) // Can't be -1 lol
				for (int i = 0; i < fovModifiers.Count; i++)
					fov += fovModifiers[i].Mod;


			fov = Mathf.Clamp(fov, 0f, 125f);

			cam.camCom.fieldOfView = fov;
		}

		private GameCamera cam;

		private float defaultFov;

		public List<BaseModifier> fovModifiers = [];
	}
}
