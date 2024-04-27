using HarmonyLib;
using PixelInternalAPI.Components;
using System.Reflection;
using UnityEngine;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// An extension class intended for the creation of components and objects that exists in-game
	/// </summary>
	public static class ObjectCreationExtensions
	{
		// ****************** AudioManager *******************
		// **************** Builder Pattern ******************
		static readonly FieldInfo _audio_minDistance = AccessTools.Field(typeof(PropagatedAudioManager), "minDistance");
		static readonly FieldInfo _audio_maxDistance = AccessTools.Field(typeof(PropagatedAudioManager), "maxDistance");
		static readonly FieldInfo _audio_loopOnStart = AccessTools.Field(typeof(AudioManager), "loopOnStart");
		static readonly FieldInfo _audio_soundObjects = AccessTools.Field(typeof(AudioManager), "soundOnStart");
		/// <summary>
		/// Creates a <see cref="PropagatedAudioManager"/> component.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="minDistance"></param>
		/// <param name="maxDistance"></param>
		/// <returns>The <see cref="PropagatedAudioManager"/> component.</returns>
		public static PropagatedAudioManager CreatePropagatedAudioManager(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
		{
			var audio = target.AddComponent<PropagatedAudioManager>();

			_audio_minDistance.SetValue(audio, minDistance);
			_audio_maxDistance.SetValue(audio, maxDistance);

			return audio;
		}
		/// <summary>
		/// Creates an <see cref="AudioManager"/> component.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="minDistance"></param>
		/// <param name="maxDistance"></param>
		/// <returns>The <see cref="AudioManager"/> component.</returns>
		public static AudioManager CreateAudioManager(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
		{
			var audio = target.AddComponent<AudioManager>();
			audio.audioDevice = target.CreateAudioSource(minDistance, maxDistance);

			return audio;
		}
		/// <summary>
		/// Turns the <see cref="AudioManager"/> into a non positional audio (like the jhonny's shop audio for example).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="man"></param>
		/// <returns>The <see cref="AudioManager"/> itself.</returns>
		public static T MakeAudioManagerNonPositional<T>(this T man) where T : AudioManager
		{
			man.audioDevice.spatialBlend = 0f;
			man.positional = false;
			man.audioDevice.rolloffMode = AudioRolloffMode.Logarithmic;
			return man;
		}
		/// <summary>
		/// Resets an <see cref="AudioManager"/> for an intended prefab (an object in the DontDestroyOnLoad scene that is intended to serve as a prefab). This is intended to be used upon creating the GameObject.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="audio"></param>
		/// <returns>The <see cref="AudioManager"/> itself.</returns>
		public static T SetAudioManagerAsPrefab<T>(this T audio) where T : AudioManager
		{
			if (audio.audioDevice != null && audio.audioDevice.gameObject != audio.gameObject) // Must check for the gameObject aswell to not cause unintended behaviour
				Object.Destroy(audio.audioDevice.gameObject);
			AudioManager.totalIds--;
			if (AudioManager.totalIds < 0)
				AudioManager.totalIds = 0;
			audio.sourceId = 0; // Copypaste from api lmao
			return audio;
		}
		/// <summary>
		/// Mkaes the <see cref="AudioManager"/> automatically play something upon active. This is useful for looping songs like Playtime's song for example.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="audio"></param>
		/// <param name="loopOnStart"></param>
		/// <param name="startingAudios"></param>
		/// <returns>The <see cref="AudioManager"/> itself.</returns>
		public static T AddStartingAudiosToAudioManager<T>(this T audio, bool loopOnStart, params SoundObject[] startingAudios) where T : AudioManager
		{
			_audio_loopOnStart.SetValue(audio, loopOnStart);
			_audio_soundObjects.SetValue(audio, startingAudios);

			return audio;
		}
		/// <summary>
		/// Creates a basic <see cref="AudioSource"/> component.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="minDistance"></param>
		/// <param name="maxDistance"></param>
		/// <returns>The <see cref="AudioSource"/> component.</returns>
		public static AudioSource CreateAudioSource(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
		{
			var audio = target.AddComponent<AudioSource>();
			audio.minDistance = minDistance;
			audio.maxDistance = maxDistance;
			return audio;
		}

		// ******************* Entity ************************

		static readonly FieldInfo _entity_rendererBase = AccessTools.Field(typeof(Entity), "rendererBase");
		static readonly FieldInfo _entity_collider = AccessTools.Field(typeof(Entity), "collider");
		static readonly FieldInfo _entity_trigger = AccessTools.Field(typeof(Entity), "trigger");
		static readonly FieldInfo _entity_activity = AccessTools.Field(typeof(Entity), "externalActivity");
		static readonly FieldInfo _entity_etrigger = AccessTools.Field(typeof(Entity), "iEntityTrigger");
		static readonly FieldInfo _entity_CollisionMask = AccessTools.Field(typeof(Entity), "collisionLayerMask");
		/// <summary>
		/// Creates an <see cref="Entity"/> component to an object. Warning: Adding an <see cref="Entity"/> component automatically disables the object, so make sure to use this ONLY for intended prefabs.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="collider"></param>
		/// <param name="triggerCollider"></param>
		/// <param name="rendererBase"></param>
		/// <param name="triggers"></param>
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, Collider collider, Collider triggerCollider, Transform rendererBase = null, IEntityTrigger[] triggers = null)
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
		/// <summary>
		/// Creates an <see cref="Entity"/> component to an object. Warning: Adding an <see cref="Entity"/> component automatically disables the object, so make sure to use this ONLY for intended prefabs. The default collider added is the <see cref="CapsuleCollider"/>
		/// <para>If the <paramref name="triggerColliderRadius"/> is less or equal to 0, the collider is disabled</para>
		/// </summary>
		/// <param name="target"></param>
		/// <param name="colliderRadius"></param>
		/// <param name="triggerColliderRadius"></param>
		/// <param name="rendererBase"></param>
		/// <param name="triggers"></param>
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius = 0f, Transform rendererBase = null, IEntityTrigger[] triggers = null) =>
			CreateEntity(target, colliderRadius, triggerColliderRadius, out _, out _, rendererBase, triggers);

		/// <summary>
		/// Creates an <see cref="Entity"/> component to an object. Warning: Adding an <see cref="Entity"/> component automatically disables the object, so make sure to use this ONLY for intended prefabs. The default collider added is the <see cref="CapsuleCollider"/>
		/// <para>If the <paramref name="triggerColliderRadius"/> is less or equal to 0, the collider is disabled</para>
		/// </summary>
		/// <param name="target"></param>
		/// <param name="colliderRadius"></param>
		/// <param name="triggerColliderRadius"></param>
		/// <param name="nonTriggerCollider"></param>
		/// <param name="triggerCollider"></param>
		/// <param name="rendererBase"></param>
		/// <param name="triggers"></param>
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius, out CapsuleCollider nonTriggerCollider, out CapsuleCollider triggerCollider, Transform rendererBase = null, IEntityTrigger[] triggers = null)
		{
			var collider = target.AddComponent<CapsuleCollider>();
			collider.radius = colliderRadius;
			nonTriggerCollider = collider;

			var trigger = target.AddComponent<CapsuleCollider>();
			trigger.isTrigger = true;

			if (triggerColliderRadius <= 0f)
				trigger.enabled = false;
			else
				trigger.radius = triggerColliderRadius;

			triggerCollider = trigger;


			return CreateEntity(target, collider, trigger, rendererBase, triggers);
		}
		/// <summary>
		/// Sets a collision mask for the <paramref name="entity"/> (not the GameObject layer mask, it's another mask).
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="layer"></param>
		/// <returns>The <paramref name="entity"/> itself</returns>
		public static Entity SetEntityCollisionLayerMask(this Entity entity, LayerMask layer)
		{
			_entity_CollisionMask.SetValue(entity, layer);
			return entity;
		}

		// ************* Object Placer **************
		readonly static FieldInfo _objplace_prefab = AccessTools.Field(typeof(ObjectPlacer), "prefab");
		readonly static FieldInfo _objplace_min = AccessTools.Field(typeof(ObjectPlacer), "min");
		readonly static FieldInfo _objplace_max = AccessTools.Field(typeof(ObjectPlacer), "max");
		readonly static FieldInfo _objplace_usewalldir = AccessTools.Field(typeof(ObjectPlacer), "useWallDir");
		readonly static FieldInfo _objplace_useopendir = AccessTools.Field(typeof(ObjectPlacer), "useOpenDir");
		readonly static FieldInfo _objplace_includeOpen = AccessTools.Field(typeof(ObjectPlacer), "includeOpen");
		readonly static FieldInfo _genericHall_objectPlacer = AccessTools.Field(typeof(GenericHallBuilder), "objectPlacer");
		/// <summary>
		/// Creates a new <see cref="ObjectPlacer"/> instance. The <paramref name="requiredCoverages"/> are the cell coverages the <see cref="ObjectPlacer"/> needs to properly place an object. The <paramref name="eligibleShapes"/> are the shapes required by the <see cref="ObjectPlacer"/>.
		/// <para>Note: if the <paramref name="prefab"/> doesn't contain an <see cref="EnvironmentObject"/>, it'll include one automatically.</para>
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="requiredCoverages"></param>
		/// <param name="eligibleShapes"></param>
		/// <returns>Returns an <see cref="ObjectPlacer"/> instance</returns>
		public static ObjectPlacer SetANewObjectPlacer(GameObject prefab, CellCoverage requiredCoverages, params TileShape[] eligibleShapes)
		{
			var placer = new ObjectPlacer
			{
				eligibleShapes = [.. eligibleShapes],
				coverage = requiredCoverages
			};
			if (!prefab.GetComponent<EnvironmentObject>())
				prefab.AddComponent<EnvironmentObject>(); // This is required for the ObjectPlacer
			
			_objplace_prefab.SetValue(placer, prefab);

			return placer;
		}
		/// <summary>
		/// Sets the <paramref name="min"/> and <paramref name="max"/> values of objects that <paramref name="pl"/> should spawn.
		/// </summary>
		/// <param name="pl"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns>The <paramref name="pl"/> itself.</returns>
		public static ObjectPlacer SetMinAndMaxObjects(this ObjectPlacer pl, int min, int max) 
		{
			_objplace_min.SetValue(pl, min);
			_objplace_max.SetValue(pl, max);
			return pl;		
		}
		/// <summary>
		/// Set the tile preferences for the <paramref name="pl"/>. <paramref name="useWallDir"/> allows the objects to spawn facing a wall. <paramref name="useOpenDir"/> allows the objects to spawn facing open tiles. <paramref name="includeOpenTiles"/> allows open tiles.
		/// </summary>
		/// <param name="pl"></param>
		/// <param name="useWallDir"></param>
		/// <param name="useOpenDir"></param>
		/// <param name="includeOpenTiles"></param>
		/// <returns>The <paramref name="pl"/> itself.</returns>
		public static ObjectPlacer SetTilePreferences(this ObjectPlacer pl, bool useWallDir, bool useOpenDir, bool includeOpenTiles)
		{
			_objplace_useopendir.SetValue(pl, useOpenDir);
			_objplace_usewalldir.SetValue(pl, useWallDir);
			_objplace_includeOpen.SetValue(pl, includeOpenTiles);
			return pl;
		}
		/// <summary>
		/// Sets the <c>objectPlacer</c> field from <paramref name="b"/> to the <paramref name="placer"/>.
		/// </summary>
		/// <param name="b"></param>
		/// <param name="placer"></param>
		/// <returns>The <paramref name="b"/> itself.</returns>
		public static GenericHallBuilder SetObjectPlacer(this GenericHallBuilder b, ObjectPlacer placer)
		{
			_genericHall_objectPlacer.SetValue(b, placer);
			return b;
		}

		// ************************** Custom Vending Machines ******************************

		internal static SodaMachine prefab;

		readonly static FieldInfo _sodaMach_meshRenderer = AccessTools.Field(typeof(SodaMachine), "meshRenderer");
		readonly static FieldInfo _sodaMach_outofstockmat = AccessTools.Field(typeof(SodaMachine), "outOfStockMat");
		readonly static FieldInfo _sodaMach_potentialitems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
		readonly static FieldInfo _sodaMach_usesleft = AccessTools.Field(typeof(SodaMachine), "usesLeft");
		readonly static FieldInfo _sodaMach_requireditem = AccessTools.Field(typeof(SodaMachine), "requiredItem");

		/// <summary>
		/// Creates a new <see cref="SodaMachine"/> instance. <paramref name="sodaTex"/> and <paramref name="sodaOutTex"/> sets the textures for new <see cref="SodaMachine"/>. <paramref name="isPrefab"/> disables the <see cref="SodaMachine"/> and set to the DontDestroyOnLoad scene.
		/// </summary>
		/// <param name="sodaTex"></param>
		/// <param name="sodaOutTex"></param>
		/// <param name="isPrefab"></param>
		/// <returns>A <see cref="SodaMachine"/> instance.</returns>
		public static SodaMachine CreateSodaMachineInstance(Texture sodaTex, Texture sodaOutTex, bool isPrefab = true)
		{
			var machine = Object.Instantiate(prefab);
			if (isPrefab)
			{
				Object.DontDestroyOnLoad(machine.gameObject);
				machine.gameObject.SetActive(false);
			}

			var renderer = (MeshRenderer)_sodaMach_meshRenderer.GetValue(machine);
			renderer.material.mainTexture = sodaTex;
			((Material)_sodaMach_outofstockmat.GetValue(machine)).mainTexture = sodaOutTex;

			return machine;
		}
		/// <summary>
		/// Set the <paramref name="potentialItems"/> from the <paramref name="mach"/> and sets the <paramref name="usesLeft"/> aswell.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="usesLeft"></param>
		/// <param name="potentialItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine SetPotentialItemsAndUses(this SodaMachine mach, int usesLeft, params WeightedItemObject[] potentialItems)
		{
			_sodaMach_potentialitems.SetValue(mach, potentialItems);
			_sodaMach_usesleft.SetValue(mach, usesLeft);
			return mach;
		}
		/// <summary>
		/// Set the <paramref name="usesLeft"/> from the <paramref name="mach"/> and add new <paramref name="potentialItems"/> to the <paramref name="mach"/>.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="usesLeft"></param>
		/// <param name="potentialItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine SetUsesAndAddNewPotentialItems(this SodaMachine mach, int usesLeft, params WeightedItemObject[] potentialItems)
		{
			var itms = (WeightedItemObject[])_sodaMach_potentialitems.GetValue(mach);
			itms = itms.AddRangeToArray(potentialItems);
			mach.SetPotentialItemsAndUses(usesLeft, itms);
			return mach;
		}
		/// <summary>
		/// Set the required items for the <paramref name="mach"/> through the <see cref="SodaMachineCustomComponent"/>.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="acceptableItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine SetRequiredItems(this SodaMachine mach, params Items[] acceptableItems)
		{
			var comp = mach.GetComponent<SodaMachineCustomComponent>();
			comp.requiredItems.Clear();
			comp.requiredItems.AddRange(acceptableItems);
			return mach;
		}
		/// <summary>
		/// Add new required items for the <paramref name="mach"/> through the <see cref="SodaMachineCustomComponent"/>.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="acceptableItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine AddNewRequiredItems(this SodaMachine mach, params Items[] acceptableItems)
		{
			var comp = mach.GetComponent<SodaMachineCustomComponent>();
			comp.requiredItems.AddRange(acceptableItems);
			return mach;
		}



	}
}
