using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace PixelInternalAPI.Extensions
{
	public static class ObjectCreationExtensions
	{
		static readonly FieldInfo _audio_minDistance = AccessTools.Field(typeof(PropagatedAudioManager), "minDistance");
		static readonly FieldInfo _audio_maxDistance = AccessTools.Field(typeof(PropagatedAudioManager), "maxDistance");

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
			audio.sourceId = 0; // Copypaste from api lmao

			return audio;
		}

		static readonly FieldInfo _entity_rendererBase = AccessTools.Field(typeof(Entity), "rendererBase");
		static readonly FieldInfo _entity_collider = AccessTools.Field(typeof(Entity), "collider");
		static readonly FieldInfo _entity_trigger = AccessTools.Field(typeof(Entity), "trigger");
		static readonly FieldInfo _entity_activity = AccessTools.Field(typeof(Entity), "externalActivity");
		static readonly FieldInfo _entity_etrigger = AccessTools.Field(typeof(Entity), "iEntityTrigger");

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

		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius = 0f, Transform rendererBase = null, IEntityTrigger[] triggers = null)
		{
			var collider = target.AddComponent<CapsuleCollider>();
			collider.radius = colliderRadius;

			CapsuleCollider trigger = null;
			if (triggerColliderRadius > 0f)
			{
				trigger = target.AddComponent<CapsuleCollider>();
				trigger.radius = triggerColliderRadius;
				trigger.isTrigger = true;
			}

			return CreateEntity(target, collider, trigger, rendererBase, triggers);
		}
	}
}
