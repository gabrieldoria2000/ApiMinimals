using ApiCatalogoMinimals.Models;

namespace ApiCatalogoMinimals.Services;

public interface ITokenService
{
    string GerarToken(string key, string user, string perfil, UserModel usuario);
}
