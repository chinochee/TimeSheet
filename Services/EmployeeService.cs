using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.BitcoinHttpClientService;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TimeSheetContext _context;
        private readonly TableSettings _tableSettings;
        private readonly IBitcoinClientFactory _clientFactory;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IOptions<TableSettings> config, TimeSheetContext context, IBitcoinClientFactory clientFactory)
        {
            _tableSettings = config.Value;
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<EmployeeEntryDto[]> Get()
        {
            try
            {
                var result = await _context.Users
                    .Select(e => new EmployeeEntryDto { Id = e.Id, Name = e.Name }).ToArrayAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        public async Task<EmployeeEntryBTCDto[]> GetTopLastYearTimeSheet()
        {
            try
            {
                var coinDeskTask = _clientFactory.GetClient().GetRates();
                var topEmployeesTask = GetAnnualTopUSD();

                await Task.WhenAll(coinDeskTask, topEmployeesTask);

                var coinDesk = await coinDeskTask;
                var topEmployees = await topEmployeesTask;

                return topEmployees.Select(e => new EmployeeEntryBTCDto(e, coinDesk.Rate)).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        private async Task<EmployeeEntryDto[]> GetAnnualTopUSD()
        {
            try
            {
                _logger.LogInformation("Get top employees from db");

                var result = await _context.Users.Select(e => new EmployeeEntryDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    TotalPriceUSD = e.TimeSheetList.Where(t => t.DateOfWorks >= DateTime.UtcNow.AddYears(-1)).Sum(
                        timeSheet =>
                            timeSheet.WorkHours * timeSheet.Scope.Rate * timeSheet.Scope.Currency.DollarExchangeRate ?? 0)
                }).OrderByDescending(s => s.TotalPriceUSD).Take(_tableSettings.TopEmployees).ToArrayAsync();

                _logger.LogInformation("Get top employees from db Finished");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}