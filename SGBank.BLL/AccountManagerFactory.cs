using SGBank.Data;
using SGBank.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL
{
    public static class AccountManagerFactory
    {
        public static AccountManager Create()
        {
            // determines which repository we'll be working with
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            if (mode == "FreeTest")
                return new AccountManager(new FreeAccountTestRepository());
            else if (mode == "BasicTest")
                return new AccountManager(new BasicAccountTestRepository());
            else if (mode == "PremiumTest")
                return new AccountManager(new PremiumAccountTestRepository());
            else if (mode == "FileTest")
                return new AccountManager(new FileAccountRepository(FilePathSettings.FilePath));
            else
                throw new Exception("Mode value in app config is not valid.");
        }
    }
}
