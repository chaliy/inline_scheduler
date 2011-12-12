namespace InlineScheduler.Advanced.State
{
    public interface IStateProvider
    {
        void Store(string key, WorkState state);

        WorkState Retrieve(string key);
    }    
}