using System;
using System.Collections;
using System.Collections.Generic;

using FunctionalExtentions.Proxying.Enums;

namespace FunctionalExtentions.Proxying.Emitting
{
    public class EmitResult : IReadOnlyList<EmitResult>
    {
        private List<EmitResult> _children;

        private EmitResult()
        {
            _children = new List<EmitResult>(0);
        }

        public EmitResult(Type type)
            :this()
        {
            EmittedType = type;
            ResultCode = EmitResultCode.TypeEmission;
        }

        public EmitResult(Type type, IEnumerable<string> messages)
            : this(type)
        {
            Messages = messages;
        }

        public EmitResult(Exception exception)
            : this()
        {
            ResultCode = EmitResultCode.Failed;
            Exception = exception;
        }

        public EmitResult(Exception error, EmitResultCode operation)
            : this()
        {
            Exception = error;
            ResultCode = operation | EmitResultCode.Failed;
        }

        public Type EmittedType { get; }

        public EmitResultCode ResultCode { get; }

        public Exception Exception { get; }

        public IEnumerable<string> Messages { get; }

        public void AddSubResult(EmitResult childResult)
        {
            _children.Add(childResult);
        }

        #region IReadOnlyList implementation

        public int Count => _children.Count;

        public IEnumerator<EmitResult> GetEnumerator() => _children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public EmitResult this[int index] => _children[index];

        #endregion
    }
}
