using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWebRequestService
    {
        Task<string> CreateGetRequest(string url, string token = null);
        Task<string> CreatePostRequest(string url, object data, string token = null);
    }
}
