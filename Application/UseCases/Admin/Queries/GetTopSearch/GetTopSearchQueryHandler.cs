using Application.Abstractions.ElasticSearch;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using MediatR;
using Nest;

namespace Application.UseCases.Admin.Queries.GetTopSearch
{
    public class GetTopSearchQueryHandler : IRequestHandler<GetTopSearchQuery, APIResponse>
    {
        private readonly IElasticService<SearchHistory> _elasticSearchService;

        public GetTopSearchQueryHandler(IElasticService<SearchHistory> elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }
        public async Task<APIResponse> Handle(GetTopSearchQuery request, CancellationToken cancellationToken)
        {
            var response = await _elasticSearchService.SearchAsync(
            q => q.MatchAll(), 
            agg => agg
                .Terms("top_searches", t => t
                    .Field(f => f.EventName.Suffix("keyword"))
                    .Size(request.size))
                .Terms("top_hashtag", t => t
                    .Field(f => f.Hashtag.Suffix("keyword"))
                    .Size(request.size))
                .Terms("top_location", t => t
                    .Field(f => f.Location.Suffix("keyword"))
                    .Size(request.size))
        );

            if (!response.IsValid)
            {
                throw new Exception("Elasticsearch query failed");
            }

            // Tạo kết quả từ aggregations
            var topSearches = response.Aggregations.Terms("top_searches").Buckets.Select(b => new
            {
                Key = b.Key,
                Count = b.DocCount
            }).ToList();

            var topHashtags = response.Aggregations.Terms("top_hashtag").Buckets.Select(b => new 
            {
                Key = b.Key,
                Count = b.DocCount
            }).ToList();

            var topLocations = response.Aggregations.Terms("top_location").Buckets.Select(b => new 
            {
                Key = b.Key,
                Count = b.DocCount
            }).ToList();

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = new
                {
                    TopSearches = topSearches,
                    TopHashtags = topHashtags,
                    TopLocations = topLocations
                }
                
            };
        }
    }
}
