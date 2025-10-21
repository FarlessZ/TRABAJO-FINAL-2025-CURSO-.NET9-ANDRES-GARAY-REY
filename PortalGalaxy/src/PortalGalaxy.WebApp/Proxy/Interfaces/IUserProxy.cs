using System;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;

namespace PortalGalaxy.WebApp.Proxy.Interfaces;

public interface IUserProxy
{
    Task<LoginDtoResponse> Login(LoginDtoRequest request);

    Task Register(RegisterUserDto request);

}
