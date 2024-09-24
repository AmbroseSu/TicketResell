using BusinessObject;

namespace Repository;

public interface IVerificationTokenRepository
{
    Task SaveAsync(VerificationToken verificationToken);
    Task DeleteAsync(int verificationTokenId);
    Task UpdateAsync(VerificationToken verificationToken);
    Task<VerificationToken?> FindByTokenAsync(string token);
    Task<VerificationToken?> FindByUserIdAsync(int userId);
}