using MassTransit;
using Moq;
using QNotificationService.Application.Consumers;
using QNotificationService.Application.Interfaces;
using QNotificationService.Contracts.NotificationEvents;

namespace QNotificationService.UnitTest.QNotificationService.Application.Tests;

public class SendNotificationConsumerTest
{
    private readonly Mock<INotificationService> _mockNotificationService;
    private readonly Mock<ConsumeContext<SendNotificationEvent>> _mockContext;
    private readonly SendNotificationConsumer _consumer;
    
    public SendNotificationConsumerTest()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _mockContext = new Mock<ConsumeContext<SendNotificationEvent>>();
        _consumer = new SendNotificationConsumer(_mockNotificationService.Object);
    }

    [Fact]
    public async Task Consumer_ValidMessage_Calls_Send_Notification()
    {
        var expectedEvent = new SendNotificationEvent()
        {
            Email = "tests@gmail.com",
            UserId = 1,
            Message = "Test Message"
        };
        _mockContext.Setup(s => s.Message).Returns(expectedEvent);

        //Act
        await _consumer.Consume(_mockContext.Object);
        
        //Assert
        
        _mockNotificationService.Verify(s=>s.SendAsync(expectedEvent.Email, expectedEvent.UserId, expectedEvent.Message, It.IsAny<CancellationToken>()), Times.Once);
    }
}