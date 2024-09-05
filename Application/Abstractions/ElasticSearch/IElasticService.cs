using Elastic.Clients.Elasticsearch;

namespace Application.Abstractions.ElasticSearch
{
    public interface IElasticService<TDomain> where TDomain : class
    {
        Task IndexDocumentAsync(TDomain customer);
        Task BulkIndexDocumentsAsync(IEnumerable<TDomain> customer/*, string indexName*/);
        Task<TDomain> Get(int key);
        Task<IEnumerable<TDomain>?> GetAll();
        Task Remove(int key);
        Task RemoveWithQuery(DeleteByQueryRequestDescriptor<TDomain> descriptor);
        Task Update(TDomain domain, int id);
        Task UpdateWithQuery(UpdateByQueryRequestDescriptor<TDomain> descriptor);
        Task<IEnumerable<TDomain>> FilterAsync(SearchRequestDescriptor<TDomain> descriptor);
        Task IndexDocumentWithKeywordAsync(TDomain domain, int keyword);
    }
}

