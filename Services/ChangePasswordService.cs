using System.Net;
using System.Net.Mail;
using MessangerBack.Exceptions;
using MessangerBack.Models;
using MessangerBack.Options;
using MessangerBack.Repositories;
using MessangerBack.Schemas;
using MessangerBack.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace MessangerBack.Services;

public class ChangePasswordService : IChangePasswordService
{
    private readonly IUsersRepository _repository;
    private readonly EmailOptions _options;
    private readonly IPasswordUtils _passwordUtils;

    public ChangePasswordService(
        IOptions<EmailOptions> options,
        IUsersRepository repository,
        IPasswordUtils passwordUtils)
    {
        _passwordUtils = passwordUtils;
        _options = options.Value;
        _repository = repository;
    }

    public async Task SendEmailCode(SendEmailCodeSchema userData)
    {
        UserModel? user = await _repository.GetUserByEmail(userData.Email) 
            ?? throw new NotFoundException("User with this email not found");

        using (SmtpClient smtpClient = new SmtpClient(_options.Host, _options.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_options.UserName, _options.Password);
            smtpClient.EnableSsl = true;
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_options.UserName);
                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Verification code";
                int emailCode = new Random().Next(1000, 9999);
                mailMessage.Body = $"Your verification code: {emailCode}";
                smtpClient.Send(mailMessage);
            }
        }
    }

    public Task<bool> CompareEmailCodes(CompareEmailCodeSchema changePasswordData)
    {
        throw new NotImplementedException();
    }

    public async Task ChangePassword(ChangePasswordSchema changePasswordData)
    {
        UserModel? user = await _repository.GetUserByEmail(changePasswordData.Email) 
            ?? throw new NotFoundException("User with this email not found");
        
        if (_passwordUtils.CheckPassword(changePasswordData.NewPassword, user.PasswordHash))
        {
            throw new SamePasswordsException("New password must be different from old password");
        }

        user.PasswordHash = _passwordUtils.GeneratePasswordHash(changePasswordData.NewPassword);
        await _repository.ChangePassword(user);
    }
}
