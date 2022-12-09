
using Library;

var mail = new MailBuilder()
    .From("noreply@yourdomain.ch", "YourName")
    .Subject("My Test")
    .AddRecipient( "anyone@manywhere.ch")
    .TextMessage("Hello World")
    .ScheduleAt(DateTimeOffset.Parse("2022-12-09 10:20:00+00:00"))
    .AttachFile("demo.jpg", @"c:\temp\any.jpg")
    .Build();

var sender = new MailSender("--YourKey--");
sender.SendMail(mail);