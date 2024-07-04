using Godot;

public partial class ScannerRing : Area2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animate();
	}

	private void animate()
	{
		Tween tween = GetTree().CreateTween().BindNode(this).SetParallel(true).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

		tween.TweenProperty(this, "scale", new Vector2(5, 5), 0.5f);
		tween.TweenProperty(GetNode("Sprite2D"), "modulate:a", 0f, 1f);
		tween.TweenCallback(Callable.From(QueueFree)).SetDelay(1f);
	}

}
