using Application.ResponseMessage;
using Domain.DTOs.ParticipantDto;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Admin.Queries.GetTotalParticipant
{
    public class GetTotalParticipantByDateHandler : IRequestHandler<GetTotalParticipantByDateQuery, APIResponse>
    {
        private readonly IParticipantRepository _participantRepository;

        public GetTotalParticipantByDateHandler(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<APIResponse> Handle(GetTotalParticipantByDateQuery request, CancellationToken cancellationToken)
        {
            if (request.IsDay)
            {
                // Lấy dữ liệu tham gia theo ngày từ kho dữ liệu
                var participationByDay = await _participantRepository.GetParticipationByDayAsync(request.userId, request.eventId, request.StartDate, request.EndDate);

                // Tạo danh sách để lưu trữ dữ liệu tham gia theo ngày đầy đủ
                var dailyParticipation = new List<DailyParticipation>();

                // Tạo dải ngày từ startDate đến endDate
                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    // Tìm số lượng người tham gia trong ngày hiện tại
                    var participantCount = participationByDay.FirstOrDefault(p => p.Date == day.ToString("dd/MM/yyyy"))?.Count ?? 0;

                    dailyParticipation.Add(new DailyParticipation
                    {
                        Date = day.ToString("dd/MM/yyyy"),
                        Count = participantCount
                    });
                }

                // Tính tổng số lượt tham gia
                int totalParticipation = dailyParticipation.Sum(p => p.Count);

                // Tạo phản hồi API
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.OK,
                    Message = MessageCommon.GetSuccesfully,
                    Data = new
                    {
                        EventId = request.eventId,
                        TotalParticipation = totalParticipation,
                        DailyParticipation = dailyParticipation
                    }
                };
            }
            else
            {
                // Lấy dữ liệu tham gia theo giờ từ kho dữ liệu
                var participationByHour = await _participantRepository.GetParticipationByHourAsync(request.userId, request.eventId, request.StartDate, request.EndDate);

                // Tạo danh sách để lưu trữ dữ liệu tham gia theo giờ đầy đủ
                var hourlyParticipants = new List<HourlyPartitipant>();

                for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
                {
                    // Xác định giờ bắt đầu và giờ kết thúc cho từng ngày
                    int startHour = (day.Date == request.StartDate.Date) ? request.StartDate.Hour : 0;
                    int endHour = (day.Date == request.EndDate.Date) ? request.EndDate.Hour : 23; 

                    for (var hour = startHour; hour <= endHour; hour++) // Duyệt qua từng giờ trong ngày
                    {
                        var hourString = $"{hour:D2}:00"; // Định dạng giờ
                        var participantCount = participationByHour.FirstOrDefault(p => p.Hour == hourString)?.Count ?? 0;

                        hourlyParticipants.Add(new HourlyPartitipant
                        {
                            Date = day.ToString("dd/MM/yyyy"),
                            Hour = hourString,
                            Count = participantCount
                        });
                    }
                }

                // Tính tổng số lượt tham gia
                int totalParticipation = hourlyParticipants.Sum(p => p.Count);

                // Tạo phản hồi API
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.OK,
                    Message = MessageCommon.GetSuccesfully,
                    Data = new
                    {
                        EventId = request.eventId,
                        TotalParticipation = totalParticipation,
                        HourlyParticipants = hourlyParticipants
                    }
                };
            }


        }

        
    }

}
