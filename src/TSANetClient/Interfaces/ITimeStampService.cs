namespace TSANetClient.Interfaces
{
    public interface ITimeStampService
    {
        TimeStampResult GenerateTimeStamp(string content);

        TimeStampResult VerifyTimeStamp(string content, string signature);
    }
}