using Identity.Framework.Core.DTOs;
using Identity.Framework.Core.Mapper.Abstract;
using Identity.Framework.Data.Model;
using Riok.Mapperly.Abstractions;

namespace Identity.Framework.Core.Mapper.Concrete
{
    [Mapper]
    public partial class UserMapper : IUserMapper
    {
        public partial User AddUserRequestDtoToUser(AddUserRequestDto userRequestDto);
        public partial User LoginUserRequestDtoToUser(LoginUserRequestDto userRequestDto);
    }
}
