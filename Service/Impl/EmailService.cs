using System.Net;
using BusinessObject;
using DataAccess.DTO.Response;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Repository;
using Service.Response;

namespace Service.Impl;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUserRepository _userRepository;

    public EmailService(IConfiguration config, IVerificationTokenRepository verificationTokenRepository, IUserRepository userRepository)
    {
        _config = config;
        _verificationTokenRepository = verificationTokenRepository;
        _userRepository = userRepository;
    }
    public async Task<ResponseDTO> SendEmail(String emailResponse)
    {
        try
        {
            var user = await _userRepository.FindUserByEmailAsync(emailResponse);

            
            if (user == null)
            {
                return ResponseUtil.Error("User not found for the provided email", "Failed", HttpStatusCode.NotFound);
            }
            int otp = GenerateOTP();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("lisa92@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(emailResponse));
            email.Subject = "OTP ECommerce";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = otp.ToString()
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection( "EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate( _config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value );
            smtp.Send(email);
            smtp.Disconnect(true);
            var verificationToken = new VerificationToken(otp.ToString(), user.Id);
            //verificationToken.user = user;

            // Save the verification token to the repository
            await _verificationTokenRepository.SaveAsync(verificationToken);
            return ResponseUtil.GetObject("Send email Success", "ok", HttpStatusCode.Created, null);
        }
        catch (Exception ex)
        {
            return ResponseUtil.Error(ex.Message, "Failed", HttpStatusCode.InternalServerError);
        }
        
    }
    
    private int GenerateOTP()
    {
        Random random = new Random();
        int otp = random.Next(100000, 999999); // Tạo một số ngẫu nhiên từ 100000 đến 999999
        return otp;
    }
}