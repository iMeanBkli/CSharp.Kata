namespace iMean.CSharp.Kata.Core.Abstractions
{
    public interface IKataExecution
    {
        public string Name { get; }

        public IKataOutput Execute();
    }

    public interface IKataInput;

    public interface IKataOutput
    {
        public bool HasValue => Value is not null;

        public object? Value { get; }

        public string AsStringValue();
    }

    public interface IKataOutput<T> : IKataOutput
    {
        public new T Value { get; }
    }
}
