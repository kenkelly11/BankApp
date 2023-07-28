using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Models.Responses
{
    public class AccountLookupResponse : Response
    {
        // the green account is the Account class. Which means, the white account is the name for the entirety of the info
        // contained in the Account object. Eg, Name, AccountNumber, Balance, and AccountType
        // So, every time the white Account is used in other classes, it is an object that contains all of the information in Account.
        public Account Account { get; set; }
    }
}
