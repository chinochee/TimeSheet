using Data.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;
using Services.HttpClientService;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TimeSheetContext _context;
        private readonly TableSettings _tableSettings;
        private readonly IBitcoinClientFactory _clientFactory;
        private readonly ILogger<EmployeeService> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly IRoleService _roleService;

        public EmployeeService(ILogger<EmployeeService> logger, IOptions<TableSettings> config, TimeSheetContext context, IBitcoinClientFactory clientFactory, UserManager<Employee> userManager, IRoleService roleService)
        {
            _tableSettings = config.Value;
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
            _userManager = userManager;
            _roleService = roleService;
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

        public async Task<IdentityResult> Create(RegisterDataDto user)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var resultAdd = await _userManager.CreateAsync(new Employee
                {
                    Name = user.Name,
                    UserName = user.UserName
                }, user.Password);

                foreach (var error in resultAdd.Errors)
                    _logger.LogWarning($"{error.Description} ({error.Code})");

                var roles = await _roleService.GetRoles();

                var userCreated = await _userManager.FindByNameAsync(user.UserName);
                userCreated.RoleList = roles.Where(r => user.RoleIdList.Contains(r.Id)).Select(role => new Role
                {
                    Id = role.Id,
                    Name = role.Name,
                    EmployeeList = new List<Employee>{ userCreated }
                }).ToList();

                var resultUpd = await _userManager.UpdateAsync(userCreated);

                if (resultUpd.Succeeded)
                {
                    await transaction.CommitAsync();
                    return resultUpd;
                }

                foreach (var error in resultUpd.Errors)
                    _logger.LogWarning($"{error.Description} ({error.Code})");

                return resultUpd;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}