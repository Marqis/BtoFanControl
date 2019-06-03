namespace BtoFanControl
{
    public interface IFanControl
    {
        ECData2 GetECData(int fanNr);
        void SetFanSpeed(int fanNr, int fanSpeedPercentage);
    }
}