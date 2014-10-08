using System.IO;

namespace TecX.Agile.Serialization
{
    /// <summary>
    /// Defines an interface for custom binary serialization
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Writes data to the <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="writer">The writer.</param>
        void WriteDataTo(BinaryWriter writer);

        /// <summary>
        /// Sets data from the <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="reader">The reader.</param>
        void SetDataFrom(BinaryReader reader);

        /// <summary>
        /// Gets the id for the type
        /// </summary>
        /// <returns>The unique id for the type</returns>
        /// <remarks>Should be implemented to return a constant unique
        /// integer id for the type</remarks>
        int GetTypeId();
    }
}
