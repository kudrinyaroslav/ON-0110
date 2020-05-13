using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUT.WithLogic.Proxy
{
    public partial class User
    {
        public User CloneWithoutPassword()
        {
            User res = new User();

            res.UserLevel = this.UserLevel;
            res.Username = this.Username;
            res.AnyAttr = this.AnyAttr;
            res.Extension = this.Extension;
            res.Password = null;

            return res;
        }
    }
}