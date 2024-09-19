using Application.Abstractions.Caching;
using Application.UseCases.Events.Queries.GetEventByTag;
using Domain.DTOs.Events;
using Domain.DTOs.User.Response;
using Domain.Repositories;
using System.Text;

namespace Application.Helper
{
    public static class EventHelper
    {
       

        

        public static string GenerateCacheKeyFilteredEvent(EventFilterObjectDto filter, int pageNo, int eachPage)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"FilteredEvents_Page_{pageNo}_Size_{eachPage}");

            if (filter.TagId != null && filter.TagId.Any())
                keyBuilder.Append($"_TagIds_{string.Join("-", filter.TagId)}");

            if (!string.IsNullOrEmpty(filter.UserCoord))
                keyBuilder.Append($"_UserCoord_{filter.UserCoord}");

            if (!string.IsNullOrEmpty(filter.EventName))
                keyBuilder.Append($"_EventName_{filter.EventName}");

            if (filter.Status != null && filter.Status.Any())
                keyBuilder.Append($"_Status_{string.Join("-", filter.Status)}");

            if (filter.StartDateFrom.HasValue)
                keyBuilder.Append($"_StartDateFrom_{filter.StartDateFrom.Value}");

            if (filter.StartDateTo.HasValue)
                keyBuilder.Append($"_StartDateTo_{filter.StartDateTo.Value}");

            if (filter.EndDateFrom.HasValue)
                keyBuilder.Append($"_EndDateFrom_{filter.EndDateFrom.Value}");

            if (filter.EndDateTo.HasValue)
                keyBuilder.Append($"_EndDateTo_{filter.EndDateTo.Value}");

            if (!string.IsNullOrEmpty(filter.Location))
                keyBuilder.Append($"_Location_{filter.Location}");

            if (!string.IsNullOrEmpty(filter.LocationId))
                keyBuilder.Append($"_LocationId_{filter.LocationId}");

            if (filter.Approval.HasValue)
                keyBuilder.Append($"_Approval_{filter.Approval.Value}");

            if (filter.TicketFrom.HasValue)
                keyBuilder.Append($"_TicketFrom_{filter.TicketFrom.Value}");

            if (filter.TicketTo.HasValue)
                keyBuilder.Append($"_TicketTo_{filter.TicketTo.Value}");

            if (!string.IsNullOrEmpty(filter.SortBy))
                keyBuilder.Append($"_SortBy_{filter.SortBy}");

            keyBuilder.Append($"_IsAscending_{filter.IsAscending}");

            return keyBuilder.ToString();
        }

        public static string GenerateCacheKeyByTag(GetEventByTagQuery request)
        {
            var tagIds = string.Join("_", request.TagIds);
            return $"Events_by_tags_{tagIds}_page_{request.PageNo}_size_{request.ElementEachPage}";
        }

        //public static async Task InvalidateEventCacheAsync()
        //{
        //    string getEventByTagPattern = $"Events_by_tags_*";
        //    await _redisCaching.InvalidateCacheByPattern(getEventByTagPattern);

        //    string getEventByUserRolePattern = $"GetEventByUserRole_*";
        //    await _redisCaching.InvalidateCacheByPattern(getEventByUserRolePattern);

        //    string getEventParticipatedByUserPattern = $"GetEventParticipatedByUser_*";
        //    await _redisCaching.InvalidateCacheByPattern(getEventParticipatedByUserPattern);

        //    string getFilteredEventPattern = $"FilteredEvents_*";
        //    await _redisCaching.InvalidateCacheByPattern(getFilteredEventPattern);


        //}

        

    }
}
