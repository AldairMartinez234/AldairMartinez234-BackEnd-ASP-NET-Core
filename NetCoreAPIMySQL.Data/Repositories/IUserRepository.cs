using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using NetCoreAPIMySQL.Model.Responses;
using System.Collections.Generic;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public interface IUserRepository
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetUser(int id);
        RegisterResponse Register(RegisterRequest registerRequest);
    }
}
