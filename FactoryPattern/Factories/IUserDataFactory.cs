using FactoryPattern.samples;

namespace FactoryPattern.Factories
{
	public interface IUserDataFactory
	{
		IUserData Create(string name);
	}
}