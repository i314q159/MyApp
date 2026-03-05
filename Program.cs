using Serilog;

using SharpDX.DirectInput;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log/log_.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            MonitorGamepadInput();
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // 获取手柄事件
    private static Guid MonitorGamepadInput()
    {
        // Initialize DirectInput
        var directInput = new DirectInput();

        // Find a Joystick Guid
        var joystickGuid = Guid.Empty;

        foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
        {
            joystickGuid = deviceInstance.InstanceGuid;
            Console.WriteLine("Found Gamepad with GUID: {0}", joystickGuid);
        }

        // If Gamepad not found, look for a Joystick
        if (joystickGuid == Guid.Empty)
        {
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
                Console.WriteLine("Found Joystick with GUID: {0}", joystickGuid);
            }
        }

        // If Joystick not found, throws an error
        if (joystickGuid == Guid.Empty)
        {
            Console.WriteLine("No joystick/Gamepad found.");
            Console.ReadKey();
            Environment.Exit(1);
            throw new InvalidOperationException(); // 告诉编译器此路不通
        }
        else
        {
            // Instantiate the joystick
            var joystick = new Joystick(directInput, joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
            {
                Console.WriteLine("Effect available {0}", effectInfo.Name);
            }
            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();

            // Poll events from joystick
            while (true)
            {
                joystick.Poll();
                var datas = joystick.GetBufferedData();
                foreach (var state in datas)
                {
                    // Console.WriteLine(state);
                    Log.Information(state.ToString());
                }
            }

            // 永远不会执行到这里，但编译器需要这一行
            throw new InvalidOperationException("Unexpected exit from infinite loop");
        }
    }
};
