using Microsoft.Extensions.Options;

namespace Services.Configuration
{
    public class TableSettingsModel
    {
        private readonly TableSettings _settings;
        public TableSettingsModel(IOptions<TableSettings> settings)
        {
            _settings = settings.Value;
        }
    }
}
