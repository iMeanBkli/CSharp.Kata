// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WordValuesKata.cs" company="iMean.CSharp.Kata.Core.WordValuesKata">
//   Copyright (c) iMean.CSharp.Kata.Core.WordValuesKata All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.WordValues
{
    public class WordValuesKata : BaseKata
    {
        // ------------------------------------------------------------------------------------------------------------
        // Constants
        // ------------------------------------------------------------------------------------------------------------

        private static readonly string[] INPUT = ["abc", "cba abc", "cba"];

        private const string WHITE_SPACE = " ";
        private static readonly int LOWERCASE_A_ASCII_CODE = (int)'a';
        private static readonly int LOWERCASE_Z_ASCII_CODE = (int)'z';


        // ------------------------------------------------------------------------------------------------------------
        // Kata Implementation
        // ------------------------------------------------------------------------------------------------------------

        private int ComputeWordValue(string word)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(word);

            if (word.Any(l =>
                (int)l < LOWERCASE_A_ASCII_CODE ||
                (int)l > LOWERCASE_Z_ASCII_CODE))
            {
                throw new ArgumentException($"Word must contain only alphabetical letters. '{word}' is invalid.");
            }

            string trimedWord = word.Replace(WHITE_SPACE, string.Empty);

            int wordValue = trimedWord.Select(c => char.ToLower(c) - LOWERCASE_A_ASCII_CODE).Sum();

            return wordValue;
        }

        // ------------------------------------------------------------------------------------------------------------
        // TPL Dataflow
        // ------------------------------------------------------------------------------------------------------------

        private async Task<int[]> ConsumeAsync(ISourceBlock<KeyValuePair<int, string>> source)
        {
            List<int> values = [];

            while (await source.OutputAvailableAsync())
            {
                KeyValuePair<int, string> data = await source.ReceiveAsync();

                int value = ComputeWordValue(data.Value) * data.Key;
                values.Add(value);
            }

            return [.. values];
        }

        private void ProduceData(string[] input, ITargetBlock<KeyValuePair<int, string>> target)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var word = new KeyValuePair<int, string>(i + 1, input[i].ToLower());

                target.Post(word);
            }

            target.Complete();
        }

        private async Task<WordValuesOutput> RunDataflowAsync(WordValuesInput input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(input.Value, nameof(input.Value));

            string[] value = input.Value;
            BufferBlock<KeyValuePair<int, string>> buffer = new();
            Task<int[]> consumerTask = ConsumeAsync(buffer);

            ProduceData(value, buffer);

            int[] output = await consumerTask;

            return new WordValuesOutput(output);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Overriden Members
        // ------------------------------------------------------------------------------------------------------------

        public override bool IsAsync => true;

        public override string Name => "WordValues";

        public override IKataInput GetKataInput() => new WordValuesInput(INPUT);

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is WordValuesInput kataInput)
            {
                return await RunDataflowAsync(kataInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(WordValuesInput)}.");
        }

        protected class WordValuesInput(string[] value) : KataInput
        {
            public string[]? Value { get; init; } = value;
        }

        protected class WordValuesOutput(int[] output) : KataOutput(output)
        {
            public static readonly WordValuesOutput Default = new([]);

            public new int[] Value => (int[]) base.Value;

            public override string AsStringValue()
            {
                StringBuilder builder = new();

                builder.Append("{ ");

                for(int i = 0; i < Value.Length; i++)
                {
                    builder.Append(Value[i]);

                    if (i < Value.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.Append(" }");

                return builder.ToString();
            }
        }
    }
}
