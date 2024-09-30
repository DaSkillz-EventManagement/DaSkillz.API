using Application.Abstractions.Caching;
using Application.UseCases.User.Queries.GetAllUsers;
using AutoMapper;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using System.Text.Json;
using Xunit.Abstractions;

namespace Daskillz.Testing.Users
{
    public class GetUser
    {
        private readonly Mock<IRedisCaching> _cachingMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ITestOutputHelper _output;
        private readonly IMapper _mapper;

        public GetUser(ITestOutputHelper output)
        {
            _cachingMock = new Mock<IRedisCaching>();
            _output = output;
            _userRepositoryMock = new Mock<IUserRepository>();

            // Configure the AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponseDto>()
                   .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                   .ReverseMap();
            });

            _mapper = config.CreateMapper();
        }




        [Fact]
        public async Task GetAllUser_InsertData_Return200GetAllSuccessfully()
        {
            // Arrange
            var handler = new GetAllUserQueryHandler(_userRepositoryMock.Object, _mapper, _cachingMock.Object);

            var fakeUsers = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    FullName = "Test User 1",
                    Email = "test1@example.com",
                    Phone = "09090310241",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Role = new Role
                    {
                        RoleId = 1,
                        RoleName = "User",
                    }
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    FullName = "Test User 2",
                    Email = "test2@example.com",
                    Phone = "09090310242",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Role = new Role
                    {
                        RoleId = 1,
                        RoleName = "User",
                    }
                }
            };

            // Mock the repository to return the fake users
            _userRepositoryMock
                .Setup(x => x.GetAllUser(It.IsAny<int>(), It.IsAny<int>(), "email", false))
                .ReturnsAsync(fakeUsers);

            // Act
            var resultHandler = await handler.Handle(new GetAllUserQuery(1, 10), default);

            // Assert
            var jsonData = JsonSerializer.Serialize(resultHandler.Data);
            _output.WriteLine($"Data: {jsonData}");
            _output.WriteLine($"Result Message: {resultHandler.Message}");

            resultHandler.Data.Should().NotBeNull();
            var resultData = resultHandler.Data as IEnumerable<UserResponseDto>;
            resultData.Should().NotBeEmpty();
            resultData.Should().HaveCount(2); // Check the number of users returned
        }


        [Fact]
        public void Map_CheckMapper_ReturnRoleNameNotNull()
        {
            //arrange

            var fakeUser = new User
            {
                UserId = Guid.NewGuid(),
                FullName = "Test User",
                Email = "test@example.com",
                Phone = "09090310241",
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                Role = new Role
                {
                    RoleId = 1,
                    RoleName = "Test",
                }
            };

            //act
            var result = _mapper.Map<UserResponseDto>(fakeUser);

            //assert
            _output.WriteLine($"RoleId: {result.RoleId} && RoleName: {result.RoleName}");
            result.RoleId.Should().Be(fakeUser.RoleId);
            result.RoleName.Should().Be(fakeUser.Role.RoleName);
        }
    }
}
