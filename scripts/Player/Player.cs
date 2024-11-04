using Godot;

public partial class Player : CharacterBody3D
{
	[Export] private float _lookSensitivity = 0.006f;
	[Export] private float _controllerLookSensitivity = 0.05f;
	[Export] private float _jumpVelocity = 6.0f;
	[Export] private float _walkSpeed = 7.0f;
	[Export] private float _sprintSpeed = 8.5f;
	[Export] private Node3D _cameraNode;

	private float _headbobMoveAmount = 0.06f;
	private float _headbobFrequency = 2.4f;
	private float _headbobTime = 0.0f;

	private Vector3 _wishDir = Vector3.Zero;
	private Vector3 _velocity;
	private Vector2 _controllerLook;
	private Vector2 _inputDir;
	private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
	


	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}

	


	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
		else if (@event.IsActionPressed("ui_cancel"))
		{
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}

		if (Input.GetMouseMode() == Input.MouseModeEnum.Captured)
		{
			if (@event is InputEventMouseMotion motion)
			{
				RotateY(-motion.Relative.X * _lookSensitivity);
				_cameraNode.RotateX(-motion.Relative.Y * _lookSensitivity);
				_cameraNode.Rotation = _cameraNode.Rotation with {X = Mathf.Clamp(_cameraNode.Rotation.X,Mathf.DegToRad(-90f),Mathf.DegToRad(90f))};
			}
		}
	}
	
	public override void _Process(double delta)
	{
		
	}


	public override void _PhysicsProcess(double delta)
	{
		_inputDir = Input.GetVector("move_left","move_right","move_up","move_down").Normalized();
		_wishDir = GlobalTransform.Basis * new Vector3(_inputDir.X,0,_inputDir.Y);
		HandleControllerLookInput((float)delta);


		if (IsOnFloor())
		{
			if (Input.IsActionPressed("detect"))
			{
				_velocity.Y = _jumpVelocity;
			}
			HandleGroundPhysics((float)delta);
		}
		else
		{
			HandleAirPhysics((float)delta);
		}

		Velocity = _velocity;
		MoveAndSlide();

	}

	private void HeadbobEffect(float delta)
	{
		_headbobTime += delta * Velocity.Length();
		_cameraNode.Transform = _cameraNode.Transform with { Origin = new Vector3(
			Mathf.Cos(_headbobTime * _headbobFrequency * 0.5f) * _headbobMoveAmount,
			Mathf.Sin(_headbobTime * _headbobFrequency) * _headbobMoveAmount,
			0f
			) };
	}

	private void HandleAirPhysics(float delta)
	{
		_velocity.Y -= _gravity * delta;
	}

	private void HandleGroundPhysics(float delta)
	{
		_velocity.X = _wishDir.X * GetMoveSpeed();
		_velocity.Z = _wishDir.Z * GetMoveSpeed();
		
		HeadbobEffect(delta);
	}

	private void HandleControllerLookInput(float delta)
	{
		Vector2 targetLook = Input.GetVector("look_left", "look_right", "look_down", "look_up").Normalized();
		

		if (targetLook.Length() < _controllerLook.Length())
		{
			_controllerLook = targetLook;
		}
		else
		{
			_controllerLook = _controllerLook.Lerp(targetLook, 5.0f * delta);
		}
		
		RotateY(-_controllerLook.X * _controllerLookSensitivity);
		_cameraNode.RotateX(_controllerLook.Y * _controllerLookSensitivity);
		_cameraNode.Rotation = _cameraNode.Rotation with {X = Mathf.Clamp(_cameraNode.Rotation.X,Mathf.DegToRad(-90f),Mathf.DegToRad(90f))};
	}

	private float GetMoveSpeed()
	{
		return Input.IsActionPressed("sprint") ? _sprintSpeed : _walkSpeed;
	}
}
