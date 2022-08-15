using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using NetCoreAPIMySQL.Model.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebTestNET.Helpers;

namespace NetCoreAPIMySQL.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppSettings _appSettings;
        private MySQLConfiguration _connectionString;
        public UserRepository(IOptions<AppSettings> appSettings, MySQLConfiguration connectionString)
        {
            _appSettings = appSettings.Value;
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT *
                         FROM users
                         WHERE email = @email";

            var user = db.QueryFirstOrDefault<User>(sql, new { email = model.Email});

            // return null if user not found
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            var db = dbConnection();

            var sql = @"
                        SELECT id, name, email
                         FROM users";

            return db.Query<User>(sql, new { });
        }

        public User GetUser(int id)
        {
            var db = dbConnection();

            var sql = @"
                        SELECT id, name, email
                         FROM users
                         WHERE id = @id ";

            return  db.QueryFirstOrDefault<User>(sql, new { Id = id });
        }

        // helper methods
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            string message;
            try
            {
                var db = dbConnection();

                db.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT count(*) FROM users WHERE email = '" + registerRequest.Email + "';", db);

                int exists = Convert.ToInt32(cmd.ExecuteScalar());

                if (exists > 0)
                {
                    db.Close();
                    message = "Este correo ya ha sido registrado por otro usuario";
                    return new RegisterResponse(message , 0);
                }
                else
                {
                    var sql = @"
                         INSERT INTO users (name, email, password, created_at)
                          VALUES (@Name, @Email, @Password, now()) ";
                    string Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
                    var result = db.ExecuteAsync(sql, new { registerRequest.Name, registerRequest.Email, Password });
                    db.Close();
                    message = "Registro exitoso";
                    return new RegisterResponse(message, 1);
                }
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
                return new RegisterResponse(message, 0);
            }
        }
    }
}
