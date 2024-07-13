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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Identity;

namespace MessangerBack.Services;

public class ChangePasswordService : IChangePasswordService
{
    private readonly EmailOptions _options;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IDistributedCache _cahce;

    public ChangePasswordService(
        IOptions<EmailOptions> options,
        UserManager<IdentityUser<Guid>> userManager,
        IDistributedCache cache)
    {
        _options = options.Value;
        _userManager = userManager;
        _cahce = cache;
    }

    public async Task SendEmailCode(SendEmailCodeSchema userData)
    {
        var user = await _userManager.FindByEmailAsync(userData.Email);
        if (user == null)
        {
            throw new NotFoundException("Пользователь с таким email не найден.");
        }

        int resetCode = new Random().Next(1000, 9999);
        await _cahce.SetStringAsync(userData.Email, resetCode.ToString());
        using (SmtpClient smtpClient = new SmtpClient(_options.Host, _options.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_options.UserName, _options.Password);
            smtpClient.EnableSsl = true;
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_options.UserName);
                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Код верификации";
                mailMessage.Body = $"Код для сброса пароля: {resetCode}";
                smtpClient.Send(mailMessage);
            }
        }
    }

    public async Task ChangePassword(ChangePasswordSchema changePasswordData)
    {
        var user = await _userManager.FindByEmailAsync(changePasswordData.Email);
        if (user == null)
        {
            throw new NotFoundException("Пользователь с таким email не найден.");
        }

        var validator = new PasswordValidator<IdentityUser<Guid>>();
        var passwordIsValid = (await validator
            .ValidateAsync(_userManager, user, changePasswordData.NewPassword))
            .Succeeded;

        if (!passwordIsValid)
        {
            throw new WrongUserInputException(
                "Длинна пароля должна быть больше 8 символов.\r\n" + 
                "Пароль должен содержать буквы верхнего и нижнего регистра, спец символы и цифры.");
        }

        var resetCode = await _cahce.GetStringAsync(changePasswordData.Email);
        await _cahce.RemoveAsync(changePasswordData.Email);
        if (resetCode != changePasswordData.ResetCode) 
        {
            throw new WrongUserInputException("Код сброса неверный.");
        }

        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, changePasswordData.NewPassword);
    }
}
