// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IceCreamBundleKata.cs" company="iMean.CSharp.Kata.Core.IceCreamBundle">
//   Copyright (c) iMean.CSharp.Kata.Core.IceCreamBundle All rights reserved.
// </copyright>
// <author>iMeanBkli</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Frozen;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks.Dataflow;

using iMean.CSharp.Kata.Core.Abstractions;

namespace iMean.CSharp.Kata.Core.IceCreamBundle
{
    public class IceCreamBundleKata : BaseKata
    {
        // ------------------------------------------------------------------------------------------------------------
        // Constants
        // ------------------------------------------------------------------------------------------------------------

        private static readonly string[] FLAVORS = ["fraise", "café", "vanille", "chocolat", "mangue"];

        private static readonly Dictionary<int, double> DISCOUNTS = new()
        {
            { 1, 1d },
            { 2, 0.9 },
            { 3, 0.85 },
            { 4, 0.75 },
            { 5, 0.6 }
        };

        private const int UNIT_PRICE = 10;
        private const int BUNDLE_MAX_SIZE = 5;

        // ------------------------------------------------------------------------------------------------------------
        // Kata Implementation
        // ------------------------------------------------------------------------------------------------------------

        private double ComputePrice(HashSet<string> bundle, int price) =>
            DISCOUNTS[bundle.Count] * bundle.Count * price;

        // ------------------------------------------------------------------------------------------------------------
        // TPL Dataflow
        // ------------------------------------------------------------------------------------------------------------

        private async Task<IDictionary<string, double>> ConsumeAsync(int unitPrice,
            ISourceBlock<KeyValuePair<string, HashSet<string>>> source)
        {
            Dictionary<string, double> output = [];

            while (await source.OutputAvailableAsync())
            {
                KeyValuePair<string, HashSet<string>> data = await source.ReceiveAsync();
                double price = ComputePrice(data.Value, unitPrice);

                output.Add(data.Key, price);
            }

            return output;
        }

        private void ProduceData(string[] input, ITargetBlock<KeyValuePair<string, HashSet<string>>> target)
        {
            var availableFlavors = input.GroupBy(g => g).ToDictionary(k => k.Key, v => v.Count());

            int bundleCount = 0;
            while (availableFlavors.Values.Any(v => v > 0))
            {
                var bundle = new HashSet<string>();

                foreach(string flavor in FLAVORS)
                {
                    if (availableFlavors.TryGetValue(flavor, out int count) && count > 0)
                    {
                        bundle.Add(flavor);
                        availableFlavors[flavor] = count - 1;
                    }
                }

                string bundleName = $"#{ ++bundleCount } - [ {string.Join(", ", bundle)} ]";
                KeyValuePair<string, HashSet<string>> data = new(bundleName, bundle);

                target.Post(data);
            }

            target.Complete();
        }

        private async Task<IceCreamBundleOutput> RunDataflowAsync(IceCreamBundleInput input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(input.Value, nameof(input.Value));

            string[] value = input.Value;
            BufferBlock<KeyValuePair<string, HashSet<string>>> buffer = new();
            Task<IDictionary<string, double>> consumerTask = ConsumeAsync(input.UnitPrice, buffer);

            ProduceData(value, buffer);

            IDictionary<string, double> output = await consumerTask;

            return new IceCreamBundleOutput(output);
        }

        // ------------------------------------------------------------------------------------------------------------
        // Overriden Members
        // ------------------------------------------------------------------------------------------------------------

        public override bool IsAsync => true;

        public override string Name => "IceCreamBundle";

        public override IKataInput GetKataInput()
        {
            Random random = new();
            int inputSize = random.Next(10);
            string[] input = new string[inputSize];

            for (int i = 0; i < inputSize; i++)
            {
                int flavorIndex = random.Next(0, BUNDLE_MAX_SIZE - 1);
                string flavor = FLAVORS[flavorIndex];

                input[i] = flavor;
            }

            return new IceCreamBundleInput(UNIT_PRICE, input);
        }

        protected override async Task<IKataOutput> DoExecuteAsync(IKataInput input)
        {
            return input is IceCreamBundleInput kataInput
                ? (IKataOutput) await RunDataflowAsync(kataInput)
                : throw new InvalidOperationException($"Input {input} must be of type {nameof(kataInput)}.");
        }

        protected class IceCreamBundleInput(int unitPrice, string[] value) : KataInput
        {
            public int UnitPrice { get; } = unitPrice;

            public string[]? Value { get; init; } = value;
        }

        protected class IceCreamBundleOutput(IDictionary<string, double> output) : KataOutput(output)
        {
            private static readonly Dictionary<string, double> EMPTY = [];
            public static readonly IceCreamBundleOutput Default = new(EMPTY);

            public new IDictionary<string, double> Value => (IDictionary<string, double>)base.Value;

            public override string AsStringValue()
            {
                StringBuilder builder = new();

                builder.AppendLine("Bundle Prices: ");

                foreach (KeyValuePair<string, double> bundle in Value)
                {
                    builder.AppendLine($"{bundle.Key}: {string.Join(", ", bundle.Value)}");
                }

                return builder.ToString();
            }
        }
    }
}

