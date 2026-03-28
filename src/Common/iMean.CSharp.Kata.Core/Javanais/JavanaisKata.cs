// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavanaisKata.cs" company="iMean.CSharp.Kata.Core.Javanais">
//   Copyright (c) iMean.CSharp.Kata.Core.Javanais All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.Javanais
{
    public class JavanaisKata : BaseKata
    {
        // ------------------------------------------------------------------------------------------------------------
        // Constants
        // ------------------------------------------------------------------------------------------------------------

        private const string INPUT = "This is my Javanais implementation";

        private const string CYPHER = "av";
        private static readonly char[] VOWELS = ['a', 'e', 'i', 'o', 'u', 'y'];

        private const int UPPERCASE_A_ASCII_CODE = 65;
        private const int UPPERCASE_Z_ASCII_CODE = 90;
        private const int LOWERCASE_A_ASCII_CODE = 97;
        private const int LOWERCASE_Z_ASCII_CODE = 122;


        // ------------------------------------------------------------------------------------------------------------
        // Kata Implementation
        // ------------------------------------------------------------------------------------------------------------

        private string TranslateToJavanais(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            StringBuilder output = new();

            if (IsVowel(word[0]))
            {
                output.Append(CYPHER);
            }

            output.Append(word);

            return output.ToString();
        }

        private static bool IsLetter(char character) => (int)character
            is
            (>= UPPERCASE_A_ASCII_CODE and <= UPPERCASE_Z_ASCII_CODE)
            or
            (>= LOWERCASE_A_ASCII_CODE and <= LOWERCASE_Z_ASCII_CODE);


        private static bool IsVowel(char letter) => VOWELS.Any(v => v == letter);

        // ------------------------------------------------------------------------------------------------------------
        // TPL Dataflow
        // ------------------------------------------------------------------------------------------------------------

        private async Task<string> ConsumeAsync(ISourceBlock<string> source)
        {
            StringBuilder output = new();

            while (await source.OutputAvailableAsync())
            {
                string data = await source.ReceiveAsync();

                string result = TranslateToJavanais(data);
                output.Append(result);
            }

            return output.ToString();
        }

        private void ProduceData(string input, ITargetBlock<string> target)
        {
            StringBuilder chunk = new();

            foreach (char @char in input)
            {
                chunk.Append(@char);

                if (!IsLetter(@char) || !IsVowel(@char))
                {
                    target.Post(chunk.ToString());
                    chunk.Clear();
                }
            }

            target.Complete();
        }

        private async Task<JavanaisOutput> RunDataflowAsync(JavanaisInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Value))
            {
                return JavanaisOutput.Default;
            }

            BufferBlock<string> buffer = new();
            Task<string> consumerTask = ConsumeAsync(buffer);

            ProduceData(input.Value, buffer);
            string output = await consumerTask;

            return new JavanaisOutput(output);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Overriden Members
        // ------------------------------------------------------------------------------------------------------------

        public override string Name => "Javanais";

        public override bool IsAsync => true;

        public override IKataInput GetKataInput() => new JavanaisInput(INPUT);

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is JavanaisInput kataInput)
            {
                return await RunDataflowAsync(kataInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(JavanaisInput)}.");
        }

        protected class JavanaisInput(string value) : KataInput
        {
            public string Value { get; init; } = value;
        }

        protected class JavanaisOutput(string output) : KataOutput(output)
        {
            public static readonly JavanaisOutput Default = new(string.Empty);

            public new string Value => (string)base.Value;

            public override string AsStringValue() => Value;
        }
    }
}

