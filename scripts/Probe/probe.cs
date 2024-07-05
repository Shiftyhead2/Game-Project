using Godot;

public partial class probe : CharacterBody2D
{
	[Export]
	private float _speed = 300.0f;
	[Export]
	private float _acceleration = 10.0f;

	[Export]
	private float _deacceleration = 5.0f;


	private Vector2 _velocity;
	private Vector2 _direction;

	[Export]
	private Marker2D _spawnPoint;

	[Export]
	private PackedScene _scannerPackedScene;

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("scan"))
		{
			SpawnScannerRing();
		}
	}


	public override void _Process(double delta)
	{
		_velocity = Velocity;

		_direction = Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized();

		if (_direction != Vector2.Zero)
		{
			_velocity.X = Mathf.MoveToward(_velocity.X, _speed * _direction.X, _acceleration);
			_velocity.Y = Mathf.MoveToward(_velocity.Y, _speed * _direction.Y, _acceleration);
		}
		else
		{
			_velocity.X = Mathf.MoveToward(_velocity.X, 0f, _deacceleration);
			_velocity.Y = Mathf.MoveToward(_velocity.Y, 0f, _deacceleration);
		}

		Velocity = _velocity;
		MoveAndSlide();
	}

	private void SpawnScannerRing()
	{
		PackedScene _scannerToAdd = ResourceLoader.Load<PackedScene>(_scannerPackedScene.ResourcePath);
		ScannerRing _scannerInstance = _scannerToAdd.Instantiate() as ScannerRing;
		_spawnPoint.AddChild(_scannerInstance);
	}
}
