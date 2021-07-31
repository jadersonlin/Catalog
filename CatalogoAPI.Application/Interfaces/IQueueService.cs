using Catalog.Application.Dtos;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces
{
    public interface IQueueService
    {
        Task<string> SendMessage(string message);

        Task<Message> DequeueMessage();

        Task DeleteMessage(Message message);
    }
}
