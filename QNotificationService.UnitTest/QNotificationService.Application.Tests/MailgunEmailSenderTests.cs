using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using QNotificationService.Application;
using QNotificationService.Application.Services.EmailSenderTypes;
using RichardSzalay.MockHttp;

namespace QNotificationService.UnitTest.QNotificationService.Application.Tests;

public class MailgunEmailSenderTests
{
    private readonly MailgunSettings _mailgunSettings;
    private readonly MockHttpMessageHandler _handler;
    private readonly MailgunEmailSender _emailSender;

    public MailgunEmailSenderTests()
    {
        _mailgunSettings = new MailgunSettings
        {
            ApiKey = "test-api-key",
            Domain = "test-domain",
            FromEmail = "sender@test"
        };
        _handler = new MockHttpMessageHandler();
        var httpClient = _handler.ToHttpClient();

        _emailSender = new MailgunEmailSender(Options.Create(_mailgunSettings), httpClient);
    }

    [Fact]
    public async Task Send_Email_When_Commands_Valid()
    {
        // Arrange
        
        var to = "test@gmail.com";
        var subject = "Test Subject";
        var body = "Test Body Content";

        var expectedUrl = $"https://api.mailgun.net/v3/{_mailgunSettings.Domain}/messages";

        _handler.Expect(HttpMethod.Post, expectedUrl)
            .WithFormData(new[]
            {
                new KeyValuePair<string, string>("from", _mailgunSettings.FromEmail),
                new KeyValuePair<string, string>("to", to),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", body)
            })
            .Respond(HttpStatusCode.OK, new StringContent("{\"message\":\"Email sent\"}", Encoding.UTF8, "application/json"));

        // Act
        await _emailSender.SendEmailAsync(to, subject, body);

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.VerifyNoOutstandingRequest();
    }
}