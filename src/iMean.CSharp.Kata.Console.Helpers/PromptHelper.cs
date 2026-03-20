using iMean.CSharp.Kata.Core.Abstractions;

using Spectre.Console;

namespace iMean.CSharp.Kata.Console.Helpers
{
    public class PromptHelper
    {
        public SelectionPrompt<IKataExecution> CreateKataSelectionPrompt(IEnumerable<IKataExecution> katas)
        {
            SelectionPrompt<IKataExecution> prompt = new();

            prompt.Title("Select a [green]kata[/] to run.");
            prompt.MoreChoicesText("[grey](Move up and down to see more katas)[/]");
            prompt.WrapAround();
            prompt.HighlightStyle(new Style(Color.Cyan1, decoration: Decoration.Bold));
            prompt.UseConverter(k => $"[bold]{k.Name}[/]");
            prompt.AddChoices(katas);

            return prompt;
        }
    }
}
