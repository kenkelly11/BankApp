using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Models.Interfaces
{
    public interface IAccountRepository
    {
        // Returns an account by account number
        // saves an account by passing in account details
        // Blueprint. Any time I use LoadAccount method, Must return an Account object, and use a string AccountNumber
        Account LoadAccount(string AccountNumber);
        void SaveAccount(Account account);
    }
}
