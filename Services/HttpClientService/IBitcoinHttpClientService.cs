namespace Services.BitcoinHttpClientService
{
    public interface IBitcoinHttpClientService
    {
        IBitcoinHttpClient GetClient();
    }
}