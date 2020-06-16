using EPiServer.Data;
using EPiServer.Data.Dynamic;
using PulsePersonalizationApp.Interfaces;

namespace PulsePersonalizationApp.Models
{
    [EPiServerDataStore(AutomaticallyRemapStore = true, AutomaticallyCreateStore = true)]
    public class PulseAdminModel : IDataStoreModelInterface
    {
        public Identity Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PulseApiKey { get; set; }

    }
}