using Spectre.Console;

namespace iMean.CSharp.Kata.Console.Helpers
{
    public class WidgetHelper
    {
        public Panel CreateApplicationNamePanel()
        {
            var panel = new Panel("i[bold][yellow]M[/][/]ean C[blue]#[/] Kata");

            return panel;
        }
    }
}
