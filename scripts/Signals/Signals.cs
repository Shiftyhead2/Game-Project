using Godot;

public partial class Signals : StaticBody2D
{
	[Export]
	private SignalResource _planetResource;

	[Export]
	private Sprite2D _sprite;

	[Export]
	private bool _detected = false;


	public void SetUpPlanet(SignalResource planetResource, Vector2 spawnPosition)
	{
		_planetResource = planetResource;
		if (_planetResource != null)
		{
			_sprite.Texture = _planetResource.SignalTexture;
		}
		Position = spawnPosition;
	}

	public void GetDetected()
	{
		_sprite.Visible = true;
		_detected = true;
	}

	public bool IsPlanetScannable()
	{
		return _planetResource.IsScannable;
	}

	public bool IsDetected()
	{
		return _detected;
	}
}
