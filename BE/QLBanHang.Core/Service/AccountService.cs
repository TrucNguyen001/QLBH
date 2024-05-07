using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using QLBanHang.Core.Service;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Resource;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.ValidateException;
using QLBanHang.Core.Auth;

namespace QLBanHang.Core.Service
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        IAccountRepository _accountRepository;
        IConfiguration _configuration;

        private static System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public static System.Runtime.Caching.MemoryCache Cache
        {
            get { return _cache; }
        }

        public AccountService(IAccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public string GetAccountCodeBiggest()
        {
            var accounts = _accountRepository.SortDecrease();
            var accountCode = "";
            if (accounts == null)
            {
                accountCode = "KH-00001";
            }
            else
            {
                Account account = accounts.FirstOrDefault();
                accountCode = account.AccountCode;
            }
            var numberCode = accountCode.Substring(3);
            var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
            numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
            accountCode = accountCode.Substring(0, 3) + numberCode;
            return accountCode;
        }

        /// <summary>
        /// Kiểm tra xem password có đúng hay không
        /// </summary>
        /// <param name="password">Mật khẩu nhập</param>
        /// <param name="account">Tài khoản tìm được theo username</param>
        /// <returns>
        /// true: Nhập đúng mật khẩu
        /// false: Sai mật khẩu
        /// </returns>
        /// Created by: Nguyễn Văn Trúc(17/02/2024)
        public bool CheckPassword(Account account, string password)
        {
            if (account.Password == password)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Đăng nhâp
        /// </summary>
        /// <param name="account">Tài khoản dùng để đăng nhập</param>
        /// <returns>Trả về 1 nếu nhập đầy đủ dữ liệu</returns>
        /// CreatedBy: Nguyễn Văn Trúc(4/4/2024)
        public int Login(AccountLogin account)
        {
            if(string.IsNullOrEmpty(account.Account))
            {
                throw new ValidateExceptionAuth(MISAResourceVN.AccountNotNull, MISAResourceVN.DevMsg);
            }
            if(string.IsNullOrEmpty(account.Password))
            {
                throw new ValidateExceptionAuth(MISAResourceVN.PasswordNotNull, MISAResourceVN.DevMsg);
            }
            return 1;
        }

        /// <summary>
        /// Kiểm tra xem tài khoản có tồn tại không
        /// </summary>
        ///  <param name="account">Tài khoản dùng để đăng nhập</param>
        /// <returns>Trả về thông tin token nếu tài khoản tồn tại</returns>
        /// CreatedBy: Nguyễn Văn Trúc(4/3/2024)
        public TokenModel CheckAccount(AccountLogin account, string role)
        {
            Login(account);
            var user = _accountRepository.Login(account, role);
            if(user != null && CheckPassword(user, account.Password))
            {
                var fullName = (user.FullName == null) ? user.UserName : user.FullName;
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(MISAResourceVN.Roles, user.Role),
                    new Claim("FullName", fullName),
                    new Claim("AccountCode", user.AccountCode)
                };

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration[MISAResourceVN.RefreshTokenValidityInDays], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                _accountRepository.UpdateAccount(user);

                var modelToken = new TokenModel()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                };

                return modelToken;
            }
            throw new ValidateExceptionAuth(MISAResourceVN.LoginNotSuccess, MISAResourceVN.DevMsg);
        }

        /// <summary>
        /// Hàm làm mới token
        /// </summary>
        /// <param name="tokenModel">Thông tin cần thiết để có thể làm mới token</param>
        /// <returns>Trả về model token chứa token mới</returns>
        /// Created by: Nguyễn Văn Trúc(4/4/2024)
        public TokenModel RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.NotValid);
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.TokenNotValid);
            }

            string userName = principal.Identity.Name;

            var user = _accountRepository.GetByUserName(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.TokenNotValid);
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _accountRepository.UpdateAccount(user);

            var modelToken = new TokenModel()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                Expiration = newAccessToken.ValidTo
            };

            return modelToken;
        }

        /// <summary>
        /// Hàm đăng ký tài khoản
        /// </summary>
        /// <param name="model">Model chứa thông tin dùng để đăng ký</param>
        /// <returns>Trả về 1 nếu xác nhận thông tin thành công</returns>
        /// Created by: Nguyễn Văn Trúc(4/3/2024)
        public int Register(RegisterModel model)
        {
            var userExists = _accountRepository.GetByUserName(model.UserName);
            if (userExists != null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.UserNameValid);
            }
            return 1;
        }

        /// <summary>
        /// Hàm huỷ bỏ token
        /// </summary>
        /// <param name="userName">Tên người dùng</param>
        /// <returns>Trả về tài khoản nếu huỷ thành công</re
        public Account Revoke(string userName)
        {
            var user = _accountRepository.GetByUserName(userName);
            if (user == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.UserNameValid);
            }
            user.RefreshToken = null;
            return user;
        }

        /// <summary>
        /// Hàm xoá tất cả token
        /// </summary>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// Created by: Nguyễn Văn Trúc(4/4/2024)
        public int RevokeAll()
        {
            var accounts = _accountRepository.GetAll(1);
            if(accounts == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.ListAccountEmpty);
            }
            foreach (var user in accounts)
            {
                user.RefreshToken = null;
                _accountRepository.UpdateAccount(user);
            }
            return 1;
        }

        /// <summary>
        /// Hàm tạo token
        /// </summary>
        /// <param name="authClaims">Danh sách các Clain</param>
        /// <returns>Trả về token đã tạo</returns>
        /// CreatedBy: NVTruc(17/2/2024)
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[MISAResourceVN.Secret]));
            _ = int.TryParse(_configuration[MISAResourceVN.TokenValidityInMinutes], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration[MISAResourceVN.ValidIssuer],
                audience: _configuration[MISAResourceVN.ValidAudience],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// Hàm tạo refresh token mới
        /// </summary>
        /// <returns>Trả về 1 chuỗi refresh token ngẫu nhiên</returns>
        /// CreatedBy: NVTruc(17/2/2024)
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Hàm lấy ra các ClaimsPrincipal từ một token JWT
        /// </summary>
        /// <param name="token">Chuỗi token muốn xác thực</param>
        /// <returns>Trả về một ClaimsPrincipal chứa các Claim từ token JWT đã xác thực</returns>
        /// <exception cref="SecurityTokenException"></exception>
        /// CreatedBy: NVTruc(17/2/2024)
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.TokenNotValid);

            return principal;

        }

        /// <summary>
        /// Hàm gửi email lấy lại mật khẩu
        /// </summary>
        /// <param name="email">Email muốn gửi đến</param>
        /// <returns>Trả về 1 nếu gửi thành công</returns>
        ///  CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public int SendEmail(string email)
        {
            CheckEmailNull(email);
            var account = _accountRepository.GetByEmail(email);
            if(account == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.EmailNoValid);
            }
            // Lấy code ngẫu nhiên
            var codeRecoverPassword = GenerateRandomNumber();

            Cache.Set($"{MISAResourceVN.CodeRecoverPassword}-${email}", codeRecoverPassword, DateTimeOffset.UtcNow.AddMinutes(5));

            // Lấy thông tin từ appsetting
            var fromMail = _configuration[MISAResourceVN.FromMail];
            var fromPassword = _configuration[MISAResourceVN.FromPassword];

            // Tạo một đối tượng MailMessage để gửi email
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = MISAResourceVN.SubjectCodeRecoverPassword;
            message.To.Add(new MailAddress(email));
            message.Body = $"{codeRecoverPassword} {MISAResourceVN.BodyCodeRecoverPassword}";
            message.IsBodyHtml = true;

            // Tạo một đối tượng SmtpClient để gửi email thông qua máy chủ SMTP của Gmail
            var smtpClient = new SmtpClient(_configuration[MISAResourceVN.SMTPGmail])
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            // Gửi
            smtpClient.Send(message);
            return 1;
        }

        /// <summary>
        /// Tạo ra số có 6 số ngẫu nhiên
        /// </summary>
        /// <returns>Trả về số ngẫu nhiên</returns>
        /// CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public int GenerateRandomNumber()
        {
            Random random = new Random();
            // Sử dụng Next() để tạo số ngẫu nhiên trong khoảng từ 100000 đến 999999
            int randomNumber = random.Next(100000, 999999);
            return randomNumber;
        }

        /// <summary>
        /// Check email
        /// </summary>
        /// <param name="email">email kiểm tra</param>
        /// <exception cref="ValidateException.ValidateExceptionError">Ngoại lệ ném ra khi xảy ra lỗi validate</exception>
        /// CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public void CheckEmailNull(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.EmailNotEmpty);
            }
            else if(!IsValidEmail(email))
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.EmailMalformed);
            }
        }

        /// <summary>
        /// Hàm kiểm tra trước khi cập nhật lại mật khẩu
        /// </summary>
        /// <param name="AccountId">Id tài khoản</param>
        /// <param name="account">Tài khoản</param>
        /// <exception cref="ValidateException.ValidateExceptionError">Ngoại lệ ném ra khi xảy ra lỗi validate</exception>
        /// CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public void UpdatePassword(Guid accountId, Account account)
        {
            if(accountId == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.AccountIdNotValid);
            }
            if (account == null)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.AccountNotValid);
            }
        }

        // <summary>
        /// Kiểm tra mã code có đúng không
        /// </summary>
        /// <param name="code">Mã code</param>
        /// <returns>
        /// Trả về 1 nếu nhập đúng
        /// </returns>
        ///  CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public int CheckCodeRecoverPassword(string code, string email)
        {
            CheckCodeRecoverPasswordNotNull(code);
            // Lấy mã code lưu trong cache
            
            var codeRecover = Cache[$"{ MISAResourceVN.CodeRecoverPassword}-${email}"];

            if(Convert.ToInt32(codeRecover) == Convert.ToInt32(code.Replace(" ", "")))
            {
                return 1;
            }
           
            throw new ValidateException.ValidateExceptionError(MISAResourceVN.CodeRecoverPasswordNoValid);
        }

        /// <summary>
        /// Kiểm tra mã có null hay không
        /// </summary>
        /// <param name="code">Mã kiểm tra</param>
        /// <exception cref="ValidateException.ValidateExceptionError">Ngoại lệ ném ra khi xảy ra lỗi validate</exception>
        /// CreatedBy: Nguyễn Văn Trúc(9/3/2024)
        public void CheckCodeRecoverPasswordNotNull(string code)
        {
            if(string.IsNullOrEmpty(code))
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.CodeRecoverPasswordNoValid);
            }
        }

        public int GetPaging(int pageSize, int pageIndex, string? text)
        {
            if (pageSize < 1)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.PageSizeNotLessThan1);
            }
            if (pageIndex < 1)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.PageIndexNotLessThan1);
            }
            return 1;
        }

        /// <summary>
        /// Kiểm tra định dạng email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>
        /// false: Email không đúng định dạng
        /// true: Email đúng định dạng
        /// </returns>
        /// CreatedBy: NVTruc(28/12/2023)
        public bool IsValidEmail(string email)
        {
            if (email == "" || email == null)
            {
                return true;
            }
            else
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
