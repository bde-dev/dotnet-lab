using FactoryPattern.samples;

namespace FactoryPattern.Factories
{
	public interface IVehicleFactory
	{
		IVehicle Create(string name);
	}
}