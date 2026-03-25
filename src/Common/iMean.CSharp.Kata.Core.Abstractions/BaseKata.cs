// -----------------------------------------------------------------------------
// <copyright file="BaseKata.cs" company="iMean.CSharp.Kata.Core.Abstractions">
//   Copyright (c) iMean.CSharp.Kata.Core.Abstractions All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// -----------------------------------------------------------------------------

namespace iMean.CSharp.Kata.Core.Abstractions
{
    public abstract class BaseKata : IKata
    {
        public abstract string Name { get; }

        public abstract bool IsAsync { get; }

        public abstract IKataInput GetKataInput();

        public IKataOutput Execute()
        {
            try
            {
                IKataInput input = GetKataInput();

                return DoExecuteAsync(input).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IKataOutput> ExecuteAsync()
        {
            try
            {
                IKataInput input = GetKataInput();

                return await DoExecuteAsync(input);
            }
            catch
            {
                throw;
            }
        }

        protected abstract Task<IKataOutput> DoExecuteAsync(IKataInput input);

        protected abstract class KataInput : IKataInput { }

        protected abstract class KataOutput(object value) : IKataOutput
        {
            public object Value { get; protected set; } = value;

            public abstract string AsStringValue();
        }
    }
}
