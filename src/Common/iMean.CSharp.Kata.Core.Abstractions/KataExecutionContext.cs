namespace iMean.CSharp.Kata.Core.Abstractions
{
    public class KataExecutionContext
    {
        private readonly Stack<IKataExecution> _history;

        public KataExecutionContext()
        {
            _history = new Stack<IKataExecution>();
        }

        public IEnumerable<IKataExecution> History => _history;

        public IKataOutput Run(IKataExecution kata)
        {
            try
            {
                return kata.Execute();
            }
            catch
            {
                throw;
            }
        }

    }
}
