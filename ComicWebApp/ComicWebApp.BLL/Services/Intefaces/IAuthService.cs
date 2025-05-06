using ComicWebApp.DAL.Models.User;
using ComicWebApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicWebApp.BLL.Services.Intefaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterUserDto register);
    Task<TokenResponseDto> LoginAsync(LoginUserDto login);
    Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequest request);
}
