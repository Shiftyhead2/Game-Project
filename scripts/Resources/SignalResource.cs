using Godot;

[GlobalClass, Icon("res://sprites/Planet.png")]
public partial class SignalResource : Resource
{
  [Export]
  public Texture2D SignalTexture { get; set; }

  [Export]
  public bool IsScannable { get; set; }
}
