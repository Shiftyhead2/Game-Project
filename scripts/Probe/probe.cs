using Godot;

public partial class Probe : CharacterBody2D
{
	[Export]
	private float _speed = 300.0f;
	[Export]
	private float _acceleration = 10.0f;

	[Export]
	private float _deacceleration = 5.0f;

	[Export]
	private double _timeBetweenScans = 20.0f;

	[Export]
	private bool _canDetect = true;

	[Export]
	private Timer _timer;


	private Vector2 _velocity;
	private Vector2 _direction;

	[Export]
	private Marker2D _spawnPoint;

	[Export]
	private PackedScene _detectorPackedScene;

	[Export]
	private Area2D _scannerArea;

	[Export]
	private Signals _scannableSignalsInRange;

	private DetectionRing _detectionRing;




	public override void _Ready()
	{
		if (_scannerArea != null)
		{
			_scannerArea.BodyEntered += OnBody2DEntered;
			_scannerArea.BodyExited += OnBody2DExited;
		}

		_timer.Timeout += ResetDetect;
		GameManager.instance.SetProbe(this);
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("scan"))
		{
			if (_scannableSignalsInRange == null)
			{
				if (_canDetect)
				{
					SpawnScannerRing();
				}
			}
			else
			{
				ScanSignal();
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
		if (_detectionRing == null)
		{
			DetectionRing _detectionRingToAdd = ResourceLoader.Load<PackedScene>(_detectorPackedScene.ResourcePath).Instantiate() as DetectionRing;
			_spawnPoint.AddChild(_detectionRingToAdd);
			_detectionRing = _detectionRingToAdd;
			_timer.Start(_timeBetweenScans);
			_canDetect = false;
		}
		else
		{
			_detectionRing.OnScan();
			_timer.Start(_timeBetweenScans);
			_canDetect = false;
		}
		EventManager.ExecuteOnDetectSignals();
	}

	private void ScanSignal()
	{
		EventManager.ExecuteOnScanPlanet(_scannableSignalsInRange);
	}

	private void OnBody2DEntered(Node2D body)
	{
		if (_scannableSignalsInRange != null)
		{
			return;
		}

		if (body is Signals)
		{
			Signals collidedSignal = body as Signals;
			if (collidedSignal.IsSignalScannable() && collidedSignal.IsDetected())
			{
				_scannableSignalsInRange = collidedSignal;
			}
		}

	}

	private void OnBody2DExited(Node2D body)
	{
		if (_scannableSignalsInRange == null)
		{
			return;
		}

		if (body is Signals)
		{
			_scannableSignalsInRange = null;
		}
	}

	private void ResetDetect()
	{
		_canDetect = true;
		_timer.Stop();
	}
}
