// -----------------------------------------------------------------------------
// <copyright file="$safeitemname$.cs" company="$rootnamespace$">
//   Copyright (c) $rootnamespace$ All rights reserved.
// </copyright>
// <author>$author$</author>
// -----------------------------------------------------------------------------

using System.Text;

using iMean.CSharp.Kata.Core.Abstractions;

namespace $rootnamespace$
{

    public class $fileinputname$Kata : KataExecution
    {
        // -------------------------------------
        // Kata Implementation
        // -------------------------------------

        private $fileinputname$Input GetWordValuesInput(
        {
            return new $fileinputname$Input($input$);
        }

        private $fileinputname$Output DoExecute($fileinputname$Input input)
        {
            return new $fileinputname$Output();
        }

        // -------------------------------------
        // Overriden Members
        // -------------------------------------

        public override string Name => "$fileinputname$";

        protected override IKataInput GetKataInput()
        {
            return GetWordValuesInput();
        }

        protected override IKataOutput DoExecute(IKataInput input)
        {
            if (input is $fileinputname$Input $title$Input)
            {
                return DoExecute($title$Input);
            }

            throw new InvalidOperationException($"Input {input} must be of type {nameof($fileinputname$Input)}.");
        }

        protected class $fileinputname$Input($input$) : KataInput
        {
            // Input properties are defined here.
        }

        protected class $fileinputname$Output : KataOutput
        {
            public $fileinputname$Output($output$)
                : base(output) { }

            public new $output$ Value => ($output$)base.Value;

            public override string AsStringValue()
            {
                StringBuilder builder = new();

                // Build output string using StringBuilder

                return builder.ToString();
            }
        }
    }
}

