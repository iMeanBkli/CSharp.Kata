using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.WordValues
{
    public class WordValuesKata : BaseKata
    {
        public override bool IsAsync => false;

        // -------------------------------------
        // Kata Implementation
        // -------------------------------------

        private WordValuesInput GetWordValuesInput()
        {
            return new WordValuesInput(["abc", "cba abc", "cba"]);
        }

        private async Task<WordValuesOutput> ComputeValueAsync(WordValuesInput input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(input.WordInputValues, nameof(input.WordInputValues));

            string[] words = input.WordInputValues;
            BufferBlock<KeyValuePair<int, string>> buffer = new();
            Task<int[]> consumerTask = ConsumeAsync(buffer);

            ProduceWords(words, buffer);

            int[] output = await consumerTask;

            return new WordValuesOutput(output);
        }

        private int ComputeWordValue(string word)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(word);

            const string WHITE_SPACE = " ";
            string trimedWord = word.Replace(WHITE_SPACE, string.Empty);

            int wordValue = trimedWord.Select(c => char.ToUpper(c) - 64).Sum();

            return wordValue;
        }

        // -------------------------------------
        // TPL Dataflow
        // -------------------------------------

        private static void ProduceWords(string[] words, ITargetBlock<KeyValuePair<int, string>> target)
        {
            for (int i = 0; i < words.Length; i++)
            {
                var word = new KeyValuePair<int, string>(i + 1, words[i]);

                target.Post(word);
            }

            target.Complete();
        }

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

        // -------------------------------------
        // Overriden Members
        // -------------------------------------

        public override string Name => "Word Values";

        public override IKataInput GetKataInput() => GetWordValuesInput();

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is WordValuesInput wordValuesInput)
            {
                return await ComputeValueAsync(wordValuesInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(WordValuesInput)}.");
        }

        protected class WordValuesInput(string[] values) : KataInput
        {
            public string[]? WordInputValues { get; init; } = values;
        }

        protected class WordValuesOutput : KataOutput
        {
            public WordValuesOutput(int[] output)
                : base(output) { }

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
