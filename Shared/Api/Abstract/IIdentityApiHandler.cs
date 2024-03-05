using Shared.Api.DTOs;
using Shared.Model.Abstract;

namespace Shared.Api.Abstract
{
    public interface IIdentityApiHandler
    {
        Task<IApiResult> LoginAsync(LoginApiDto loginDto);
        Task<IApiResult> AddUserAsync(AddUserRequestApiDto addUserRequest);
        Task<IApiResult> LoginWithKeyAsync(string apiKey);
        Task<IApiResult> AddUserWithAdminServiceAsync(AddUserRequestApiDto addUserRequest);
        Task<IApiResult> GetUserKeysByIdAsync(string userId);
    }
}
