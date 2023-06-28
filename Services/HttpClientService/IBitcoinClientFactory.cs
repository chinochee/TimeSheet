namespace Services.HttpClientService
{
    public interface IBitcoinClientFactory
    {
        IBitcoinHttpClient GetClient();
    }
}