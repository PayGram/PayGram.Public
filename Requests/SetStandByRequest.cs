namespace PayGram.Public.Requests
{
	/// <summary>
	/// Request body for <c>SetStandByV2</c>.
	/// Toggles the server's maintenance mode on or off. Requires root token.
	/// </summary>
	public class SetStandByRequest
	{
		/// <summary>True to enable standby (maintenance) mode, false to disable it.</summary>
		public bool StandBy { get; set; }
	}
}
