// -----------------------------------------------------------------------------
// <copyright file="$safeitemname$.cs" company="$rootnamespace$">
//   Copyright (c) $rootnamespace$ All rights reserved.
// </copyright>
// <author>$author$</author>
// -----------------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace $rootnamespace$
{

    public class $fileinputname$Kata : BaseKata
    {
        // -------------------------------------
        // Constants
        // -------------------------------------

        private static readonly TInput INPUT = null;

        // -------------------------------------
        // Kata Implementation
        // -------------------------------------


        // -------------------------------------
        // TPL Dataflow
        // -------------------------------------

        private async Task<TOutput> ConsumeAsync(ISourceBlock<TProduct> source)
        {
            TOutput output = default;

            while (await source.OutputAvailableAsync())
            {
                TProduct data = await source.ReceiveAsync();
            }

            return output;
        }

        private void ProduceData(TInput input, ITargetBlock<TProduct> target)
        {
            target.Complete();
        }

        private async Task<$fileinputname$Output> RunDataflowAsync($fileinputname$Input input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));

            TInput value = input.Input;
            BufferBlock<TProduct> buffer = new();
            Task<TOutput> consumerTask = ConsumeAsync(buffer);

            ProduceData(value, buffer);

            TOutput output = await consumerTask;

            return new $fileinputname$Output(output);
        }

        // -------------------------------------
        // Overriden Members
        // -------------------------------------

        public override bool IsAsync => true;

        public override string Name => "$fileinputname$";

        protected override IKataInput GetKataInput() => new $fileinputname$Input(INPUT);

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is $fileinputname$Input kataInput)
            {
                return await RunDataflowAsync(kataInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(kataInput)}.");
        }

        protected class $fileinputname$Input(TInput value) : KataInput
        {
            public TInput? Value { get; init; } = value;
        }

        protected class $fileinputname$Output(TOutput output) : KataOutput(output)
        {
            public static readonly $fileinputname$Output Default = new(default);

            public new TOutput Value => (TOutput)base.Value;

            public override string AsStringValue()
            {
                StringBuilder builder = new();

                return builder.ToString();
            }
        }
    }
}

