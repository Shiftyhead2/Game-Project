using Godot;

public partial class ScannerRing : Area2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnArea2DBodyEntered;
		BodyExited += OnArea2DBodyExited;

		Animate();
	}

	private void Animate()
	{
		Tween _tween = CreateTween().SetParallel(true).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

		_tween.TweenProperty(this, "scale", new Vector2(5, 5), 0.5f);
		_tween.TweenProperty(GetNode("Sprite2D"), "modulate:a", 0f, 1f);
		_tween.TweenCallback(Callable.From(QueueFree)).SetDelay(1f);
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if (body is Planet)
		{
			GD.Print($"Collding with {body.Name}");
			Planet collidedPlanet = body as Planet;
			collidedPlanet.GetDetected();
		}
	}

	private void OnArea2DBodyExited(Node2D body)
	{
		if (body is Planet)
		{
			GD.Print($"{body.Name} has exited the ring");
		}
	}

}
