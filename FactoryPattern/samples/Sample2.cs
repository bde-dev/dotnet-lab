namespace FactoryPattern.samples;

public class Sample2 : ISample2
{
	public int RandomValue { get; set; }

	public Sample2()
	{
		RandomValue = Random.Shared.Next(1, 101);
	}
}