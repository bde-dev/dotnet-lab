using FactoryPattern.samples;

namespace FactoryPattern.Factories;

public static class DifferentImplementationsFactoryExtensions
{
	public static void AddVehicleFactory(this IServiceCollection pServices)
	{
		pServices.AddTransient<IVehicle, Car>();
		pServices.AddTransient<IVehicle, Truck>();
		pServices.AddTransient<IVehicle, Van>();
		pServices.AddSingleton<Func<IEnumerable<IVehicle>>>(x => () => x.GetService<IEnumerable<IVehicle>>()!);
		pServices.AddSingleton<IVehicleFactory, VehicleFactory>();
	}
}

public class VehicleFactory : IVehicleFactory
{
	private readonly Func<IEnumerable<IVehicle>> _factory;

	public VehicleFactory(Func<IEnumerable<IVehicle>> pFactory)
	{
		_factory = pFactory;
	}

	public IVehicle Create(string name)
	{
		var lSet = _factory();
		IVehicle lOutput = lSet.Where(x => x.VehicleType == name).First();
		return lOutput;
	}
}