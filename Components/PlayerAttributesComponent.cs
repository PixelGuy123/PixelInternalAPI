using System.Collections.Generic;
using UnityEngine;

namespace PixelInternalAPI.Components
{
	public class PlayerAttributesComponent : MonoBehaviour
	{ 
		private readonly Dictionary<string, int> attributes = [];
		public void AddAttribute(string s) 
		{
			if (attributes.ContainsKey(s))
				attributes[s]++;
			else
				attributes.Add(s, 1);
		}
		public void RemoveAttribute(string s)
		{
			if (attributes.ContainsKey(s))
			{
				int num = --attributes[s];
				if (num <= 0)
					attributes.Remove(s);
			}
		}
		public bool HasAttribute(string s) => attributes.ContainsKey(s);
	}
}
