using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        Tuple<int, string> GetUserIDAndRole(string userName);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly DBRepository _dBRepository;


        public UserService(ILogger<UserService> logger, DBRepository dBRepository)
        {
            _logger = logger;
            _dBRepository = dBRepository;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _dBRepository.Users.FirstOrDefault(l => l.UserName == userName && l.Password == password) != null;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _dBRepository.Users.FirstOrDefault(l => l.UserName == userName) != null;
        }

        public string GetUserRole(string userName)
        {
            var user = _dBRepository.Users.FirstOrDefault(l => l.UserName == userName);

            if (user == null)
            {
                return string.Empty;
            }

            return GetRole(user.UserRole);

        }

        public Tuple<int, string> GetUserIDAndRole(string userName)
        {

            var user = _dBRepository.Users.FirstOrDefault(l => l.UserName == userName);
            var result = new Tuple<int, string>(0, string.Empty);

            if (user == null)
            {
                return result;
            }

            return new Tuple<int, string>(user.Id, GetRole(user.UserRole));
        }

        private string GetRole(int roleid)
        {
            return roleid switch
            {
                1 => "admin",
                2 => "customer",
                3 => "acc",
                _ => "customer",
            };
        }
    }

}
