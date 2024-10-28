using Godot;

public partial class DetectionRing : Area2D
{

	private Sprite2D _sprite;

	private Vector2 _defaultScale = new Vector2(0.1f, 0.1f);

	private Color _defaultColor = new Color(1f, 1f, 1f, 1f);

	private Tween _tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		OnScan();
	}


	public void OnScan()
	{
		Scale = _defaultScale;
		_sprite.Modulate = _defaultColor;
		Visible = true;
		Animate();
	}

	private void Animate()
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
		_tween = CreateTween().SetParallel().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

		_tween.TweenProperty(this, "scale", new Vector2(5, 5), 0.5f);
		_tween.TweenProperty(_sprite, "modulate", new Color(_sprite.Modulate.R, _sprite.Modulate.G, _sprite.Modulate.B, 0f), 1f);
		_tween.TweenCallback(Callable.From(SetVisible)).SetDelay(1f);
	}



	private void SetVisible()
	{
		Visible = false;
	}
}
