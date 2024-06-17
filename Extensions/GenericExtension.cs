using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection.Emit;
using MTM101BaldAPI;
using PixelInternalAPI.Components;
//using System.Reflection;

namespace PixelInternalAPI.Extensions
{
	/// <summary>
	/// This class provides a bunch of generic extensions that can be useful in many situations.
	/// </summary>
	public static class GenericExtensions
	{
		/// <summary>
		/// Performs a quadratic equation. Using <paramref name="x"/> as the independent value.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns>Result of the Quadratic equation.</returns>
		public static float QuadraticEquation(this float x, float a, float b, float c) =>
			(a * (x * x)) + (b * x) + c;
		/// <summary>
		/// Performs a linear equation. Using <paramref name="x"/> as the independent value.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>Result of the Linear equation.</returns>
		public static float LinearEquation(this float x, float a, float b) =>
			(a * x) + b;
		/// <summary>
		/// Compare the equality between two floats with a value near 0 (0.01).
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>true if both values are nearly equal, otherwise false.</returns>
		public static bool CompareFloats(this float a, float b) =>
			Mathf.Abs(a - b) <= 0.01; // I guess this works?
		/// <summary>
		/// Replace a value from the <paramref name="list"/> at <paramref name="index"/> with <paramref name="replacement"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <param name="replacement"></param>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public static void ReplaceAt<T>(this IList<T> list, int index, T replacement)
		{
			list.RemoveAt(index);
			list.Insert(index, replacement);
		}
		/// <summary>
		/// Add all elements from a <paramref name="collection"/> to the main collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="coll"></param>
		/// <param name="collection"></param>
		public static void AddRange<T>(this ICollection<T> coll, IEnumerable<T> collection) =>
			collection.Do(coll.Add);
		/// <summary>
		/// Replace an item in the <paramref name="list"/> with the <paramref name="replacement"/> based on a <paramref name="predicate"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="predicate"></param>
		/// <param name="replacement"></param>
		/// <returns>true if an item was successfully replaced, otherwise false.</returns>
		public static bool Replace<T>(this IList<T> list, Predicate<T> predicate, T replacement)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					ReplaceAt(list, i, replacement);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Replace all items in the <paramref name="list"/> with the <paramref name="replacement"/> based on a <paramref name="predicate"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="predicate"></param>
		/// <param name="replacement"></param>
		/// <returns>The amount of items replaced.</returns>
		public static int ReplaceAll<T>(this IList<T> list, Predicate<T> predicate, T replacement)
		{
			int replaceds = 0;
			for (int i = 0; i < list.Count; i++)
				if (predicate(list[i]))
				{
					ReplaceAt(list, i, replacement);
					replaceds++;
				}
				
			return replaceds;
		}
		/// <summary>
		/// Convert all items in a <paramref name="sequence"/> using a defined <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="C"></typeparam>
		/// <param name="sequence"></param>
		/// <param name="func"></param>
		/// <returns>A new collection with all items converted to type <typeparamref name="C"/>.</returns>
		public static IEnumerable<C> ConvertAll<T, C>(this IEnumerable<T> sequence, Func<T, C> func)
		{
			foreach (var item in sequence)
				yield return func(item);
		}
		/// <summary>
		/// Does a custom action in a CodeMatcher.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="a"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		public static CodeMatcher CustomAction(this CodeMatcher m, Action<CodeMatcher> a)
		{
			a(m);
			return m;
		}
		/// <summary>
		/// Calls the <see cref="Debug.Log(object)"/> method in all the CodeInstructions available in <paramref name="m"/>. <paramref name="ogPos"/> sets the index it should begin to debug (default is -1, which means all instructions).
		/// <para>Here is the following format: <c>"Index: OpCode >> Operand"</c></para>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="ogPos"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		public static CodeMatcher DebugLogAllInstructions(this CodeMatcher m, int ogPos = -1)
		{
			if (ogPos < 0)
			{
				ogPos = m.Pos;
				m.Start();
			}

			if (m.IsInvalid)
			{
				m.Advance(ogPos - m.Pos);
				return m;
			}

			Debug.Log($"{m.Pos}: {m.Opcode} >> {m.Operand}");
			m.Advance(1);
			return DebugLogAllInstructions(m, m.Pos - ogPos);
		}
		/// <summary>
		/// Returns all childs from the <paramref name="transform"/>.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns>A list of all childs from the <paramref name="transform"/>.</returns>
		public static List<Transform> AllChilds(this Transform transform)
		{
			int count = transform.childCount;
			if (count == 0)
				return [];
			List<Transform> cs = [];
			
			for (int i = 0; i < count; i++)
				cs.Add(transform.GetChild(i));
			return cs;
		}

		/// <summary>
		/// Returns all childs from the <paramref name="transform"/>. <paramref name="includeDescendants"/> if it should also include the child of the childs (in other words, everything).
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="includeDescendants"></param>
		/// <returns>A list of all childs from the <paramref name="transform"/>.</returns>
		public static List<Transform> AllChilds(this Transform transform, bool includeDescendants)
		{
			if (!includeDescendants)
				return transform.AllChilds();

			List<Transform> cs = [];
			Queue<Transform> csq = new();
			csq.Enqueue(transform);
			while (csq.Count > 0)
			{
				var childs = csq.Dequeue().AllChilds();
				foreach (var c in childs)
				{
					cs.Add(c);
					csq.Enqueue(c);
				}
			}
			
			return cs;
		}
		/// <summary>
		/// Instantiate an <paramref name="obj"/> but maintaning the same name and put in the HideAndDontSave scene.
		/// </summary>
		/// <param name="obj"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>An instantiated <paramref name="obj"/>.</returns>
		public static T DuplicatePrefab<T>(this T obj) where T : Component
		{
			var o = UnityEngine.Object.Instantiate(obj);
			o.gameObject.ConvertToPrefab(o.gameObject.activeSelf);
			o.name = obj.name;
			return o;
		}
		/// <summary>
		/// Instantiate an <paramref name="obj"/> but maintaning the same name and put in the DontDestroyOnLoad scene.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>An instantiated <paramref name="obj"/>.</returns>
		public static GameObject DuplicatePrefab(this GameObject obj)
		{
			var o = UnityEngine.Object.Instantiate(obj);
			o.ConvertToPrefab(o.activeSelf);
			o.name = obj.name;
			return o;
		}
		/// <summary>
		/// Get the code instruction (<paramref name="i"/>) and set in an out parameter.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="i"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		public static CodeMatcher GetCodeInstruction(this CodeMatcher m, out CodeInstruction i)
		{
			i = m.Instruction;
			return m;
		}
		/// <summary>
		/// Find a resource object by the <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns>The first resource object that has the passed <paramref name="name"/>.</returns>
		/// /// <exception cref="NullReferenceException"></exception>
		public static T FindResourceObjectByName<T>(string name) where T : UnityEngine.Object =>
			FindResourceObjects<T>().First(x => x.name == name);
		/// <summary>
		/// Get the first resource object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>The first resource object of type <typeparamref name="T"/>.</returns>
		/// <exception cref="NullReferenceException"></exception>
		public static T FindResourceObject<T>() where T : UnityEngine.Object =>
			Resources.FindObjectsOfTypeAll<T>().First(x => x.GetInstanceID() > 0);
		/// <summary>
		/// Find all resource objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>All resource objects of type <typeparamref name="T"/>.</returns>
		/// /// <exception cref="NullReferenceException"></exception>
		public static T[] FindResourceObjects<T>() where T : UnityEngine.Object =>
			[.. Resources.FindObjectsOfTypeAll<T>().Where(x => x.GetInstanceID() > 0)];
		/// <summary>
		/// Inserts an If block (of single condition) into the IL Instructions.
		/// <para><paramref name="ifOpcode"/> is the opcode that defines the condition check.</para>
		/// <para><paramref name="instructions"/> are the instructions to be done inside the if block.</para>
		/// <para><paramref name="parameters"/> are the parameters passed for the condition check.</para>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="parameters"></param>
		/// <param name="ifOpcode"></param>
		/// <param name="instructions"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static CodeMatcher InsertAnIfBlock(this CodeMatcher m, CodeInstruction[] parameters, OpCode ifOpcode, params CodeInstruction[] instructions) =>
			m.Insert(instructions)
			.InsertAndAdvance(parameters)
			.InsertBranch(ifOpcode, m.Pos + instructions.Length + 1);

		/// <summary>
		/// Inserts an If block (of single condition) into the IL Instructions and advance.
		/// <para><paramref name="ifOpcode"/> is the opcode that defines the condition check.</para>
		/// <para><paramref name="instructions"/> are the instructions to be done inside the if block.</para>
		/// <para><paramref name="parameters"/> are the parameters passed for the condition check.</para>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="parameters"></param>
		/// <param name="ifOpcode"></param>
		/// <param name="instructions"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static CodeMatcher InsertAnIfBlockAndAdvance(this CodeMatcher m, CodeInstruction[] parameters, OpCode ifOpcode, params CodeInstruction[] instructions) =>
			m.Insert(instructions)
			.InsertAndAdvance(parameters)
			.InsertBranch(ifOpcode, m.Pos + instructions.Length + 1)
			.Advance(instructions.Length);
		

		/// <summary>
		/// Advances to a specified index (<paramref name="pos"/>);
		/// </summary>
		/// <param name="m"></param>
		/// <param name="pos"></param>
		/// <returns>The <paramref name="m"/> itself.</returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public static CodeMatcher GoTo(this CodeMatcher m, int pos) =>
			m.Advance(pos - m.Pos);

		// *********************************** In game extensions ***********************************

		/// <summary>
		/// Return the amount of empty slots in the inventory (excluding locked slots).
		/// </summary>
		/// <param name="man"></param>
		/// <returns>The amount of empty slots in the inventory.</returns>
		public static int SlotsAvailable(this ItemManager man)
		{
			//bool[] lockeds = (bool[])_itm_slotLocked.GetValue(man);
			int a = 0;
			for (int i = 0; i <= man.maxItem; i++)
				if (man.items[i].itemType == Items.None && !man.slotLocked[i])//!lockeds[i])
					a++;
			
			return a;
		}
		/// <summary>
		/// Tells if the slot from <paramref name="man"/> at <paramref name="idx"/> is locked or not.
		/// </summary>
		/// <param name="man"></param>
		/// <param name="idx"></param>
		/// <returns>If true, the slot is locked, otherwise false.</returns>
		public static bool IsSlotLocked(this ItemManager man, int idx) =>
			man.slotLocked[idx]; //((bool[])_itm_slotLocked.GetValue(man))[idx];
		/// <summary>
		/// A simple method to set the field <c>bypassRotation</c> from the <see cref="AnimatedSpriteRotator"/>.
		/// <para>If <paramref name="bypass"/> is true. The <see cref="AnimatedSpriteRotator"/> will begin using the <see cref="AnimatedSpriteRotator.targetSprite"/> instead of the map.</para>
		/// </summary>
		/// <param name="rotator"></param>
		/// <param name="bypass"></param>
		public static void BypassRotation(this AnimatedSpriteRotator rotator, bool bypass) =>
			rotator.bypassRotation = bypass; //animatedsprite_bypassrotation.SetValue(rotator, bypass);

		/// <summary>
		/// Creates an instance of <see cref="SpriteRotationMap"/>.
		/// </summary>
		/// <param name="angleCount"></param>
		/// <param name="sprites"></param>
		/// <returns>An instance of <see cref="SpriteRotationMap"/>.</returns>
		public static SpriteRotationMap CreateRotationMap(int angleCount, params Sprite[] sprites)
		{
			var map = new SpriteRotationMap
			{
				angleCount = angleCount,
				spriteSheet = sprites // rotMap_sprites.SetValue(map, sprites);
			};
			return map;
		}
		/// <summary>
		/// Adds a room function directly into the <paramref name="asset"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="RoomFunction"/> to be added into the <paramref name="asset"/>.</typeparam>
		/// <param name="asset">The <see cref="RoomAsset"/> to contain the <typeparamref name="T"/></param>
		public static T AddRoomFunction<T>(this RoomAsset asset) where T : RoomFunction
		{
			var fun = asset.roomFunctionContainer.GetComponent<T>();
			if (!fun)
			{
				fun = asset.roomFunctionContainer.gameObject.AddComponent<T>();
				asset.roomFunctionContainer.AddFunction(fun);
			}
			return fun;
		}
		/// <summary>
		/// Adds an existing <typeparamref name="T"/> object into the <paramref name="asset"/>.
		/// </summary>
		/// <param name="asset">The target <see cref="RoomAsset"/>.</param>
		/// <param name="func">The <typeparamref name="T"/> object to be added.</param>
		/// <typeparam name="T">The type of the <see cref="RoomFunction"/>.</typeparam>
		public static void AddRoomFunction<T>(this RoomAsset asset, T func) where T : RoomFunction =>
			asset.roomFunctionContainer.AddFunction(func);
		/// <summary>
		/// Returns the <see cref="NPCAttributesContainer"/> from the <paramref name="npc"/>.
		/// </summary>
		/// <param name="npc">The npc.</param>
		/// <returns>The <see cref="NPCAttributesContainer"/> component.</returns>
		public static NPCAttributesContainer GetNPCContainer(this NPC npc) =>
			npc.GetComponent<NPCAttributesContainer>();
		/// <summary>
		/// Gets the <see cref="CustomPlayerCameraComponent"/> through the <paramref name="cam"/>.
		/// </summary>
		/// <param name="cam">The <see cref="GameCamera"/>.</param>
		/// <returns>The <see cref="CustomPlayerCameraComponent"/> component.</returns>
		public static CustomPlayerCameraComponent GetCustomCam(this GameCamera cam) =>
			cam.GetComponent<CustomPlayerCameraComponent>();
		/// <summary>
		/// Gets the <see cref="CustomPlayerCameraComponent"/> through the <paramref name="pm"/>.
		/// </summary>
		/// <param name="pm">The <see cref="PlayerManager"/> itself.</param>
		/// <returns>The <see cref="CustomPlayerCameraComponent"/> component.</returns>
		public static CustomPlayerCameraComponent GetCustomCam(this PlayerManager pm) =>
			Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).GetCustomCam();


		//readonly static FieldInfo rotMap_sprites = AccessTools.Field(typeof(SpriteRotationMap), "spriteSheet");
		//readonly static FieldInfo animatedsprite_bypassrotation = AccessTools.Field(typeof(AnimatedSpriteRotator), "bypassRotation");
		//readonly static FieldInfo _itm_slotLocked = AccessTools.Field(typeof(ItemManager), "slotLocked");
	}
}
