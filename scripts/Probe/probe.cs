using Godot;

public partial class Probe : CharacterBody2D
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
	private PackedScene _detectorPackedScene;

	[Export]
	private Area2D _scannerArea;

	[Export]
	private Planet _scannablePlanetInRange;


	public override void _Ready()
	{
		if (_scannerArea != null)
		{
			_scannerArea.BodyEntered += OnBody2DEntered;
			_scannerArea.BodyExited += OnBody2DExited;
		}
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("scan"))
		{
			if (_scannablePlanetInRange == null)
			{
				SpawnScannerRing();
			}
			else
			{
				ScanPlanet();
			}
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
		DetectionRing _scannerToAdd = ResourceLoader.Load<PackedScene>(_detectorPackedScene.ResourcePath).Instantiate() as DetectionRing;
		_spawnPoint.AddChild(_scannerToAdd);
	}

	private void ScanPlanet()
	{
		EventManager.ExecuteOnScanPlanet(_scannablePlanetInRange);
	}

	private void OnBody2DEntered(Node2D body)
	{
		if (_scannablePlanetInRange != null)
		{
			return;
		}

		if (body is Planet)
		{
			Planet collidedPlanet = body as Planet;
			if (collidedPlanet.IsPlanetScannable() && collidedPlanet.IsDetected())
			{
				_scannablePlanetInRange = collidedPlanet;
			}
		}

	}

	private void OnBody2DExited(Node2D body)
	{
		if (_scannablePlanetInRange == null)
		{
			return;
		}

		if (body is Planet)
		{
			_scannablePlanetInRange = null;
		}
	}
}
