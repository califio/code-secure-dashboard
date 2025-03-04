using CodeSecure.Manager.Integration.Model;

namespace CodeSecure.Manager.Integration.Mail;

public interface IMailSender
{
    public Task<NotificationResult> SendMailAsync(MailModel model);
    public Task<NotificationResult> SendTestMailAsync(string receiver);
    public Task SendResetPassword(IEnumerable<string> receivers, ResetPasswordModel model);
    public Task SendInviteUser(IEnumerable<string> receivers, InviteUserModel model);
    public Task SendAddUserToProject(IEnumerable<string> receivers, AddUserToProjectModel model);
    public Task SendRemoveProjectMember(IEnumerable<string> receivers, RemoveProjectMemberModel model);
}