using Nest;

namespace Application.Abstractions.ElasticSearch
{
    public interface IElasticService<TDomain> where TDomain : class
    {
        Task<TDomain> GetAsync(string id);
        Task<TDomain> GetAsync(IGetRequest request);
        Task<TDomain> FindAsync(string id);
        Task<TDomain> FindAsync(IGetRequest request);
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task<IEnumerable<TDomain>> GetManyAsync(IEnumerable<string> ids);
        Task<IEnumerable<TDomain>> SearchAsync(Func<SearchDescriptor<TDomain>, ISearchRequest> selector);
        Task<IEnumerable<TDomain>> SearchAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> request);
        Task<ISearchResponse<TDomain>> SearchAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> request, Func<AggregationContainerDescriptor<TDomain>, IAggregationContainer> aggregationsSelector);
        Task<bool> CreateIndexAsync();
        Task<bool> InsertAsync(TDomain t);
        Task<bool> InsertWithIdAsync(TDomain model);
        Task<bool> InsertManyAsync(IList<TDomain> tList);
        Task<bool> UpdateAsync(TDomain t);
        Task<long> GetTotalCountAsync();
        Task<bool> DeleteByIdAsync(string id);
        Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> selector);
        Task<bool> ExistAsync(string id);
    }
}

