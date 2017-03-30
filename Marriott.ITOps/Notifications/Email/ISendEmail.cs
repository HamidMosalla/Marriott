using System.Threading.Tasks;

namespace Marriott.ITOps.Notifications.Email
{
    public interface ISendEmail
    {
        Task SendMail(Email email);
    }
}
