namespace FactoryPattern.samples
{
	public interface IVehicle
	{
		string VehicleType { get; set; }

		string Start();
	}
}