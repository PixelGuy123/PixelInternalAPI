﻿using System;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Components;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;
using TMPro;
// using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Object;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// An extension class intended for the creation of components and objects that exists in-game
	/// </summary>
	public static class ObjectCreationExtensions
	{
		// ****************** AudioManager *******************
		// **************** Builder Pattern ******************
		//static readonly FieldInfo _audio_minDistance = AccessTools.Field(typeof(PropagatedAudioManager), "minDistance");
		//static readonly FieldInfo _audio_maxDistance = AccessTools.Field(typeof(PropagatedAudioManager), "maxDistance");
		//static readonly FieldInfo _audio_loopOnStart = AccessTools.Field(typeof(AudioManager), "loopOnStart");
		//static readonly FieldInfo _audio_soundObjects = AccessTools.Field(typeof(AudioManager), "soundOnStart");
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

			audio.minDistance = minDistance;//_audio_minDistance.SetValue(audio, minDistance);
			audio.maxDistance = maxDistance;//_audio_maxDistance.SetValue(audio, maxDistance);

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
		/// Mkaes the <see cref="AudioManager"/> automatically play something upon active. This is useful for looping songs like Playtime's song for example.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="audio"></param>
		/// <param name="loopOnStart"></param>
		/// <param name="startingAudios"></param>
		/// <returns>The <see cref="AudioManager"/> itself.</returns>
		public static T AddStartingAudiosToAudioManager<T>(this T audio, bool loopOnStart, params SoundObject[] startingAudios) where T : AudioManager
		{
			audio.loopOnStart = loopOnStart; //_audio_loopOnStart.SetValue(audio, loopOnStart);
			audio.soundOnStart = startingAudios; //_audio_soundObjects.SetValue(audio, startingAudios);

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

		/// <summary>
		/// Creates an <see cref="Entity"/> component to an object. Warning: Adding an <see cref="Entity"/> component automatically disables the object, so make sure to use this ONLY for intended prefabs.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="collider"></param>
		/// <param name="triggerCollider"></param>
		/// <param name="rendererBase"></param>
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, Collider collider, Collider triggerCollider, Transform rendererBase = null)
		{
			LayerMask mask = target.layer;
			var e = target.AddComponent<Entity>();
			e.SetActive(false);
			target.layer = mask;

			if (rendererBase == null)
			{
				var placeholder = new GameObject("placeholderRendererBase");
				rendererBase = placeholder.transform;
				placeholder.transform.SetParent(target.transform);
				placeholder.transform.localPosition = Vector3.zero;
			}

			e.rendererBase = rendererBase;
			e.collider = collider;
			e.externalActivity = target.AddComponent<ActivityModifier>();
			e.trigger = triggerCollider;

			var rigidBody = target.GetComponent<Rigidbody>();

			if (rigidBody)
				Destroy(rigidBody);

			target.AddStaticRigidBody(); // Adds rigid body to entity

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
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius = 0f, Transform rendererBase = null) =>
			CreateEntity(target, colliderRadius, triggerColliderRadius, out _, out _, rendererBase);

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
		/// <returns>The <see cref="Entity"/> component</returns>
		public static Entity CreateEntity(this GameObject target, float colliderRadius, float triggerColliderRadius, out CapsuleCollider nonTriggerCollider, out CapsuleCollider triggerCollider, Transform rendererBase = null)
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


			return CreateEntity(target, collider, trigger, rendererBase);
		}
		/// <summary>
		/// Sets a collision mask for the <paramref name="entity"/> (not the GameObject layer mask, it's another mask).
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="layer"></param>
		/// <returns>The <paramref name="entity"/> itself</returns>
		public static Entity SetEntityCollisionLayerMask(this Entity entity, LayerMask layer)
		{
			entity.collisionLayerMask = layer; // _entity_CollisionMask.SetValue(entity, layer);
			return entity;
		}

		/// <summary>
		/// Adds a static <see cref="Rigidbody"/> component to the specified <see cref="GameObject"/> with predefined settings (usually for <see cref="Entity"/> or clickable hotspots objects.).
		/// </summary>
		/// <param name="obj">The <see cref="GameObject"/> to which the <see cref="Rigidbody"/> will be added.</param>
		/// <returns>The added <see cref="Rigidbody"/> component.</returns>
		public static Rigidbody AddStaticRigidBody(this GameObject obj)
		{
			var rigid = obj.AddComponent<Rigidbody>();

			rigid.mass = 0f;
			rigid.constraints = RigidbodyConstraints.FreezeAll;
			rigid.angularDrag = 0f;
			rigid.freezeRotation = true;
			rigid.collisionDetectionMode = CollisionDetectionMode.Discrete;
			rigid.maxDepenetrationVelocity = 10f;
			rigid.maxAngularVelocity = 7f;
			rigid.sleepThreshold = 0.005f;
			rigid.solverIterations = 6;
			rigid.solverVelocityIterations = 1;

			return rigid;
		}

		// ************************** Custom Vending Machines ******************************

		internal static SodaMachine prefab;

		//readonly static FieldInfo _sodaMach_meshRenderer = AccessTools.Field(typeof(SodaMachine), "meshRenderer");
		//readonly static FieldInfo _sodaMach_outofstockmat = AccessTools.Field(typeof(SodaMachine), "outOfStockMat");
		//readonly static FieldInfo _sodaMach_potentialitems = AccessTools.Field(typeof(SodaMachine), "potentialItems");
		//readonly static FieldInfo _sodaMach_usesleft = AccessTools.Field(typeof(SodaMachine), "usesLeft");

		/// <summary>
		/// Creates a new <see cref="SodaMachine"/> instance. <paramref name="sodaTex"/> and <paramref name="sodaOutTex"/> sets the textures for new <see cref="SodaMachine"/>.
		/// <para><paramref name="isPrefab"/> disables the <see cref="SodaMachine"/> and set to the DontDestroyOnLoad scene.</para>
		/// </summary>
		/// <param name="sodaTex"></param>
		/// <param name="sodaOutTex"></param>
		/// <param name="isPrefab"></param>
		/// <returns>A <see cref="SodaMachine"/> instance.</returns>
		public static SodaMachine CreateSodaMachineInstance(Texture sodaTex, Texture sodaOutTex, bool isPrefab = true)
		{
			var machine = Instantiate(prefab);


			var renderer = machine.meshRenderer;
			renderer.materials[1].mainTexture = sodaTex;
			machine.outOfStockMat = new Material(machine.outOfStockMat)
			{
				name = sodaOutTex.name,
				mainTexture = sodaOutTex
			};
			BasePlugin._machines.Add(machine.GetComponent<SodaMachineCustomComponent>());

			if (isPrefab)
				machine.gameObject.ConvertToPrefab(true);

			return machine;
		}
		/// <summary>
		/// Set the <paramref name="potentialItems"/> from the <paramref name="mach"/>.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="potentialItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine SetPotentialItems(this SodaMachine mach, params WeightedItemObject[] potentialItems)
		{
			mach.potentialItems = potentialItems; //_sodaMach_potentialitems.SetValue(mach, potentialItems);
			return mach;
		}
		/// <summary>
		/// Sets the <paramref name="usesLeft"/> from the <paramref name="mach"/> (if any value below 0 is inputted, the soda machine will have infinite uses).
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="usesLeft"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine SetUses(this SodaMachine mach, int usesLeft)
		{
			mach.usesLeft = usesLeft; //_sodaMach_usesleft.SetValue(mach, usesLeft <= 0 ? 99 : usesLeft);

			return mach;
		}
		/// <summary>
		/// Adds new <paramref name="potentialItems"/> to the <paramref name="mach"/>.
		/// </summary>
		/// <param name="mach"></param>
		/// <param name="potentialItems"></param>
		/// <returns>The <paramref name="mach"/> itself.</returns>
		public static SodaMachine AddNewPotentialItems(this SodaMachine mach, params WeightedItemObject[] potentialItems)
		{
			//(WeightedItemObject[])_sodaMach_potentialitems.GetValue(mach);
			mach.SetPotentialItems(mach.potentialItems.AddRangeToArray(potentialItems));
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

		// **************** Animated Sprite Rotator ****************
		//readonly static FieldInfo _animatedSpriteRotator_renderer = AccessTools.Field(typeof(AnimatedSpriteRotator), "renderer");
		//readonly static FieldInfo _animatedSpriteRotator_spriteMap = AccessTools.Field(typeof(AnimatedSpriteRotator), "spriteMap");

		/// <summary>
		/// Creates a <see cref="AnimatedSpriteRotator"/> component for the <paramref name="npc"/>.
		/// <para>The <paramref name="rendererIdx"/> indicate which renderer to be used in the <see cref="NPC.spriteRenderer"/> array.</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="npc"></param>
		/// <param name="rendererIdx"></param>
		/// <param name="map"></param>
		/// <returns>The instance of <see cref="AnimatedSpriteRotator"/>.</returns>
		[Obsolete("Use CreateAnimatedSpriteRotator(SpriteRenderer, SpriteRotationMap[]) instead.")]
		public static AnimatedSpriteRotator CreateAnimatedSpriteRotator<T>(this T npc, int rendererIdx, params SpriteRotationMap[] map) where T : NPC =>
			CreateAnimatedSpriteRotator(npc.spriteRenderer[rendererIdx], map);

		/// <summary>
		/// Creates a <see cref="AnimatedSpriteRotator"/> component for the <paramref name="npc"/>.
		/// <para>The default index used for the <see cref="NPC.spriteRenderer"/> is 0.</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="npc"></param>
		/// <param name="map"></param>
		/// <returns>The instance of <see cref="AnimatedSpriteRotator"/>.</returns>
		[Obsolete("Use CreateAnimatedSpriteRotator(SpriteRenderer, SpriteRotationMap[]) instead.")]
		public static AnimatedSpriteRotator CreateAnimatedSpriteRotator<T>(this T npc, params SpriteRotationMap[] map) where T : NPC =>
			CreateAnimatedSpriteRotator(npc, 0, map);

		/// <summary>
		/// Creates a <see cref="AnimatedSpriteRotator"/> component for a renderer.
		/// </summary>
		/// <param name="renderer">The target <see cref="SpriteRenderer"/> to be animated.</param>
		/// <param name="map"></param>
		/// <returns>The instance of <see cref="AnimatedSpriteRotator"/>.</returns>
		public static AnimatedSpriteRotator CreateAnimatedSpriteRotator(this SpriteRenderer renderer, params SpriteRotationMap[] map)
		{
			var animator = renderer.gameObject.AddComponent<AnimatedSpriteRotator>();
			animator.renderer = renderer; //_animatedSpriteRotator_renderer.SetValue(animator, npc.spriteRenderer[rendererIdx]);
			animator.spriteMap = map; //_animatedSpriteRotator_spriteMap.SetValue(animator, map);
			return animator;
		}

		// ******************************* Sprite Bill Boards **********************************
		internal static SpriteRenderer _billboardprefab, _nonbillboardprefab;
		/// <summary>
		/// Returns the bill board prefab used to create sprite billboards.
		/// </summary>
		public static SpriteRenderer BillBoardPrefab => _billboardprefab;
		/// <summary>
		/// Returns the non bill board prefab used to create sprite billboards.
		/// </summary>
		public static SpriteRenderer NonBillBoardPrefab => _nonbillboardprefab;

		/// <summary>
		/// Creates a <see cref="SpriteRenderer"/> object with the default billboard material and a <see cref="RendererContainer"/>.
		/// <para>The sprite will have a billboard by default.</para>
		/// </summary>
		/// <param name="sprite"></param>
		/// <returns></returns>
		public static SpriteRenderer CreateSpriteBillboard(Sprite sprite) =>
			CreateSpriteBillboard(sprite, true);
		/// <summary>
		/// Creates a <see cref="SpriteRenderer"/> object with the default billboard material and a <see cref="RendererContainer"/>.
		/// <para><paramref name="hasBillboard"/> defines whether the sprite has billboard or not (refer to <see cref="ChalkFace"/> sprite when he's in the board as an example).</para>
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="hasBillboard"></param>
		/// <returns>A <see cref="SpriteRenderer"/> instance.</returns>
		public static SpriteRenderer CreateSpriteBillboard(Sprite sprite, bool hasBillboard)
		{
			var obj = Instantiate(hasBillboard ? _billboardprefab : _nonbillboardprefab);

			obj.sprite = sprite;
			if (sprite)
				obj.name = "SpriteBillboard_" + sprite.name;
			obj.gameObject.AddComponent<RendererContainer>().renderers = [obj];
			return obj;
		}
		/// <summary>
		/// Adds a "SpriteHolder" for the <paramref name="renderer"/>. So you can include offset or collision to the renderer without necessarily putting in the same object.
		/// <para><paramref name="holderMask"/> is null by default (and the layer will be the billboard layer, by default). When not null, it becomes the layer of the sprite holder.</para>
		/// <para>Note that the SpriteHolder will be the one with the <see cref="RendererContainer"/> component, removing the one from the <paramref name="renderer"/> (if it exists).</para>
		/// <para>The <paramref name="offset"/> defined will only affect the y axis.</para>
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="offset"></param>
		/// <param name="holderMask"></param>
		/// <returns>The <paramref name="renderer"/> itself.</returns>
		[Obsolete("Use AddSpriteHolder(SpriteRenderer, out SpriteRenderer, Vector3, LayerMask?) instead.")]
		public static SpriteRenderer AddSpriteHolder(this SpriteRenderer renderer, float offset, LayerMask? holderMask = null) =>
			AddSpriteHolder(renderer, Vector3.up * offset, holderMask);
		/// <summary>
		/// Adds a "SpriteHolder" for the <paramref name="renderer"/>. So you can include offset or collision to the renderer without necessarily putting in the same object.
		/// <para><paramref name="holderMask"/> is null by default (and the layer will be the billboard layer, by default). When not null, it becomes the layer of the sprite holder.</para>
		/// <para>Note that the SpriteHolder will be the one with the <see cref="RendererContainer"/> component, removing the one from the <paramref name="renderer"/> (if it exists).</para>
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="offset"></param>
		/// <param name="holderMask"></param>
		/// <returns>The <paramref name="renderer"/> itself.</returns>
		[Obsolete("Use AddSpriteHolder(SpriteRenderer, out SpriteRenderer, Vector3, LayerMask?) instead.")]
		public static SpriteRenderer AddSpriteHolder(this SpriteRenderer renderer, Vector3 offset, LayerMask? holderMask = null)
		{
			renderer.AddSpriteHolder(out var childRenderer, offset, holderMask);
			return childRenderer;
		}
		/// <summary>
		/// Adds a "SpriteHolder" for the <paramref name="renderer"/>. So you can include offset or collision to the renderer without necessarily putting in the same object.
		/// <para><paramref name="holderMask"/> is null by default (and the layer will be the billboard layer, by default). When not null, it becomes the layer of the sprite holder.</para>
		/// <para>Note that the SpriteHolder will be the one with the <see cref="RendererContainer"/> component, removing the one from the <paramref name="renderer"/> (if it exists).</para>
		/// <para>The <paramref name="offset"/> defined will only affect the y axis.</para>
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="childRenderer"></param>
		/// <param name="offset"></param>
		/// <param name="holderMask"></param>
		/// <returns>The <paramref name="renderer"/> itself.</returns>
		public static RendererContainer AddSpriteHolder(this SpriteRenderer renderer, out SpriteRenderer childRenderer, float offset, LayerMask? holderMask = null) =>
			AddSpriteHolder(renderer, out childRenderer, Vector3.up * offset, holderMask);
		/// <summary>
		/// Adds a "SpriteHolder" for the <paramref name="renderer"/>. So you can include offset or collision to the renderer without necessarily putting in the same object.
		/// <para><paramref name="holderMask"/> is null by default (and the layer will be the billboard layer, by default). When not null, it becomes the layer of the sprite holder.</para>
		/// <para>Note that the SpriteHolder will be the one with the <see cref="RendererContainer"/> component, removing the one from the <paramref name="renderer"/> (if it exists).</para>
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="childRenderer"></param>
		/// <param name="offset"></param>
		/// <param name="holderMask"></param>
		/// <returns>The <paramref name="renderer"/> itself.</returns>
		public static RendererContainer AddSpriteHolder(this SpriteRenderer renderer, out SpriteRenderer childRenderer, Vector3 offset, LayerMask? holderMask = null)
		{
			var parent = new GameObject("SpriteBillBoardHolder_" + renderer.name, typeof(RendererContainer));
			childRenderer = renderer;
			renderer.transform.SetParent(parent.transform);
			renderer.transform.localPosition = offset;
			if (renderer.GetComponent<RendererContainer>())
				Destroy(renderer.GetComponent<RendererContainer>()); // The parent should hold it

			parent.GetComponent<RendererContainer>().renderers = [renderer];
			parent.layer = holderMask == null ? LayerStorage.billboardLayer : holderMask.Value; // LayerMask and LayerMask? ARE DIFFERENT???
			return parent.GetComponent<RendererContainer>();
		}
		/// <summary>
		/// Adds a <typeparamref name="T"/> to the <paramref name="renderer"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="renderer"></param>
		/// <param name="animator"></param>
		/// <returns>The <paramref name="renderer"/> itself.</returns>
		public static SpriteRenderer AddSpriteAnimator<T>(this SpriteRenderer renderer, out T animator) where T : CustomSpriteAnimator
		{
			animator = renderer.gameObject.AddComponent<T>();
			animator.spriteRenderer = renderer;
			return renderer;
		}
		// ****************** UI Stuff ********************
		/// <summary>
		/// Creates a simple <see cref="Canvas"/>.
		/// </summary>
		/// <returns>A <see cref="Canvas"/> instance.</returns>
		public static Canvas CreateCanvas()
		{
			var canvas = new GameObject("Canvas").AddComponent<Canvas>();
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.gameObject.layer = LayerStorage.ui;

			var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
			scaler.referenceResolution = new(480f, 360f);
			scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referencePixelsPerUnit = 1f;

			canvas.gameObject.AddComponent<PlaneDistance>().planeDistance = 2f;

			return canvas;
		}
		/// <summary>
		/// Creates a <see cref="TextMeshProUGUI"/> object.
		/// </summary>
		/// <param name="color">The color of the text.</param>
		/// <returns>A <see cref="TextMeshProUGUI"/> instance.</returns>
		public static TextMeshProUGUI CreateTextMeshProUGUI(Color color)
		{
			var text = new GameObject("text").AddComponent<TextMeshProUGUI>();
			text.color = color;
			text.gameObject.layer = LayerStorage.ui;

			return text;
		}
		/// <summary>
		/// Creates an <see cref="Image"/>.
		/// </summary>
		/// <param name="canvas">The <see cref="Canvas"/> the image is supposed to be attached.</param>
		/// <param name="texture">The <see cref="Texture2D"/> of the image.</param>
		/// <param name="coverTheEntireScreen">If true, it'll have the same parameters as the gum overlay, to cover the entire screen.</param>
		/// <returns>An <see cref="Image"/> instance.</returns>
		public static Image CreateImage(Canvas canvas, Texture2D texture, bool coverTheEntireScreen = true) =>
			CreateImage(canvas, texture != null ? AssetLoader.SpriteFromTexture2D(texture, 1f) : null, coverTheEntireScreen);

		/// <summary>
		/// Creates an <see cref="Image"/>.
		/// </summary>
		/// <param name="canvas">The <see cref="Canvas"/> the image is supposed to be attached.</param>
		/// <param name="sprite">The <see cref="Sprite"/> of the image.</param>
		/// <param name="coverTheEntireScreen">If true, it'll have the same parameters as the gum overlay, to cover the entire screen.</param>
		/// <returns>An <see cref="Image"/> instance.</returns>
		public static Image CreateImage(Canvas canvas, Sprite sprite, bool coverTheEntireScreen = true)
		{
			var img = new GameObject("Image").AddComponent<Image>();
			if (sprite != null)
				img.sprite = sprite;

			img.transform.SetParent(canvas.transform);
			img.transform.localPosition = Vector3.zero;
			img.gameObject.layer = LayerStorage.ui;

			if (coverTheEntireScreen)
			{
				img.rectTransform.sizeDelta = new(0f, 360f);
				img.rectTransform.anchorMin = new(0f, 0.5f);
				img.rectTransform.anchorMax = new(1f, 0.5f);
			}

			return img;
		}
		/// <summary>
		/// Creates an <see cref="Image"/> with no texture.
		/// </summary>
		/// <param name="canvas">The <see cref="Canvas"/> the image is supposed to be attached.</param>
		/// <param name="coverTheEntireScreen">If true, it'll have the same parameters as the gum overlay, to cover the entire screen.</param>
		/// <returns>An <see cref="Image"/> instance with no texture.</returns>
		public static Image CreateImage(Canvas canvas, bool coverTheEntireScreen = true) =>
			CreateImage(canvas, sprite: null, coverTheEntireScreen);

	}
}
