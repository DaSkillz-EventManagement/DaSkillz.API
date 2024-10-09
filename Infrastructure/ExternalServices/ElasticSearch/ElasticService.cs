using Application.Abstractions.ElasticSearch;
using Nest;

namespace Infrastructure.ExternalServices.ElasticSearch
{
    public class ElasticService<TDomain> : IElasticService<TDomain> where TDomain : class
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName;

        public ElasticService(IElasticClient client)
        {
            _elasticClient = client;
            _indexName = typeof(TDomain).Name.ToLower();
        }

        public async Task<TDomain> GetAsync(string id)
        {
            var response = await _elasticClient.GetAsync(DocumentPath<TDomain>.Id(id).Index(_indexName));

            if (response.IsValid)
                return response.Source;

            //Log.Error(response.OriginalException, response.ServerError?.ToString());
            return null;
        }

        public async Task<TDomain> GetAsync(IGetRequest request)
        {
            var response = await _elasticClient.GetAsync<TDomain>(request);

            if (response.IsValid)
                return response.Source;

            //Log.Error(response.OriginalException, response.ServerError?.ToString());
            return null;
        }

        public async Task<TDomain> FindAsync(string id)
        {
            var response = await _elasticClient.GetAsync(DocumentPath<TDomain>.Id(id).Index(_indexName));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Source;
        }

        public async Task<TDomain> FindAsync(IGetRequest request)
        {
            var response = await _elasticClient.GetAsync<TDomain>(request);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Source;
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            var search = new SearchDescriptor<TDomain>(_indexName).MatchAll();
            var response = await _elasticClient.SearchAsync<TDomain>(search);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<IEnumerable<TDomain>> GetManyAsync(IEnumerable<string> ids)
        {
            var response = await _elasticClient.GetManyAsync<TDomain>(ids, _indexName);

            return response.Select(item => item.Source).ToList();
        }

        public async Task<IEnumerable<TDomain>> SearchAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> request)
        {
            var response = await _elasticClient.SearchAsync<TDomain>(s =>
                s.Index(_indexName)
                    .Query(request));
            //.Sort(a => a.Ascending(b => sort))

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<ISearchResponse<TDomain>> SearchAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> request,
            Func<AggregationContainerDescriptor<TDomain>, IAggregationContainer> aggregationsSelector)
        {
            var response = await _elasticClient.SearchAsync<TDomain>(s =>
                s.Index(_indexName)
                    .Query(request)
                    .Aggregations(aggregationsSelector));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response;
        }

        public async Task<IEnumerable<TDomain>> SearchAsync(Func<SearchDescriptor<TDomain>, ISearchRequest> selector)
        {
            var list = new List<TDomain>();
            var response = await _elasticClient.SearchAsync<TDomain>(s => selector(s.Index(_indexName)));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<bool> CreateIndexAsync()
        {
            if (!(await _elasticClient.Indices.ExistsAsync(_indexName)).Exists)
            {
                await _elasticClient.Indices.CreateAsync(_indexName, c =>
                {
                    c.Map<TDomain>(p => p.AutoMap());
                    return c;
                });
            }
            return true;
        }

        public async Task<bool> InsertAsync(TDomain model)
        {
            var response = await _elasticClient.IndexAsync(model, descriptor => descriptor.Index(_indexName));

            if (!response.IsValid)
                return false;

            return true;
        }

        public async Task<bool> InsertWithIdAsync(TDomain model)
        {
            var documentId = Guid.NewGuid().ToString();
            var response = await _elasticClient.IndexAsync(model, descriptor => descriptor.Index(_indexName).Id(documentId).Refresh(Elasticsearch.Net.Refresh.True));

            if (!response.IsValid)
                return false;

            return true;
        }

        public async Task<bool> InsertManyAsync(IList<TDomain> tList)
        {
            await CreateIndexAsync();
            var response = await _elasticClient.IndexManyAsync(tList, _indexName);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);


            return true;
        }

        public async Task<bool> UpdateAsync(TDomain model)
        {
            var response = await _elasticClient.UpdateAsync(DocumentPath<TDomain>.Id(model).Index(_indexName), p => p.Doc(model));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        //public async Task<bool> UpdatePartAsync(TDomain model, object partialEntity)
        //{
        //    var request = new UpdateRequest<TDomain, object>(_indexName, model.Id)
        //    {
        //        Doc = partialEntity
        //    };
        //    var response = await _elasticClient.UpdateAsync(request);

        //    if (!response.IsValid)
        //        throw new Exception(response.ServerError?.ToString(), response.OriginalException);

        //    return true;
        //}

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var response = await _elasticClient.DeleteAsync(DocumentPath<TDomain>.Id(id).Index(_indexName));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public async Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<TDomain>, QueryContainer> selector)
        {
            var response = await _elasticClient.DeleteByQueryAsync<TDomain>(q => q
                .Query(selector)
                .Index(_indexName)
            );

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public async Task<long> GetTotalCountAsync()
        {
            var search = new SearchDescriptor<TDomain>(_indexName).MatchAll();
            var response = await _elasticClient.SearchAsync<TDomain>(search);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Total;
        }

        public async Task<bool> ExistAsync(string id)
        {
            var response = await _elasticClient.DocumentExistsAsync(DocumentPath<TDomain>.Id(id).Index(_indexName));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Exists;
        }


    }
}
