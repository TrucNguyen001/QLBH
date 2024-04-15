using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using QLBanHang.Core.Auth;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Service;
using QLBanHang.Infrastructure.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QLBanHang.API.Controllers
{
    /// <summary>
    /// Các dịch vụ về tài khoản
    /// </summary>
    /// CreatedBy: Nguyễn Văn Trúc(1/3/2024)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Field
        IAccountRepository _accountRepository;

        IAccountService _accountService;
        #endregion

        #region Constructor
        public AccountController(IAccountRepository accountRepository, IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _accountService = accountService;
        }
        #endregion

        /// <summary>
        /// Lấy bản ghi theo role
        /// </summary>
        /// <param name="role">role của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("getByRole/{role}")]
        public IActionResult GetByRole(string role)
        {
            var entity = _accountRepository.GetByRole(role);
            return StatusCode(200, entity);
        }

        /// <summary>
        /// Phân trang những bản ghi
        /// </summary>
        /// <param name="pageSize">Số lượng trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="text">Từ tìm kiếm trong trang</param>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        ///</returns>
        /// CreatedBy: NVTruc(24/12/2023)
        [HttpGet("paging/{role}")]
        public IActionResult Paging(int pageSize, int pageIndex, string text = "", string role = "User")
        {
            var validatePage = _accountService.GetPaging(pageSize, pageIndex, text);
            var entities = _accountRepository.GetPaging(pageSize, pageIndex, text, role);

            // Lấy ra tổng số bản ghi theo phân trang
            var total = _accountRepository.GetByTextUser(text, role).Count();

            var result = new
            {
                ToTalRecord = total,
                ListRecord = entities,
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("getById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var entity = _accountRepository.GetById(id);
            return StatusCode(200, entity);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="account">Tài khoản đăng nhập</param>
        /// <returns>
        /// 201: Đăng nhập thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(17/2/2024)
        [HttpPost]
        [Route("login/{role}")]
        public async Task<IActionResult> Login(AccountLogin account, string role)
        {
            var result = _accountService.CheckAccount(account, role);

            var user = _accountRepository.Login(account, role);

            var model = new
            {
                Model = result,
                FullName = user.FullName == null ? user.UserName : user.FullName,
                AccountId = user.AccountId,
                Email = user.Email,
            };

            return StatusCode(200, model);
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="tokenModel">model token</param>
        /// 200: làm mới thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(19/2/2024)
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var res = _accountService.RefreshToken(tokenModel);
            return StatusCode(200, res);
        }


        /// <summary>
        /// Đăng ký tài khoản user
        /// </summary>
        /// <param name="model">Thông tin đăng ký</param>
        /// <returns>
        /// 201: Đăng ký thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(1/3/2024)
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var res = _accountService.Register(model);
            Account user = new()
            {
                AccountId = Guid.NewGuid(),
                AccountCode = _accountService.GetAccountCodeBiggest(),
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                Role = "User",
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                NormalizedEmail = model.Email.ToLower(),
                NormalizedUserName = model.UserName.ToLower(),
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnd = 30,
                RefreshToken = "",
                RefreshTokenExpiryTime = null,
            };
            var result = _accountRepository.InsertAccount(user);
            return StatusCode(201, result);
        }

        /// <summary>
        /// Đăng ký tài khoản admin
        /// </summary>
        /// <param name="model">Thông tin đăng ký</param>
        /// <returns>
        /// 201: Đăng ký thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(1/3/2024)
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterModel model)
        {
            var res = _accountService.Register(model);

            Account admin = new()
            {
                AccountId = Guid.NewGuid(),
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                Role = "Admin",
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                NormalizedEmail = model.Email.ToLower(),
                NormalizedUserName = model.UserName.ToLower(),
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnd = 30,
                RefreshToken = "",
                RefreshTokenExpiryTime = null,
            };
            var result = _accountRepository.InsertAccount(admin);
            return StatusCode(201, result);
        }
        /// <summary>
        /// Huỷ bỏ token
        /// </summary>
        /// <param name="username">Tên người dùng</param>
        /// <returns>
        /// 201: Huỷ bỏ thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(1/3/2024)
        [Authorize]
        [HttpGet("{username}")]
        public IActionResult Revoke(string username)
        {
            var user = _accountService.Revoke(username);
            var result = _accountRepository.UpdateAccount(user);

            return StatusCode(200, result);
        }


        /// <summary>
        /// Huỷ bỏ tất cả token
        /// </summary>
        /// <returns>
        /// 201: Huỷ bỏ thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(1/3/2024)
        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var result = _accountService.RevokeAll();

            return StatusCode(200, result);
        }

        /// <summary>
        /// Gửi mã code về email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// 200: Gửi thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(9/3/2024)
        [HttpGet("forget/sendEmail/{email}")]
        public IActionResult SendEmail(string email)
        {
            var result = _accountService.SendEmail(email);
            return StatusCode(200, 1);
        }

        /// <summary>
        /// Kiểm tra mã code lấy lại mật khẩu
        /// </summary>
        /// <param name="code">Mã code</param>
        /// <returns>
        /// 200: Gửi thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(9/3/2024)
        [HttpGet("forget/checkCode/{code}/{email}")]
        public IActionResult CheckCodeRecoverPassword(string code, string email)
        {
            var result = _accountService.CheckCodeRecoverPassword(code, email);
            return StatusCode(200, result);
        }

        /// <summary>
        /// Cập nhật lại mật khẩu
        /// </summary>
        /// <param name="accountId">Id tài khoản</param>
        /// <param name="account">tài khoản</param>
        /// <returns>
        /// 201: Cập nhật thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(9/3/2024)
        [HttpPut("forget/reupdatePassword/{accountId}")]
        public IActionResult UpdatePassword(Guid accountId, Account account)
        {
            _accountService.UpdatePassword(accountId, account);
            var reusult = _accountRepository.UpdatePassword(accountId, account);

            return StatusCode(200, reusult);
        }

        /// <summary>
        /// Lấy tài khoản theo email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>
        /// 200: Lấy thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(9/3/2024)
        [HttpGet("forget/getByEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var result = _accountRepository.GetByEmail(email);
            return StatusCode(200, result);
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        /// <param name="entity">Bản ghi</param>
        /// <param name="id">Ib bản ghi muốn sửa</param>
        /// <returns>
        /// 200: Sửa dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpPut("put/{id}")]
        public IActionResult Put(Account entity, Guid id)
        {
            var service = _accountService.Update(entity, id);

            var respository = _accountRepository.Update(entity, id);

            return StatusCode(200, respository);
        }
    }
}
