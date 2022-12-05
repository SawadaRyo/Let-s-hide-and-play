public interface IPresenter
{
    void EventMethod<T>() where T : IModel<T>;
}
