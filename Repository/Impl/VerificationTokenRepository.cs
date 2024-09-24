using BusinessObject;
using DataAccess.DAO;

namespace Repository.Impl;

public class VerificationTokenRepository: IVerificationTokenRepository
{
    public async Task SaveAsync(VerificationToken verificationToken) => await VerificationTokenDAO.Instance.SaveAsync(verificationToken);

    public async Task DeleteAsync(int verificationTokenId) => await VerificationTokenDAO.Instance.DeleteAsync(verificationTokenId);

    public async Task UpdateAsync(VerificationToken verificationToken) => await VerificationTokenDAO.Instance.UpdateAsync(verificationToken);

    public async Task<VerificationToken?> FindByTokenAsync(string token) => await VerificationTokenDAO.Instance.FindByTokenAsync(token);

    public async Task<VerificationToken?> FindByUserIdAsync(int userId) => await VerificationTokenDAO.Instance.FindByUserIdAsync(userId);
}