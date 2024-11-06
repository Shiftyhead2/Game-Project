using Godot;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Sensitivity variables")]
	[Export] private float _lookSensitivity = 0.006f;
	[Export] private float _controllerLookSensitivity = 0.05f;
	
	[ExportGroup("Movement variables")]
	[Export] private float _jumpVelocity = 6.0f;
	[Export] private float _walkSpeed = 7.0f;
	[Export] private float _sprintSpeed = 8.5f;
	
	[ExportGroup("Node variables")]
	[Export] private Node3D _cameraNode;
	
	[ExportGroup("Air control variables")]
	[Export] private float _airCap = 0.85f;
	[Export] private float _airAcceleration = 800.0f;
	[Export] private float _airMoveSpeed = 500.0f;

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
			if (GamepadInputManager.Instance?.GetControllerStatus() == true)
			{
				return;
			}
			
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
		HandleControllerLookInput((float)delta);
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

		float currentSpeedInWishedDirection = _velocity.Dot(_wishDir);

		float cappedSpeed = Mathf.Min((_airMoveSpeed * _wishDir).Length(), _airCap);
		float addSpeedTillCap = cappedSpeed - currentSpeedInWishedDirection;

		if (addSpeedTillCap > 0)
		{
			float accelerationSpeed = _airAcceleration * _airMoveSpeed * delta;
			accelerationSpeed = Mathf.Min(accelerationSpeed, addSpeedTillCap);
			_velocity += accelerationSpeed * _wishDir;
		}
	}

	private void HandleGroundPhysics(float delta)
	{
		_velocity.X = _wishDir.X * GetMoveSpeed();
		_velocity.Z = _wishDir.Z * GetMoveSpeed();
		
		HeadbobEffect(delta);
	}

	private void HandleControllerLookInput(float delta)
	{
		if (GamepadInputManager.Instance?.GetControllerStatus() == false)
		{
			return;
		}
		
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
