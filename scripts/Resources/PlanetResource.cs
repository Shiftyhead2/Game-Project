using Godot;

[GlobalClass, Icon("res://sprites/Planet.png")]
public partial class PlanetResource : Resource
{
  [Export]
  public string PlanetName { get; set; }

  [Export]
  public Vector2 SpawnPosition { get; set; }

  [Export]
  public Texture2D PlanetTexture { get; set; }

  public PlanetResource() : this(string.Empty, new Vector2(0, 0), null) { }

  public PlanetResource(string planetName, Vector2 spawnPosition, Texture2D planetTexture)
  {
    PlanetName = planetName;
    SpawnPosition = spawnPosition;
    PlanetTexture = planetTexture;
  }
}
