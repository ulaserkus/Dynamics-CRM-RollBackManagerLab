using Identity.Framework.Core.DTOs;
using Identity.Framework.Data.Model;

namespace Identity.Framework.Core.Mapper.Abstract
{
    public interface IUserMapper
    {
        User AddUserRequestDtoToUser(AddUserRequestDto userRequestDto);
    }
}
