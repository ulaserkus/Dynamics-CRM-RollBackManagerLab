using Identity.Framework.Core.DTOs;
using Shared.Model.Abstract;

namespace Identity.Framework.Core.Services.Abstract
{
    public interface IUserService
    {
        Task<IApiResult> AddUserAsync(AddUserRequestDto userRequestDto);
        Task<IApiResult> LoginAsync(LoginUserRequestDto userRequestDto);
        Task<IApiResult> LoginWithApiKeyAsync(string apiKey);
        Task<IApiResult> GetUserApiKeysAsync(string userId);
    }
}
