using Godot;

public partial class Camera2DFollow : Camera2D
{
	[Export]
	private NodePath _followNode;

	private CharacterBody2D _probe;

	private Tween _followTween;

	public override void _Ready()
	{
		_probe = GetNode<CharacterBody2D>(_followNode);

		if (_probe == null)
		{
			GD.PushError($"Cannot find node at path: {_followNode}");
		}
	}

	public override void _Process(double delta)
	{
		if (_probe != null)
		{
			if (_followTween != null)
			{
				_followTween.Kill();
			}

			_followTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine).SetParallel(true);
			_followTween.TweenProperty(this, "position:x", _probe.Position.X, 0.5f);
			_followTween.TweenProperty(this, "position:y", _probe.Position.Y, 0.5f);
		}
	}
}
