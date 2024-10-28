using Godot;

public partial class RandomNumberGeneratorManager : Node
{
	

	public static RandomNumberGeneratorManager Instance { get; private set; }
	
	private RandomNumberGenerator _randomNumberGenerator;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;

		_randomNumberGenerator = new RandomNumberGenerator();
		
		_randomNumberGenerator.Randomize();
	}


	public int GetRandomNumber(int min, int max)
	{
		return _randomNumberGenerator.RandiRange(min, max);
	}

	
}
