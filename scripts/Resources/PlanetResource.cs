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

  [Export]
  public bool IsEarth { get; set; }

  [Export]
  public bool IsScannable { get; set; }

  public PlanetResource() : this(string.Empty, new Vector2(0, 0), null, false, false) { }

  public PlanetResource(string planetName, Vector2 spawnPosition, Texture2D planetTexture, bool isEarth, bool isScannable)
  {
    PlanetName = planetName;
    SpawnPosition = spawnPosition;
    PlanetTexture = planetTexture;
    IsEarth = isEarth;
    IsScannable = isScannable;
  }
}
