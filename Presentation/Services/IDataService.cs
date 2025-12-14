// Файл: Services/IDataService.cs

using System.Threading.Tasks;

namespace Presentation.Services
{
	public interface IDataService
	{
		// Додайте тут методи, які потрібні для завантаження даних
		Task<string> GetWelcomeMessage();
		// Наприклад, Task<List<Game>> GetFeaturedGames();
	}
}