using Microsoft.Extensions.Logging;
using Moq;
using QNotificationService.Application.Interfaces;
using QNotificationService.Application.Services;

namespace QNotificationService.UnitTest.QNotificationService.Application.Tests;

public class NotificationServiceTests
{
    private readonly Mock<ILogger<NotificationService>> _mockLogger;
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly NotificationService _notificationService;

    public NotificationServiceTests()
    {
        _mockLogger = new Mock<ILogger<NotificationService>>();
        _mockEmailSender = new Mock<IEmailSender>();
        _notificationService = new NotificationService(_mockLogger.Object, _mockEmailSender.Object);
    }

    [Fact]
    public async Task Should_Send_Email_Successfully()
    {
        _mockEmailSender.Setup(s => s.SendEmailAsync("test@gmail", "Test Subject", "Test Message"));

        await _notificationService.SendAsync("test@gmail.com", 1, "Test Message", CancellationToken.None);
    }
}