using FactoryPattern.samples;

namespace FactoryPattern.Factories;

public static class GenerateClassWithDataFactoryExtension
{
	public static void AddGenericDataClassWithFactory(this IServiceCollection pServices)
	{
		pServices.AddTransient<IUserData, UserData>();
		pServices.AddSingleton<Func<IUserData>>(x => () => x.GetService<IUserData>()!);
		pServices.AddSingleton<IUserDataFactory, UserDataFactory>();
	}
}

public class UserDataFactory : IUserDataFactory
{
	private readonly Func<IUserData> _factory;

	public UserDataFactory(Func<IUserData> pFactory)
	{
		_factory = pFactory;
	}

	public IUserData Create(string name)
	{
		var lOutput = _factory();
		lOutput.Name = name;
		return lOutput;
	}
}