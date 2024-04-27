using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// A component that adds a simple timer to despawn the particles
	/// <para>Used for explosions.</para>
	/// </summary>
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
		/// <summary>
		/// The <see cref="EnvironmentController"/> field (must be set to work).
		/// </summary>
		public EnvironmentController ec;

		/// <summary>
		/// The particles it should use for the explosion.
		/// </summary>
		[SerializeField]
		public ParticleSystem particles;

		/// <summary>
		/// The explosion audio (optional).
		/// </summary>
		[SerializeField]
		public SoundObject audExplode = null;

		/// <summary>
		/// The <see cref="AudioManager"/> (optional too)
		/// </summary>
		[SerializeField]
		public AudioManager audMan = null;

		/// <summary>
		/// The min and max amount of particles for the explosion.
		/// </summary>
		[SerializeField]
		public int minParticles, maxParticles;

		/// <summary>
		/// How long does the particle object last before despawning.
		/// </summary>
		[SerializeField]
		public float cooldown = 10f;
	}
}
