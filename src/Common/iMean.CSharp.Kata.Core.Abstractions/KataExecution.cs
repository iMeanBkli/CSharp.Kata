namespace iMean.CSharp.Kata.Core.Abstractions
{
    public abstract class KataExecution : IKataExecution
    {
        public abstract string Name { get; }

        public IKataOutput Execute()
        {
            try
            {
                IKataInput input = GetKataInput();

                return DoExecute(input);
            }
            catch
            {
                throw;
            }
        }

        protected abstract IKataInput GetKataInput();

        protected abstract IKataOutput DoExecute(IKataInput input);

        protected abstract class KataInput : IKataInput { }

        protected abstract class KataOutput(object value) : IKataOutput
        {
            public object Value { get; protected set; } = value;

            public abstract string AsStringValue();
        }
    }
}
