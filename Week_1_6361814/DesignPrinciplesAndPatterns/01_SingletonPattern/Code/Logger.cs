public class Logger
{
    private static Logger _instance;
    private static readonly object _lock = new object();

    private Logger()
    {
        Console.WriteLine("Logger Initialized.");
    }

    public static Logger GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new Logger();
            }
        }
        return _instance;
    }

    public void Log(string message)
    {
        Console.WriteLine($"Log: {message}");
    }
}
