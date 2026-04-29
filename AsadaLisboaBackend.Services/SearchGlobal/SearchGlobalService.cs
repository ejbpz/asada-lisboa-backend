using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Models.DTOs.SearchGlobal;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;

namespace AsadaLisboaBackend.Services.SearchGlobal
{
    public class SearchGlobalService : ISearchGlobalService
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<SearchGlobalService> _logger;

        public SearchGlobalService(ElasticsearchClient client, ILogger<SearchGlobalService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<SearchGlobalResponseDTO>> Search(SearchGlobalRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return new List<SearchGlobalResponseDTO>();

            var from = (request.Page - 1) * request.PageSize;

            var response = await _client.SearchAsync<SearchGlobalDocument>(s => s
                .Indices(new[] { "contenido" })
                .From(from)
                .Size(request.PageSize)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(new[] { "title^3", "description^2" })
                        .Query(request.Query)
                        .Fuzziness("AUTO")
                    )
                ));


            if (!response.IsValidResponse)
            {
                _logger.LogError(response.DebugInformation);

                throw new ApplicationException("Busqueda fallida."); 
            }

            //mapping
            var results = response.Hits
                .Where(h => h.Source != null)
                .Select(hit => new SearchGlobalResponseDTO
            {
                Id = hit.Source.Id,
                Title = hit.Source.Title,
                Description = hit.Source.Description,
                Slug = hit.Source.Slug,
                Type = hit.Source.Type,
                Date = hit.Source.PublicationDate
            }).ToList();

            return results;
        }
    }
}

