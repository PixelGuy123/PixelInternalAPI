namespace PixelInternalAPI.Classes
{
	/// <summary>
	/// A class that holds a selection and the limitations for that selection.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="C"></typeparam>
	/// <param name="sel"></param>
	/// <param name="weight"></param>
	/// <param name="selectionLimiter"></param>
	public class SelectionHolder<T, C>(T sel, int weight, C[] selectionLimiter)
	{
		readonly WeightedSelection<T> selection = new() { selection = sel, weight = weight };
		/// <summary>
		/// The Selection (of type <typeparamref name="T"/>).
		/// </summary>
		public WeightedSelection<T> Selection => selection;

		readonly C[] selectionLimiters = selectionLimiter;
		/// <summary>
		/// The limits of the selection (of type <typeparamref name="C"/>).
		/// </summary>
		public C[] SelectionLimiters => selectionLimiters;
	}
}
