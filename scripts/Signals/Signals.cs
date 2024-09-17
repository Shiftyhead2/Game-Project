using Godot;

public partial class Signals : StaticBody2D
{
	[Export]
	private SignalResource _signalResource;

	[Export]
	private Sprite2D _sprite;

	[Export]
	private bool _detected = false;

	private double _detectionTime = 10f;

	public override void _Ready()
	{
		_sprite.Visible = false;
	}


	public void SetUpSignal(SignalResource signalResource, Vector2I spawnPosition)
	{
		_signalResource = signalResource;
		if (_signalResource != null)
		{
			_sprite.Texture = _signalResource.SignalTexture;
		}
		Position = spawnPosition;
		_sprite.Visible = true;
		_detected = true;
	}

	public bool IsSignalScannable()
	{
		return _signalResource.IsScannable;
	}

	public bool IsDetected()
	{
		return _detected;
	}
}
