// Файл: Services/DataService.cs

using System.Threading.Tasks;

namespace Presentation.Services
{
    public class DataService : IDataService
    {
        public Task<string> GetWelcomeMessage()
        {
            return Task.FromResult("Ласкаво просимо до GameOverDose!");
        }
    }
}