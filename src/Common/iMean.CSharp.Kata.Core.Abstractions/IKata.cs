// -----------------------------------------------------------------------------
// <copyright file="IKata.cs" company="iMean.CSharp.Kata.Core.Abstractions">
//   Copyright (c) iMean.CSharp.Kata.Core.Abstractions All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// -----------------------------------------------------------------------------

namespace iMean.CSharp.Kata.Core.Abstractions
{
    public interface IKata
    {
        public string Name { get; }

        public bool IsAsync { get; }

        public IKataInput GetKataInput();

        public IKataOutput Execute();

        public Task<IKataOutput> ExecuteAsync();
    }

    public interface IKataInput;

    public interface IKataOutput
    {
        public bool HasValue => Value is not null;

        public object? Value { get; }

        public string AsStringValue();
    }

    public interface IKataOutput<T> : IKataOutput
    {
        public new T Value { get; }
    }
}
