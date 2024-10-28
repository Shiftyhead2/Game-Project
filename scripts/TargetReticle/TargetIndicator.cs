using Godot;

public partial class TargetIndicator : Node2D
{

	[Export] private Probe _probe;
	[Export] private Signals _owner;

	[Export]
	private Line2D _line;

	[Export]
	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier;

	private bool _visible = false;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await Owner.ToSignal(Owner, Node.SignalName.Ready);
		_owner = Owner as Signals;
		_probe = GameManager.Instance.GetProbe();
	}


	public override void _EnterTree()
	{
		_visibleOnScreenNotifier.ScreenEntered += OnScreenEnter;
		_visibleOnScreenNotifier.ScreenExited += OnScreenExited;
	}


	public override void _Process(double delta)
	{
		if (_probe != null && _owner != null)
		{
			if (_owner.IsDetected())
			{
				_line.Points = new Vector2[] { _probe.Position, _owner.Position };
			}
		}

		if (_visible)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	private void OnScreenEnter()
	{
		_visible = true;
	}

	private void OnScreenExited()
	{
		_visible = false;
	}


}
