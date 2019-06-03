using OpenHardwareMonitor.Hardware;
using System;

namespace BtoFanControl
{
    internal class FanRegulator : IDisposable
    {
        private static class MagicConstants
        {
            public const int NR_MEASUREMENTS_FOR_AVERAGE_TEMP = 12;
            public const int NR_MEASUREMENTS_FOR_CURRENT_TEMP = 3;
            public const int INITIAL_FAN_PERCENTAGE = 50;
        }

        private readonly IFanControl fan;
        private readonly Computer computer;

        public FanRegulator(IFanControl fan)
        {
            this.fan = fan;
            this.computer = new Computer();
        }

        internal void Exec()
        {
            computer.Open();
            MonitorCPUTemperatureAndRegulateFan();
        }

        private void MonitorCPUTemperatureAndRegulateFan()
        {
            int fanSpeedAdjustmentsCounter = 0;
            var temps = new RunningAvg(MagicConstants.NR_MEASUREMENTS_FOR_AVERAGE_TEMP);
            int? currTemp;
            int fanPercentage = MagicConstants.INITIAL_FAN_PERCENTAGE;
            int prevFanPercentage = MagicConstants.INITIAL_FAN_PERCENTAGE;
            Console.CursorVisible = false;

            fan.SetFanSpeed(1, fanPercentage);

            var CPU = GetCPU();
            while (true)
            {
                CPU.Update();
                currTemp = GetCurrentTemperature(CPU);
                if (currTemp != null)
                {
                    temps.Add(currTemp.Value);
                    fanPercentage = CalcFanPercentage(fanPercentage, temps.GetAvg(MagicConstants.NR_MEASUREMENTS_FOR_CURRENT_TEMP), temps.GetAvg());
                    Console.Write($" => Fan% is now {fanPercentage}%    ");
                    Console.SetCursorPosition(0, 0);

                    if (fanPercentage != prevFanPercentage)
                    {
                        Console.Write($"\nLast updated fan speed at {DateTime.Now.ToLongTimeString()}. {++fanSpeedAdjustmentsCounter} updates so far.                                   ");
                        Console.SetCursorPosition(0, 0);
                        fan.SetFanSpeed(1, fanPercentage);
                        prevFanPercentage = fanPercentage;
                    }
                }
                System.Threading.Thread.Sleep(300);
            }
        }

        private static int CalcFanPercentage(int currentFanPercentage, int currentTemp, int averageTemp)
        {
            int fanPercShouldBe;
            if (currentTemp > 85 || averageTemp > 75) fanPercShouldBe = 70;
            else if (currentTemp > 67 || averageTemp > 58) fanPercShouldBe = 50;
            else if (currentTemp > 57 || averageTemp > 49) fanPercShouldBe = 30;
            else fanPercShouldBe = 22;

            Console.Write($"Average = {averageTemp}°C, Current = {currentTemp}°C, Fan% should be {fanPercShouldBe}%");

            if (fanPercShouldBe >= currentFanPercentage) return fanPercShouldBe;
            else if (averageTemp < 46) return 22;
            else if (averageTemp < 54) return 30;
            else if (averageTemp < 68) return 50;
            return currentFanPercentage;
        }

        private IHardware GetCPU()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            computer.CPUEnabled = true;
            computer.NICEnabled = false;
            computer.Accept(updateVisitor);

            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    return hardware;
                }
            }

            return null;
        }

        private static int? GetCurrentTemperature(IHardware CPUHardware)
        {
            foreach (ISensor sensor in CPUHardware.Sensors)
            {
                if (sensor.SensorType == SensorType.Temperature)
                    return (int?)sensor.Value;
            }

            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    computer.Close();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}