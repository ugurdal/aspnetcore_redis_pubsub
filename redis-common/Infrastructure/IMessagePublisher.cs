using System.Threading.Tasks;
using redis_common.Models;

namespace redis_common.Infrastructure
{
    public interface IMessagePublisher
    {
        Task UserCreatedAsync(UserDto user);
    }
}