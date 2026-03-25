// -----------------------------------------------------------------------------
// <copyright file="JavanaisKata.cs" company="iMean.CSharp.Kata.Core.Javanais">
//   Copyright (c) iMean.CSharp.Kata.Core.Javanais All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// -----------------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.Javanais
{
    public class JavanaisKata : BaseKata
    {
        // -------------------------------------
        // Constants
        // -------------------------------------

        private const string INPUT = "This is my Javanais implementation";

        private const string CYPHER = "av";
        private static readonly char[] VOWELS = ['a', 'e', 'i', 'o', 'u', 'y'];

        private const int UPPERCASE_A_ASCII_CODE = 65;
        private const int UPPERCASE_Z_ASCII_CODE = 90;
        private const int LOWERCASE_A_ASCII_CODE = 97;
        private const int LOWERCASE_Z_ASCII_CODE = 122;


        // -------------------------------------
        // Kata Implementation
        // -------------------------------------

        private async Task<JavanaisOutput> TranslateToJavanaisAsync(JavanaisInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Word))
            {
                return JavanaisOutput.Default;
            }

            BufferBlock<string> buffer = new();
            Task<string> consumerTask = ConsumeAsync(buffer);

            ProduceChunks(input.Word, buffer);

            string result = await consumerTask;

            JavanaisOutput output = new(result);

            return output;
        }

        private static string TranslateToJavanais(string word)
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

        // -------------------------------------
        // TPL Dataflow
        // -------------------------------------

        private static void ProduceChunks(string word, ITargetBlock<string> target)
        {
            StringBuilder chunk = new();

            foreach (char @char in word)
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

        private async Task<string> ConsumeAsync(ISourceBlock<string> source)
        {
            StringBuilder output = new();

            while(await source.OutputAvailableAsync())
            {
                string data = await source.ReceiveAsync();
                string encrypted = TranslateToJavanais(data);

                output.Append(encrypted);
            }

            return output.ToString();
        }

        // -------------------------------------
        // Overriden Members
        // -------------------------------------

        public override string Name => "Javanais";

        public override bool IsAsync => true;

        public override IKataInput GetKataInput() => new JavanaisInput(INPUT);

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is JavanaisInput kataInput)
            {
                return await TranslateToJavanaisAsync(kataInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(JavanaisInput)}.");
        }

        protected class JavanaisInput(string word) : KataInput
        {
            public string Word { get; init; } = word;
        }

        protected class JavanaisOutput(string output) : KataOutput(output)
        {
            public static readonly JavanaisOutput Default = new(string.Empty);

            public new string Value => (string)base.Value;

            public override string AsStringValue() => Value;
        }
    }
}

