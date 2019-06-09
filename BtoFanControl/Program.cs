namespace BtoFanControl
{
    partial class Program
    {
        static void Main(string[] args)
        {
            using(var regulator = new FanRegulator(new ClevoEcInfo(), new ConsoleLogger()))
            {
                regulator.Exec();
            }
        }
    }
}
