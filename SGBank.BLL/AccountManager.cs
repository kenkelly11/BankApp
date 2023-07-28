using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL
{
    public class AccountManager
    {
        // the interface is like the blueprint, or idea of the thing. The _accountRepository is the actual thing. 
        private IAccountRepository _accountRepository;
        
        public AccountManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

        }

        // this takes the accountNumber received from the user in AccountLookupWorkflow and verifies that it's associated with an account
        public AccountLookupResponse LookupAccount(string accountNumber)
        {
            AccountLookupResponse response = new AccountLookupResponse();

            if (accountNumber == "1")
            {
                response.Account = _accountRepository.LoadAccount(accountNumber);
            }
            if (accountNumber == "2")
            {
                response.Account = _accountRepository.LoadAccount(accountNumber);
            }
            else if (accountNumber == "3")
            {
                response.Account = _accountRepository.LoadAccount(accountNumber);
            }
            

            // if the account number entered by the user is not associated with an exisiting account, 
            // the error message pops up. Otherwise, the account information loads. 
            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }

        public AccountDepositResponse Deposit(string accountNumber, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse();

            response.Account = _accountRepository.LoadAccount(accountNumber);

            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
                return response;
            }
            else
            {
                response.Success = true;
            }

            // we're calling the Create method from the DepositRulesFactory class, based on the user's entry
            IDeposit depositRule = DepositRulesFactory.Create(response.Account.Type);
            response = depositRule.Deposit(response.Account, amount);

            if (response.Success)
            {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
        }

        public AccountWithdrawResponse Withdraw(string accountNumber, decimal amount)
        {
            AccountWithdrawResponse response = new AccountWithdrawResponse();

            response.Account = _accountRepository.LoadAccount(accountNumber);

            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account!";
            }
            else
            {
                response.Success = true;
            }

            IWithdraw withdrawRule = WithdrawRulesFactory.Create(response.Account.Type);
            response = withdrawRule.Withdraw(response.Account, amount);

            if (response.Success == true)
            {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
            
        }
    }
}
