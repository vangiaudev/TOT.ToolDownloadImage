using TOT.ToolDownloadImage.DataAccess;
using TOT.ToolDownloadImage.Models;

namespace TOT.ToolDownloadImage.Services.Implementation
{
    public class DownloadRepositoryService : IDownloadRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        public DownloadRepositoryService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context; 
        }

        public async Task AddAsync(Image image)
        {
            await _context.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Stream?> GetUrlContent(string url)
        {
            var result = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return result.IsSuccessStatusCode ? await result.Content.ReadAsStreamAsync() : null;
        }
    }
}
