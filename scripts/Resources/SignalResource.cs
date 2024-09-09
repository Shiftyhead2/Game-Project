using Godot;

[GlobalClass, Icon("res://sprites/Planet.png")]
public partial class SignalResource : Resource
{

  [Export]
  public Godot.Collections.Array<Vector2> SpawnPositions { get; set; }

  [Export]
  public Texture2D SignalTexture { get; set; }

  [Export]
  public bool IsScannable { get; set; }
}
