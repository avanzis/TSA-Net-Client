namespace TSANetClient.Providers
{
    public interface ITimeStampProviderClient
    {
        TimeStampProviderApiResult GenerateTimeStamp(string contentToStamp);

        TimeStampProviderApiResult VerifyTimeStamp(string contentToVerify, string signature);
    }
}