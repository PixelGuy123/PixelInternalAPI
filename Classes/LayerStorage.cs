using UnityEngine;

namespace PixelInternalAPI.Classes
{
	public static class LayerStorage
	{
		public readonly static LayerMask windowLayer = LayerMask.NameToLayer("Window");
		public readonly static LayerMask billboardLayer = LayerMask.NameToLayer("Billboard");
		public readonly static LayerMask iClickableLayer = LayerMask.NameToLayer("ClickableCollidableEntities");
		public readonly static LayerMask ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
		public readonly static LayerMask standardEntities = LayerMask.NameToLayer("StandardEntities");
		public readonly static LayerMask map = LayerMask.NameToLayer("Map");
	}
}
