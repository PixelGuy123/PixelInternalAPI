using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace PixelInternalAPI.Extensions
{
	public static class ObjectCreationExtensions
	{
		static readonly FieldInfo _audio_minDistance = AccessTools.Field(typeof(PropagatedAudioManager), "minDistance");
		static readonly FieldInfo _audio_maxDistance = AccessTools.Field(typeof(PropagatedAudioManager), "maxDistance");
		static readonly FieldInfo _audio_loopOnStart = AccessTools.Field(typeof(AudioManager), "loopOnStart");
		static readonly FieldInfo _audio_soundObjects = AccessTools.Field(typeof(AudioManager), "soundOnStart");

		public static PropagatedAudioManager CreateAudioManager(this GameObject target, float minDistance = 25f, float maxDistance = 50f, bool isAPrefab = false)
		{
			var audio = target.AddComponent<PropagatedAudioManager>();
			_audio_minDistance.SetValue(audio, minDistance);
			_audio_maxDistance.SetValue(audio, maxDistance);
			if (!isAPrefab)
				return audio;

			if (audio.audioDevice != null)
				Object.Destroy(audio.audioDevice.gameObject);
			AudioManager.totalIds--;
			if (AudioManager.totalIds < 0)
				AudioManager.totalIds = 0;
			audio.sourceId = 0; // Copypaste from api lmao

			return audio;
		}

		public static PropagatedAudioManager CreateAudioManager(this GameObject target, bool loopOnStart, SoundObject[] startingAudios, float minDistance = 25f, float maxDistance = 50f, bool isAPrefab = false)
		{
			var audio = target.CreateAudioManager(minDistance, maxDistance, isAPrefab);
			_audio_loopOnStart.SetValue(audio, loopOnStart);
			_audio_soundObjects.SetValue(audio, startingAudios);

			return audio;
		}

		public static AudioManager CreateAudioManager(this GameObject target, AudioSource audioDevice, bool loopOnStart, SoundObject[] startingAudios, bool isAPrefab = false)
		{
			var audio = target.AddComponent<AudioManager>();
			audio.audioDevice = audioDevice;

			if (isAPrefab)
			{
				AudioManager.totalIds--;
				if (AudioManager.totalIds < 0) // SOMEHOW THIS IS POSSIBLE
					AudioManager.totalIds = 0;
				audio.sourceId = 0;
			}

			_audio_loopOnStart.SetValue(audio, loopOnStart);
			_audio_soundObjects.SetValue(audio, startingAudios);

			return audio;
		}

		public static AudioSource CreateAudioSource(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
		{
			var audio = target.AddComponent<AudioSource>();
			audio.minDistance = minDistance;
			audio.maxDistance = maxDistance;
			return audio;
		}

		public static void MakeAudioManagerNonPositional(this AudioManager man)
		{
			man.audioDevice.spatialBlend = 0f;
			man.positional = false;
			man.audioDevice.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		static readonly FieldInfo _entity_rendererBase = AccessTools.Field(typeof(Entity), "rendererBase");
		static readonly FieldInfo _entity_collider = AccessTools.Field(typeof(Entity), "collider");
		static readonly FieldInfo _entity_trigger = AccessTools.Field(typeof(Entity), "trigger");
		static readonly FieldInfo _entity_activity = AccessTools.Field(typeof(Entity), "externalActivity");
		static readonly FieldInfo _entity_etrigger = AccessTools.Field(typeof(Entity), "iEntityTrigger");
		static readonly FieldInfo _entity_CollisionMask = AccessTools.Field(typeof(Entity), "collisionLayerMask");

		public static Entity CreateEntity(this GameObject target, Collider collider, Collider triggerCollider = null, Transform rendererBase = null, IEntityTrigger[] triggers = null)
		{
			LayerMask mask = target.layer;
			var e = target.AddComponent<Entity>();
			e.SetActive(false);
			target.layer = mask;
			_entity_rendererBase.SetValue(e, rendererBase);
			_entity_collider.SetValue(e, collider);
			_entity_activity.SetValue(e, target.AddComponent<ActivityModifier>());
			_entity_trigger.SetValue(e, triggerCollider);
			if (triggers != null)
				_entity_etrigger.SetValue(e, triggers);
			

			return e;
		}

		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius = 0f, Transform rendererBase = null, IEntityTrigger[] triggers = null) =>
			CreateEntity(target, colliderRadius, triggerColliderRadius, out _, out _, rendererBase, triggers);
		

		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius, out CapsuleCollider nonTriggerCollider, out CapsuleCollider triggerCollider, Transform rendererBase = null, IEntityTrigger[] triggers = null)
		{
			var collider = target.AddComponent<CapsuleCollider>();
			collider.radius = colliderRadius;
			nonTriggerCollider = collider;


			CapsuleCollider trigger = null;
			triggerCollider = null;
			if (triggerColliderRadius > 0f)
			{
				trigger = target.AddComponent<CapsuleCollider>();
				trigger.radius = triggerColliderRadius;
				trigger.isTrigger = true;
				triggerCollider = trigger;
			}

			return CreateEntity(target, collider, trigger, rendererBase, triggers);
		}

		public static Entity SetEntityCollisionLayerMask(this Entity entity, LayerMask layer)
		{
			_entity_CollisionMask.SetValue(entity, layer);
			return entity;
		}

		
	}
}
