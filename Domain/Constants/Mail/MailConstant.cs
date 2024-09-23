namespace Domain.Constants.Mail;

public class MailConstant
{
    public class TicketMail
    {
        public const string Title = "Ticket for event";
        public const string PathTemplate = "./template/TicketUser.cshtml";
    }
    public class ReminderMail
    {
        public const string Title = "Reminder for event";
        public const string PathTemplate = "./template/ReminderEmail.cshtml";
    }

}
