/*
using UnityEngine;
using System.Collections.Generic;

namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// A class to overwrite the audio system from the game.
	/// </summary>
	public static class GlobalAudioListenerModifier
	{
		internal static void PauseListener(bool p)
		{
			if (p)
				pause++;
			else
				pause--;
			if (pause < 0)
				pause = 0;
		}
		internal static bool ListenerIsPaused => pause > 0;
		/// <summary>
		/// Adds a volume modifier to the game's audio.
		/// </summary>
		/// <param name="mod"></param>
		public static void AddVolumeModifier(float mod)
		{
			modifiers.Add(mod);
			AudioListener.volume = GetVolume();
		}
		/// <summary>
		/// Remove the volume modifier to the game's audio.
		/// </summary>
		/// <param name="mod"></param>
		public static void RemoveVolumeModifier(float mod)
		{
			modifiers.Remove(mod);
			AudioListener.volume = GetVolume();
		}

		static float GetVolume()
		{
			float val = defaultVolume;
			for (int i = 0; i < modifiers.Count; i++)
				val *= modifiers[i];
			return val;
		}
		internal static void Reset()
		{
			modifiers.Clear();
			AudioListener.volume = GetVolume();
			pause = 0;
			AudioListener.pause = false;
		}
		static readonly float defaultVolume = AudioListener.volume;

		static readonly List<float> modifiers = [];

		static int pause = 0;
	}
}
*/
