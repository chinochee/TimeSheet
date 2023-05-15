namespace Services
{
    public interface IBitcoinHttpClient
    {
        Task<CoinDesk> GetRates();
    }
}