using Godot;


public partial class GamepadInputManager: Node
{
    public static GamepadInputManager Instance { get; private set; }

    private bool _isControllerConnected;

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        
        Instance = this;

        if (Input.GetConnectedJoypads().Count != 0)
        {
            Logger.LogMessage(Name,"Gamepad detected!");
            _isControllerConnected = true;
        }
        else
        {
            Logger.LogMessage(Name,"Gamepad not detected!");
            _isControllerConnected = false;
        }
    }

    public override void _EnterTree()
    {
        Input.JoyConnectionChanged += OnJoyConnectionChanged;
    }


    public override void _ExitTree()
    {
        Input.JoyConnectionChanged -= OnJoyConnectionChanged;
        
    }

    private void OnJoyConnectionChanged(long deviceId, bool connected)
    {
        if (connected)
        {
            if (_isControllerConnected)
            {
                Logger.LogMessage(Name,"Gamepad has already been detected! Exiting function!");
                return;
            }
            Logger.LogMessage(Name,$"Gamepad connected: {Input.GetJoyName((int)deviceId)}");
            _isControllerConnected = true;
        }
        else
        {
            Logger.LogMessage(Name,$"Gamepad disconnected. Keyboard and mouse controls enabled");
            _isControllerConnected = false;
        }
    }

    public bool GetControllerStatus()
    {
        return _isControllerConnected;
    }
}