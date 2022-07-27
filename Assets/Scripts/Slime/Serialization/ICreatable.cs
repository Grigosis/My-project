namespace ROR.Core.Serialization
{
    public interface ICreatable
    {
        public void Init(IState state);
        public IState GetState();
    }
}