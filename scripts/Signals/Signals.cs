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

	[Export]
	private Color _defaultNotScannedColor;

	[Export]
	private Color _inScanRangeColor;

	[Export]
	private Color _scannedColor;

	[Export]
	private bool _isInScanRange = false;
	[Export]
	private bool _isScanned = false;

	public override void _Ready()
	{
		_sprite.Visible = false;
	}


	public override void _EnterTree()
	{
		EventManager.OnScanSignal += SetScanned;
	}

	public override void _ExitTree()
	{
		EventManager.OnScanSignal -= SetScanned;
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
		_sprite.Modulate = _defaultNotScannedColor;
		_detected = true;
		_isScanned = false;
		_isInScanRange = false;
	}

	public bool IsSignalScannable()
	{
		return _signalResource.IsScannable;
	}

	public bool IsDetected()
	{
		return _detected;
	}

	public void SetIsInScanRange(bool isInScanRange)
	{
		if (_isScanned)
		{
			return;
		}

		_isInScanRange = isInScanRange;

		if (_isInScanRange)
		{
			_sprite.Modulate = _inScanRangeColor;
		}
		else
		{
			_sprite.Modulate = _defaultNotScannedColor;
		}
	}


	private void SetScanned(Signals signal)
	{
		Rid passedSignalRid = signal.GetRid();
		Rid currentSignalRid = GetRid();

		if (passedSignalRid != currentSignalRid)
		{
			Logger.LogError(Name,$"Scanned signal Rid: {passedSignalRid} is not the same as Rid: {currentSignalRid}");
		}

		_isScanned = true;

		_sprite.Modulate = _scannedColor;
	}
}
