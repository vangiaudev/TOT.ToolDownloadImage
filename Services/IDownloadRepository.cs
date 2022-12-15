using TOT.ToolDownloadImage.Models;

namespace TOT.ToolDownloadImage.Services
{
    public interface IDownloadRepository
    {
        Task AddAsync(Image image);
        Task<Stream?> GetUrlContent(string url);
    }
}
