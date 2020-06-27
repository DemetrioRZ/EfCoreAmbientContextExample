namespace EfCoreAmbientContextExample
{
    public interface IContextScopeFactory
    {
        IContextScope CreateContextScope();
    }
}