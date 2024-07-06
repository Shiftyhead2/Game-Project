using Godot;

public partial class Planet : StaticBody2D
{
	[Export]
	private PlanetResource _planetResource;

	[Export]
	private Sprite2D _sprite;


	public void SetUpPlanet(PlanetResource planetResource)
	{
		_planetResource = planetResource;
		if (_planetResource != null)
		{
			Name = _planetResource.PlanetName;
			_sprite.Texture = _planetResource.PlanetTexture;
			_sprite.Visible = false;
			Position = _planetResource.SpawnPosition;
		}
	}

	public void GetDetected()
	{
		_sprite.Visible = true;
	}


}
