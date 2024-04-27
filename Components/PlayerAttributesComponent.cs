using PixelInternalAPI.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	/// <summary>
	/// A component to add attributes to the player.
	/// </summary>
	public class PlayerAttributesComponent : MonoBehaviour
	{ 
		void Awake()
		{
			pm = GetComponent<PlayerManager>();
			walkSpeed = pm.plm.walkSpeed;
			runSpeed = pm.plm.runSpeed;
			staminaDrop = pm.plm.staminaDrop;
			staminaMax = pm.plm.staminaMax;
			staminaRise = pm.plm.staminaRise;
		}

		private readonly Dictionary<string, int> attributes = [];
		/// <summary>
		/// Adds an attribute to the player.
		/// </summary>
		/// <param name="s"></param>
		public void AddAttribute(string s) 
		{
			if (attributes.ContainsKey(s))
				attributes[s]++;
			else
				attributes.Add(s, 1);
		}
		/// <summary>
		/// Removes the attribute from the player.
		/// </summary>
		/// <param name="s"></param>
		public void RemoveAttribute(string s)
		{
			if (attributes.ContainsKey(s))
			{
				int num = --attributes[s];
				if (num <= 0)
					attributes.Remove(s);
			}
		}
		/// <summary>
		/// If the attribute exists in the player.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>true if the attribute exists in the player, otherwise false.</returns>
		public bool HasAttribute(string s) => attributes.ContainsKey(s);

		/// <summary>
		/// The stamina mods (you can add and remove).
		/// </summary>
		readonly public List<StaminaModifier> StaminaMods = [];
		/// <summary>
		/// The speed mods (you can add and remove).
		/// </summary>
		readonly public List<SpeedModifier> SpeedMods = [];

		void Update()
		{
			float wspeed = walkSpeed, rspeed = runSpeed;

			for (int i = 0; i < SpeedMods.Count; i++)
			{
				if (!SpeedMods[i].Adder)
				{
					wspeed *= SpeedMods[i].WalkSpeed;
					rspeed *= SpeedMods[i].RunSpeed;
				}
				else
				{
					wspeed += SpeedMods[i].WalkSpeed;
					rspeed += SpeedMods[i].RunSpeed;
				}
			}

			pm.plm.walkSpeed = wspeed;
			pm.plm.runSpeed = rspeed;

			float sRise = staminaRise, sDrop = staminaDrop, sMax = staminaMax;

			for (int i = 0; i < StaminaMods.Count; i++)
			{
				sRise *= StaminaMods[i].StaminaRiseMod;
				sDrop *= StaminaMods[i].StaminaDropMod;
				sMax *= StaminaMods[i].StaminaMaxMod;
			}

			pm.plm.staminaDrop = sDrop;
			pm.plm.staminaMax = sMax;
			pm.plm.staminaRise = sRise;

		}

		PlayerManager pm;
		/// <summary>
		/// Returns the <see cref="PlayerManager"/> referenced by the component.
		/// </summary>
		public PlayerManager Pm => pm;

		float walkSpeed, runSpeed, staminaRise, staminaDrop, staminaMax;
	}
}
