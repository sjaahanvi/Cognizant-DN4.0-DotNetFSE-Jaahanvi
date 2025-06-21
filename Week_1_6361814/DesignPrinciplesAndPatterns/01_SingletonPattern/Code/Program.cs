class Program
{
    static void Main(string[] args)
    {
        var logger1 = Logger.GetInstance();
        var logger2 = Logger.GetInstance();

        logger1.Log("Hello from logger1");
        logger2.Log("Hello from logger2");

        Console.WriteLine($"Are logger1 and logger2 same? {object.ReferenceEquals(logger1, logger2)}");
    }
}
