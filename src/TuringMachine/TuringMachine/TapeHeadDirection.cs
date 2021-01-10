namespace TuringMachine
{
    /// <summary>
    /// Movement direction of the tape's head.
    /// </summary>
    public enum TapeHeadDirection
    {
        /// <summary>
        /// Not moves anywhere.
        /// </summary>
        Stay = 1,

        /// <summary>
        /// Movement in the left direction.
        /// </summary>
        Left,

        /// <summary>
        /// Movement in the right direction.
        /// </summary>
        Right
    }
}
