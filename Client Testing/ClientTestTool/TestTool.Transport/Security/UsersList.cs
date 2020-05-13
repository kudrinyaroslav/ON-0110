using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.Transport.Security
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UsersList
    {
        private static UsersList _usersList;
        private Dictionary<string, Account> _accounts;

        private UsersList()
        {
            _accounts = new Dictionary<string, Account>();
        }

        public static UsersList Current
        {
            get
            {
                if (_usersList == null)
                {
                    _usersList = new UsersList();
                }
                return _usersList;
            }
        }

        public void AddUser(Account account)
        {
            _accounts[account.Username] = account;
        }

        public void AddUser(string  username, string password)
        {
            _accounts[username] = new Account() {Password = password, Username = username};
        }

        public void RemoveUser(string username)
        {
            _accounts.Remove(username);
        }
        public void Clear()
        {
            _accounts.Clear();
        }

        public Account GetUser(string username)
        {
            if (_accounts.ContainsKey(username))
            {
                return _accounts[username];
            }
            else
            {
                return null;
            }
        }
    }
}
