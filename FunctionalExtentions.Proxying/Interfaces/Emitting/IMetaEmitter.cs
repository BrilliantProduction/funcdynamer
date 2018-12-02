using System.Reflection.Emit;

using FunctionalExtentions.Proxying.Emitting;

namespace FunctionalExtentions.Proxying.Interfaces.Emitting
{
    /// <summary>
    /// Represents a CLR metadata emitter
    /// whether member or type
    /// </summary>
    public interface IMetaEmitter
    {
        /// <summary>
        /// Gets the type builder associated with this member or
        /// it's owner
        /// </summary>
        TypeBuilder TypeBuilder { get; }

        /// <summary>
        /// Gets the owner of current emitter
        /// <remarks>It could be parent class or null if object is a top emitter.</remarks>
        /// </summary>
        IMetaEmitter Owner { get; }

        /// <summary>
        /// Ensures that this emitter is in valid
        /// state
        /// </summary>
        void EnsureIsValid();

        /// <summary>
        /// Emits the metadata and returns a result <see cref="EmitResult"/>.
        /// <para/>
        /// <remarks>
        /// A result is returned in any case but it could be failed
        /// if something had gone wrong.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        EmitResult EmitMetadata();
    }
}
