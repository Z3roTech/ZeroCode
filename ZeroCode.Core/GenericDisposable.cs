using System;

namespace ZeroCode
{
    /// <summary>
    ///     Class implemented <see cref="IDisposable" />, that can be set to invoke custom action on
    ///     <see cref="IDisposable.Dispose" />. Useful for making custom unsubscribes.
    /// </summary>
    public class GenericDisposable : IDisposable
    {
        /// <summary>
        ///     Empty instance of generic disposable
        /// </summary>
        public static readonly GenericDisposable Empty = new GenericDisposable();

        /// <summary>
        ///     Action that must be invoked on dispose of this object. Provide state as argument.
        /// </summary>
        private readonly Action<object?>? _actionOnDispose;

        /// <summary>
        ///     Action that must be invoked on dispose of this object.
        /// </summary>
        private readonly Action? _actionOnDisposeWithoutState;

        /// <summary>
        ///     Custom state value that will be provided into action method on dispose.
        /// </summary>
        private readonly object? _state;

        public GenericDisposable() { }

        /// <param name="actionOnDispose">
        ///     <inheritdoc cref="_actionOnDisposeWithoutState" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericDisposable(Action actionOnDispose)
        {
            _actionOnDisposeWithoutState = actionOnDispose ?? throw new ArgumentNullException(nameof(actionOnDispose));
        }

        /// <param name="actionOnDispose">
        ///     <inheritdoc cref="_actionOnDispose" />
        /// </param>
        /// <param name="state">
        ///     <inheritdoc cref="_state" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericDisposable(Action<object?> actionOnDispose, object? state)
        {
            _state = state;
            _actionOnDispose = actionOnDispose ?? throw new ArgumentNullException(nameof(actionOnDispose));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _actionOnDispose?.Invoke(_state);
            _actionOnDisposeWithoutState?.Invoke();
        }
    }
}