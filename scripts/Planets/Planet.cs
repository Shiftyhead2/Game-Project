using Godot;

public partial class Planet : StaticBody2D
{
	[Export]
	private PlanetResource _planetResource;

	[Export]
	private Sprite2D _sprite;

	[Export]
	private bool _detected = false;


	public void SetUpPlanet(PlanetResource planetResource)
	{
		_planetResource = planetResource;
		if (_planetResource != null)
		{
			Name = _planetResource.PlanetName;
			_sprite.Texture = _planetResource.PlanetTexture;
			if (!_planetResource.IsEarth)
			{
				_sprite.Visible = false;
			}
			Position = _planetResource.SpawnPosition;
		}
	}

	public void GetDetected()
	{
		if (_planetResource.IsEarth || _detected)
		{
			return;
		}
		_sprite.Visible = true;
		_detected = true;
	}

	public bool IsPlanetEarth()
	{
		return _planetResource.IsEarth;
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
