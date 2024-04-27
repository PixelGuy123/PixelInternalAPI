using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// An animator for textures
	/// </summary>
	public class TextureAnimator : MonoBehaviour
	{
		private void Update()
		{
			if (renderers.Length == 0 || (useEnvironmentTimeScale && !Singleton<BaseGameManager>.Instance)) return;

			frame += speed * (useEnvironmentTimeScale ? Singleton<BaseGameManager>.Instance.Ec.EnvironmentTimeScale : 1f) * Time.deltaTime;

			if (frame > texs.Length)
				frame = 0f;
			int idx = Mathf.FloorToInt(frame);
			for (int i = 0; i < renderers.Length; i++)
				renderers[i].materials[defaultIndex].mainTexture = texs[idx];
		}

		float frame = 0f;

		/// <summary>
		/// The default index of the materials[] array from the renderer
		/// </summary>
		[SerializeField]
		public int defaultIndex = 0;
		/// <summary>
		/// The renderers to have their textures replaced
		/// </summary>
		[SerializeField]
		public Renderer[] renderers = [];
		/// <summary>
		/// The animated textures
		/// </summary>
		[SerializeField]
		public Texture[] texs = [];
		/// <summary>
		/// The speed of the animator.
		/// </summary>
		[SerializeField]
		public float speed = 1f;
		/// <summary>
		/// If it should use the environmentTimeScale
		/// </summary>
		[SerializeField]
		public bool useEnvironmentTimeScale = true;
	}
}
