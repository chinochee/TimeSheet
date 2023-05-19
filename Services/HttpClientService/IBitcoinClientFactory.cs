namespace Services.BitcoinHttpClientService
{
    public interface IBitcoinClientFactory
    {
        IBitcoinHttpClient GetClient();
    }
}