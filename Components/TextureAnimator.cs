using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class TextureAnimator : MonoBehaviour
	{
		private void Update()
		{
			if (renderers.Length == 0 || !Singleton<BaseGameManager>.Instance) return;

			frame += speed * Singleton<BaseGameManager>.Instance.Ec.EnvironmentTimeScale * Time.deltaTime;
			if (frame > texs.Length)
				frame = 0f;
			int idx = Mathf.FloorToInt(frame);
			for (int i = 0; i < renderers.Length; i++)
				renderers[i].materials[defaultIndex].mainTexture = texs[idx];
		}

		float frame = 0f;

		[SerializeField]
		public int defaultIndex = 0;

		[SerializeField]
		public Renderer[] renderers = [];

		[SerializeField]
		public Texture[] texs = [];

		[SerializeField]
		public float speed = 1f;
	}
}
