using Godot;

public partial class probe : CharacterBody2D
{
	[Export]
	private float _speed = 300.0f;
	[Export]
	private float _acceleration = 10.0f;

	[Export]
	private float _deacceleration = 5.0f;


	private Vector2 velocity;
	private Vector2 direction;


	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(velocity.X, _speed * direction.X, _acceleration);
			velocity.Y = Mathf.MoveToward(velocity.Y, _speed * direction.Y, _acceleration);
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0f, _deacceleration);
			velocity.Y = Mathf.MoveToward(velocity.Y, 0f, _deacceleration);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
