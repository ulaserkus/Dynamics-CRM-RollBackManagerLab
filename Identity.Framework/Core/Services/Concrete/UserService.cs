using Identity.Framework.Core.DTOs;
using Identity.Framework.Core.Exception;
using Identity.Framework.Core.Mapper.Abstract;
using Identity.Framework.Core.Services.Abstract;
using Identity.Framework.Core.Services.JWT;
using Identity.Framework.Data.Model;
using Identity.Framework.Data.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Shared.Exception;
using Shared.Model.Abstract;
using Shared.Model.Concrete;
using Shared.Utils;

namespace Identity.Framework.Core.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserMapper _userMapper;
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;

        public UserService(IUserMapper userMapper, IUserRepository repository, IJwtService jwtService)
        {
            _userMapper = userMapper;
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<IApiResult> AddUserAsync(AddUserRequestDto userRequestDto)
        {
            var existingUser = await _repository.FindOneAsync(x => x.EmailAddress == userRequestDto.EmailAddress);

            if (existingUser is null)
            {
                var user = _userMapper.AddUserRequestDtoToUser(userRequestDto);
                user.ApiKeys = new List<ApiKey>() {
                    new ApiKey
                    {
                        Key = HashUtil.GenerateHash(ApiKeyUtil.GenerateApiKey(user.EmailAddress), out string apiKeySalt),
                        ExpiresInUTC = DateTime.UtcNow.AddMonths(6)
                    }
                };
                user.Password = HashUtil.GenerateHash(user.Password, out string passwordSalt);
                user.PasswordSalt = passwordSalt;
                await _repository.InsertOneAsync(user);
                return new ApiResult { StatusCode = StatusCodes.Status201Created };
            }
            else
            {
                throw new ExistsUserException(StatusCodes.Status400BadRequest, "You have already registired with this email address.", 1);
            }
        }

        public async Task<IApiResult> LoginAsync(LoginUserRequestDto userRequestDto)
        {
            var userFilter = Builders<User>.Filter.Eq(x => x.EmailAddress, userRequestDto.EmailAddress);
            var existingUser = await _repository.FindSingleAsync(userFilter);

            if (existingUser is not null)
            {
                var verifyPassword = HashUtil.VerifyHash(userRequestDto.Password, existingUser.Password, existingUser.PasswordSalt);
                if (verifyPassword)
                {
                    var token = _jwtService.GenerateToken(existingUser);
                    return new ApiResult { StatusCode = StatusCodes.Status200OK, Result = token };
                }

                throw new UnAuthorizedException();
            }
            else
            {
                throw new ExistsUserException(StatusCodes.Status400BadRequest, "Invalid authentication, wrong email or password information.", 1);
            }
        }

        public async Task<IApiResult> LoginWithApiKeyAsync(string apiKey)
        {

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ExistsUserException(StatusCodes.Status400BadRequest, "Invalid authentication, wrong email or password information.", 1);
            }

            var existingUser = await _repository.GetUserWithApiKeyAsync(apiKey);

            if (existingUser is not null)
            {
                var apiKeyObject = existingUser.ApiKeys.Single(x => x.Key == apiKey);
                if (apiKeyObject.ExpiresInUTC < DateTime.UtcNow)
                {
                    throw new UnAuthorizedException(StatusCodes.Status401Unauthorized, "This Key is expired !");
                }
                var token = _jwtService.GenerateToken(existingUser);
                return new ApiResult { StatusCode = StatusCodes.Status200OK, Result = token };
            }
            else
            {
                throw new ExistsUserException(StatusCodes.Status400BadRequest, "Invalid authentication, wrong email or password information.", 1);
            }
        }

        public async Task<IApiResult> GetUserApiKeysAsync(string userId)
        {
            var existingUser = await _repository.FindByIdAsync(userId);

            if (existingUser is not null)
            {
                return new ApiResult { StatusCode = StatusCodes.Status200OK, Result = existingUser.ApiKeys };
            }
            else
            {
                throw new ExistsUserException(StatusCodes.Status400BadRequest, "Invalid authentication, wrong email or password information.", 1);
            }
        }
    }
}
