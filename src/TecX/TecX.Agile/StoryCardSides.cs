namespace TecX.Agile
{
    /// <summary>
    /// Enum with displayable sides of a story-card
    /// </summary>
    public enum StoryCardSides
    {
        /// <summary>
        /// Default front side
        /// </summary>
        FrontSide = 0,
        /// <summary>
        /// Side for simple test description
        /// </summary>
        PlainTestSide,
        /// <summary>
        /// Side for enhanced test description
        /// </summary>
        RichTestSide,
        /// <summary>
        /// Side for prototype implementation notes
        /// </summary>
        PrototypeSide,
        /// <summary>
        /// Side for handwritten notes
        /// </summary>
        HandwritingSide,
    }
}