using UniRx;

public interface IModel<T>
{
    public IReactiveProperty<T> ModelData { get; }
}

