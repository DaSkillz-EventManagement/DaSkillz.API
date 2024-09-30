using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Application.ResponseMessage;
using Application.UseCases.Payment.Commands.CreatePayment;
using Domain.Entities;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch.Security;
using Medallion.Threading;
using Moq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;


namespace Daskillz.Testing.Payment
{
    public class PaymentTesting
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IParticipantRepository> _participantRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IDistributedLockProvider> _synchronizationProviderMock;
        private readonly Mock<IZaloPayService> _zaloPayServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRedisCaching> _cachingMock;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _httpClient;
        public PaymentTesting(ITestOutputHelper output)
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _participantRepositoryMock = new Mock<IParticipantRepository>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _synchronizationProviderMock = new Mock<IDistributedLockProvider>();
            _zaloPayServiceMock = new Mock<IZaloPayService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cachingMock = new Mock<IRedisCaching>();
            _output = output;
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task Handle_CreatePayment_WhenTwoRequest_RunConcurrently()
        {
            // Arrange
            var handler = new CreatePaymentHandler(
                _transactionRepositoryMock.Object,
                _userRepositoryMock.Object,
                _participantRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _synchronizationProviderMock.Object,
                _zaloPayServiceMock.Object,
                _unitOfWorkMock.Object,
                _cachingMock.Object
            );

            var request = new CreatePayment
            {
                UserId = Guid.Parse("a4593c8c-6f8c-4244-960e-fc304a5dfe4e"),
                Amount = "10000",
                Description = "Test payment",
                EventId = Guid.Parse("aa00c37f-b734-447b-8c81-e70822c73104"),
                SubscriptionType = (int)PaymentType.TICKET,
                redirectUrl = "https://www.youtube.com/watch?v=DkTP-uGAzBw"
            };

            var lockHandleMock1 = new Mock<IDistributedSynchronizationHandle>();
            var lockHandleMock2 = new Mock<IDistributedSynchronizationHandle>();

            _synchronizationProviderMock
                .SetupSequence(p => p.CreateLock(It.IsAny<string>()).TryAcquireAsync(It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lockHandleMock1.Object) // First request acquires lock
                .ReturnsAsync(lockHandleMock2.Object);

            // Mocking the event capacity check to return true (capacity full) for the second request
            _participantRepositoryMock
                .SetupSequence(repo => repo.IsReachedCapacity(It.IsAny<Guid>()))
                .ReturnsAsync(false)  
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new Domain.Entities.User());

            var zaloPayResponse = new Dictionary<string, object>
                    {
                        { "return_code", 1L },
                        { "order_url", "https://www.example.com/payment" }
                    };
            _zaloPayServiceMock
                .Setup(service => service.CreateOrderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(zaloPayResponse);

            _eventRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Event());

            _transactionRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Transaction>()));

           

            var tasks = new List<Task<APIResponse>>
                {
                    handler.Handle(request, CancellationToken.None), // First request
                    handler.Handle(request, CancellationToken.None)  // Second request
                };

            var results = await Task.WhenAll(tasks);

            // Assert
            _output.WriteLine($"First Request Result: {results[0].Message}");
            _output.WriteLine($"Second Request Result: {results[1].Message}");

            Assert.Equal(HttpStatusCode.OK, results[0].StatusResponse);
            Assert.Equal(HttpStatusCode.Conflict, results[1].StatusResponse);
        }

        [Fact]
        public async Task Handle_CreatePayment_WhenTwoRequestNotAccquireLock()
        {
            // Arrange
            var handler = new CreatePaymentHandler(
                _transactionRepositoryMock.Object,
                _userRepositoryMock.Object,
                _participantRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _synchronizationProviderMock.Object,
                _zaloPayServiceMock.Object,
                _unitOfWorkMock.Object,
                _cachingMock.Object
            );

            var request = new CreatePayment
            {
                UserId = Guid.Parse("a4593c8c-6f8c-4244-960e-fc304a5dfe4e"),
                Amount = "10000",
                Description = "Test payment",
                EventId = Guid.Parse("a4593c8c-6f8c-4244-960e-fc304a5dfe4e"),
                SubscriptionType = (int)PaymentType.TICKET,
                redirectUrl = "https://www.youtube.com/watch?v=DkTP-uGAzBw"
            };

            // Mock lock handles
            var lockHandleMock = new Mock<IDistributedSynchronizationHandle>();
            _synchronizationProviderMock
                .Setup(p => p.CreateLock(It.IsAny<string>()).TryAcquireAsync(It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lockHandleMock.Object);

            // Mocking the event capacity check to return true (capacity full) for the second request
            _participantRepositoryMock
                .SetupSequence(repo => repo.IsReachedCapacity(It.IsAny<Guid>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new Domain.Entities.User());

            var zaloPayResponse = new Dictionary<string, object>
                    {
                        { "return_code", 1L },
                        { "order_url", "https://www.example.com/payment" }
                    };
            _zaloPayServiceMock
                .Setup(service => service.CreateOrderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(zaloPayResponse);

            _eventRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Event());
            _transactionRepositoryMock
                .Setup(repo => repo.GetAlreadyPaid(request.UserId, (Guid)request.EventId));
            _transactionRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Transaction>()));


            //ACT
            var task1 = await handler.Handle(request, CancellationToken.None);

            _transactionRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Transaction>()));
            var task2 = await handler.Handle(request, CancellationToken.None);

            //var results = await Task.WhenAll(task1, task2);

            // Assert
            _output.WriteLine($"First Request Result: {task1.Message}");
            _output.WriteLine($"Second Request Result: {task2.Message}");

            Assert.Equal(HttpStatusCode.OK, task1.StatusResponse);
            Assert.Equal(HttpStatusCode.OK, task2.StatusResponse);
        }

        [Fact]
        public async Task SendMultipleRequests_ToGoogle_ConcurrencyTest()
        {
            // URL của Google
            var url = "https://www.google.com";

            // Tạo danh sách các task để gửi 4 request đồng thời
            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 4; i++)
            {
                tasks.Add(_httpClient.GetAsync(url));
            }

            // Chờ tất cả các request hoàn thành
            var responses = await Task.WhenAll(tasks);

            // Kiểm tra phản hồi của từng request
            foreach (var response in responses)
            {
                Assert.True(response.IsSuccessStatusCode, "Request to Google failed");

                // Kiểm tra nội dung trả về để xác minh
                var content = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"{content}");
            }
        }
    }
}
