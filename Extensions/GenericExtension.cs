using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace PixelInternalAPI.Extensions
{

	public static class GenericExtensions
	{
		public static bool CompareFloats(this float a, float b) =>
			Mathf.Abs(a - b) <= float.Epsilon;
		public static void ReplaceAt<T>(this IList<T> list, int index, T replacement)
		{
			list.RemoveAt(index);
			list.Insert(index, replacement);
		}

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

		public static IEnumerable<C> ConvertAll<T, C>(this IEnumerable<T> sequence, Func<T, C> func)
		{
			foreach (var item in sequence)
				yield return func(item);
		}

		public static CodeMatcher CustomAction(this CodeMatcher m, Action<CodeMatcher> a)
		{
			a(m);
			return m;
		}

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

		public static List<Transform> AllChilds(this Transform transform)
		{
			List<Transform> cs = [];
			int count = transform.childCount;
			for (int i = 0; i < count; i++)
				cs.Add(transform.GetChild(i));
			return cs;
		}

		public static T DuplicatePrefab<T>(this T obj) where T : Component
		{
			var o = UnityEngine.Object.Instantiate(obj);
			UnityEngine.Object.DontDestroyOnLoad(o.gameObject);
			o.name = obj.name;
			return o;
		}

		public static CodeMatcher GetCodeInstruction(this CodeMatcher m, out CodeInstruction i)
		{
			i = m.Instruction;
			return m;
		}

		public static T FindResourceObjectByName<T>(string name) where T : UnityEngine.Object =>
			FindResourceObjects<T>().First(x => x.name == name);
		public static T FindResourceObject<T>() where T : UnityEngine.Object =>
			Resources.FindObjectsOfTypeAll<T>().First(x => x.GetInstanceID() > 0);
		public static T[] FindResourceObjects<T>() where T : UnityEngine.Object =>
			[.. Resources.FindObjectsOfTypeAll<T>().Where(x => x.GetInstanceID() > 0)];

		public static CodeMatcher InsertAnIfBlock(this CodeMatcher m, CodeInstruction[] parameters, OpCode ifOpcode, params CodeInstruction[] instructions) =>
			m.Insert(instructions)
			.InsertAndAdvance(parameters)
			.InsertBranch(ifOpcode, m.Pos + instructions.Length + 1);
			

		public static CodeMatcher InsertAnIfBlockAndAdvance(this CodeMatcher m, CodeInstruction[] parameters, OpCode ifOpcode, params CodeInstruction[] instructions) =>
			m.Insert(instructions)
			.InsertAndAdvance(parameters)
			.InsertBranch(ifOpcode, m.Pos + instructions.Length + 1)
			.Advance(instructions.Length);
		


		public static CodeMatcher GoTo(this CodeMatcher m, int pos) =>
			m.Advance(pos - m.Pos);


	}
}
