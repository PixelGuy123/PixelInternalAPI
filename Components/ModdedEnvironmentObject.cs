namespace PixelInternalAPI.Components
{
	/// <summary>
	/// This class is used to guarantee any <see cref="EnvironmentObject"/> can properly spawn inside rooms (excluding hallways).
	/// </summary>
	public class ModdedEnvironmentObject : EnvironmentObject
	{
		/// <summary>
		/// This method is overrided to set the position and rotation to where it should be.
		/// </summary>
		public override void LoadingFinished()
		{
			base.LoadingFinished();
			if (definedCell != null)
				transform.position = definedCell.FloorWorldPosition;
			if (definedDir != Direction.Null)
				transform.rotation = definedDir.ToRotation();
		}

		internal Cell definedCell;

		internal Direction definedDir = Direction.Null;
	}
}
