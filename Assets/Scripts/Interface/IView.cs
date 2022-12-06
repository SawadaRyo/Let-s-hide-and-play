using UniRx;

public interface IView
{
    void EventMethod<T>(T modelData);
}
