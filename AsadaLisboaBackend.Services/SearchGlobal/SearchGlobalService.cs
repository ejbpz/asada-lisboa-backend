using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Models.DTOs.SearchGlobal;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;

namespace AsadaLisboaBackend.Services.SearchGlobal
{
    public class SearchGlobalService : ISearchGlobalService
    {
        private readonly ElasticsearchClient _client;

        public SearchGlobalService(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<List<SearchGlobalDocument>> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<SearchGlobalDocument>(s => s
                .Indices(new[] { "Documento", "Imagen", "Noticias" })
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Fields(new[] { "title^3", "description^2" })
                        .Query(query)
                        .Fuzziness("AUTO")
                    )
                ));

            return response.Documents.ToList();
        }
    }
}
