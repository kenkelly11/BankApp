﻿using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL.DepositRules
{
    public class FreeAccountDepositRule : IDeposit
    {
        public AccountDepositResponse Deposit(Account account, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse();

            // troubleshooting the user's deposit entry amounts. 
            if (account.Type != AccountType.Free)
            {
                response.Success = false;
                response.Message = "Error: a non-free account hit the Free Deposit Rule. Contact IT.";
                return response;
            }

            if (amount > 100)
            {
                response.Success = false;
                response.Message = "Free Accounts cannot deposit more than 100 dollars at a time.";
                return response;
            }
            if (amount <= 0)
            {
                response.Success = false;
                response.Message = "Deposit amounts must be greater than zero.";
                return response;
            }
            else
            {
                // storing the properties' values
                response.OldBalance = account.Balance;
                account.Balance += amount;
                response.Account = account;
                response.Amount = amount;
                response.Success = true;

                return response;
            }
            
        }
    }
}
