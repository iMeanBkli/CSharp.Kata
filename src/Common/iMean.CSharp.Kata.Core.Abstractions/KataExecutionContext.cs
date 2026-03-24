namespace iMean.CSharp.Kata.Core.Abstractions
{
    public class KataExecutionContext
    {
        public IKataOutput Run(IKata kata) => DoRunAsync(kata).GetAwaiter().GetResult();

        public async Task<IKataOutput> RunAsync(IKata kata) => await DoRunAsync(kata);

        private async Task<IKataOutput> DoRunAsync(IKata kata)
        {
            try
            {
                return await kata.ExecuteAsync();
            }
            catch (Exception e)
            {
                throw new KataExecutionException($"Kata '{kata.Name}' execution terminated with an error.", e, kata);
            }
        }
    }

    public class KataExecutionException : Exception
    {
        private readonly IKata? _kata;

        public KataExecutionException(string message) : base(message) { }

        public KataExecutionException(string message, Exception innerException) : base(message, innerException) { }

        public KataExecutionException(string message, IKata kata)
            : base(message)
        {
            _kata = kata;
        }

        public KataExecutionException(string message, Exception innerException, IKata kata)
            : base(message, innerException)
        {
            _kata = kata;
        }

        public string KataName => _kata == null ? string.Empty : _kata.Name;
    }
}
