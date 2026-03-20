using System.Text;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.WordValues
{
    public class WordValuesKata : KataExecution
    {
        // -------------------------------------
        // Kata Implementation
        // -------------------------------------

        private WordValuesInput GetWordValuesInput()
        {
            return new WordValuesInput(["abc", "cba abc", "cba"]);
        }

        private WordValuesOutput DoExecute(WordValuesInput input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(input.WordInputValues, nameof(input.WordInputValues));

            string[] words = input.WordInputValues;
            int[] output = new int[words.Length];

            for (int i = 0; i < words.Length; i++)
            {
                output[i] = ComputeWordValue(words[i]) * (i + 1);
            }

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
        // Overriden Members
        // -------------------------------------

        public override string Name => "Word Values";

        protected override IKataInput GetKataInput()
        {
            return GetWordValuesInput();
        }

        protected override IKataOutput DoExecute(IKataInput input)
        {
            if (input is WordValuesInput wordValuesInput)
            {
                return DoExecute(wordValuesInput);
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
