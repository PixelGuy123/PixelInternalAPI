using UnityEngine;
using System.Collections.Generic;

namespace PixelInternalAPI.Components
{
	internal class SodaMachineCustomComponent : MonoBehaviour // Yes, this is internal. There's no good use for this outside.
	{
		[SerializeField]
		internal List<Items> requiredItems = [];
	}
}
