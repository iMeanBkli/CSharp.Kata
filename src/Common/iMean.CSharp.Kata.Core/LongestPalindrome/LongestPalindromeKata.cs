// -----------------------------------------------------------------------------
// <copyright file="JavanaisKata.cs" company="iMean.CSharp.Kata.Core.Javanais">
//   Copyright (c) iMean.CSharp.Kata.Core.Javanais All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// -----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.LongestPalindrome
{
    public class LongestPalindromeKata : BaseKata
    {
        // -------------------------------------
        // Constants
        // -------------------------------------

        private const string INPUT = "Non, il n'est pas bon de ressasser les erreurs du passé.";

        // -------------------------------------
        // Kata Implementation
        // -------------------------------------

        private async Task<LongestPalindromeOutput> FindLongestPalindromeAsync(LongestPalindromeInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Value))
            {
                return LongestPalindromeOutput.Default;
            }

            BufferBlock<char[]> buffer = new();
            ProduceWord(input.Value, buffer);
            Task<string> consumerTask = ConsumeAsync(buffer);

            string output = await consumerTask;

            return new LongestPalindromeOutput(output);
        }

        private bool TryGetPalindrome(char[] characters, out string palindrome)
        {
            palindrome = string.Empty;

            int length = characters.Length;

            if (length == 0)
            {
                return false;
            }

            if (length == 2 &&
                characters[0] == characters[1])
            {
                palindrome = string.Concat(characters);
                return true;
            }

            int midpointIndex = length / 2;

            int startIndex = length % 2 == 0 ? 0 : 1;

            for (int i = startIndex; i < midpointIndex; i++)
            {
                char first = characters[midpointIndex - i];
                char second = characters[midpointIndex + i];

                if (char.ToLower(first) != char.ToLower(second))
                {
                    return false;
                }
            }

            palindrome = new string(characters);

            return true;
        }

        // -------------------------------------
        // TPL Dataflow
        // -------------------------------------

        private void ProduceWord(string input, ITargetBlock<char[]> target)
        {
            List<char> word = [];

            foreach (char @char in input)
            {
                if (char.IsWhiteSpace(@char) || char.IsPunctuation(@char))
                {
                    target.Post([.. word]);
                    word.Clear();

                    continue;
                }

                word.Add(@char);
            }

            target.Complete();
        }

        private async Task<string> ConsumeAsync(ISourceBlock<char[]> source)
        {
            string output = string.Empty;
            int longest = output.Length;

            while (await source.OutputAvailableAsync())
            {
                char[] data = await source.ReceiveAsync();

                if (TryGetPalindrome(data, out string palindrome) &&
                    palindrome.Length > longest)
                {
                    output = palindrome;
                    longest = output.Length;
                }
            }

            return output;
        }

        // -------------------------------------
        // Overriden Members
        // -------------------------------------

        public override string Name => "Longest Palindrome";

        public override bool IsAsync => true;

        public override IKataInput GetKataInput() => new LongestPalindromeInput(INPUT);

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            if (input is LongestPalindromeInput kataInput)
            {
                return await FindLongestPalindromeAsync(kataInput);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof(LongestPalindromeInput)}.");
        }

        protected class LongestPalindromeInput(string value) : KataInput
        {
            public string Value { get; } = value;
        }

        protected class LongestPalindromeOutput(string value) : KataOutput(value)
        {
            public static readonly LongestPalindromeOutput Default = new(string.Empty);

            public new string Value => (string)base.Value;

            public override string AsStringValue() => Value;
        }
    }
}
