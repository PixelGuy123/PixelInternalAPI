using UnityEngine;

namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// Storage for the layers
	/// </summary>
	public static class LayerStorage // Storage for layers (but also some extra things such as Tileoffset
	{
		/// <summary>
		/// Windows Layer
		/// </summary>
		public readonly static LayerMask windowLayer = LayerMask.NameToLayer("Window");
		/// <summary>
		/// Default Billboard Layer
		/// </summary>
		public readonly static LayerMask billboardLayer = LayerMask.NameToLayer("Billboard");
		/// <summary>
		/// Clickable Entities Layer
		/// </summary>
		public readonly static LayerMask iClickableLayer = LayerMask.NameToLayer("ClickableCollidableEntities");
		/// <summary>
		/// Ignore Raycast Layer
		/// </summary>
		public readonly static LayerMask ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
		/// <summary>
		/// Block Raycast Layer
		/// </summary>
		public readonly static LayerMask blockRaycast = LayerMask.NameToLayer("Block Raycast");
		/// <summary>
		/// Entity Standard Layer
		/// </summary>
		public readonly static LayerMask standardEntities = LayerMask.NameToLayer("StandardEntities");
		/// <summary>
		/// Map Layer
		/// </summary>
		public readonly static LayerMask map = LayerMask.NameToLayer("Map");
		/// <summary>
		/// UI Layer
		/// </summary>
		public readonly static LayerMask ui = LayerMask.NameToLayer("UI");
		/// <summary>
		/// A specific layer used by the gum for its entity collision mask
		/// </summary>
		public readonly static LayerMask gumCollisionMask = 2113537;

		/// <summary>
		/// A constant value to indicate the width / height of a tile
		/// </summary>
		public const float TileBaseOffset = 10f;
	}
}
