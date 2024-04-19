using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class TemporaryParticles : MonoBehaviour
	{
		private void Start()
		{
			if (audExplode != null)
				audMan?.PlaySingle(audExplode);

			particles.Emit(Random.Range(minParticles, maxParticles));
		}

		void Update()
		{
			if (!ec) return;

			cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
			if (cooldown <= 0f)
				Destroy(gameObject);
		}

		public EnvironmentController ec;

		[SerializeField]
		public ParticleSystem particles;

		[SerializeField]
		public SoundObject audExplode = null;

		[SerializeField]
		public AudioManager audMan = null;

		[SerializeField]
		public int minParticles, maxParticles;

		[SerializeField]
		public float cooldown = 10f;
	}
}
