using FactoryPattern.samples;

namespace FactoryPattern.Factories;

public static class AbstractFactoryExtension
{
	public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection pServices)
	where TInterface : class
	where TImplementation : class, TInterface
	{
		pServices.AddTransient<TInterface, TImplementation>();
		pServices.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>()!);
		pServices.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
	}
}

public class AbstractFactory<T> : IAbstractFactory<T>
{
	private readonly Func<T> _factory;

	public AbstractFactory(Func<T> pFactory)
    {
		_factory = pFactory;
	}

	public T Create()
	{
		return _factory();
	}
}

public interface IAbstractFactory<T>
{
	T Create();
}