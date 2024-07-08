using MessangerBack.Schemas;

namespace MessangerBack.Services;

public interface IChangePasswordService
{
    public Task SendEmailCode(SendEmailCodeSchema changePasswordData);
    public Task ChangePassword(ChangePasswordSchema changePasswordData);
}
