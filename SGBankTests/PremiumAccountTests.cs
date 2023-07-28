using NUnit.Framework;
using SGBank.BLL;
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

namespace SGBankTests
{
    [TestFixture]
    public class PremiumAccountTests
    {
        [Test]
        public void CanLoadPremiumAccountTestData()
        {
            AccountManager manager = AccountManagerFactory.Create();

            AccountLookupResponse response = manager.LookupAccount("3");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("3", response.Account.AccountNumber);
        }

        [TestCase("3", "Premium Account", 500, AccountType.Basic, -100, false)]
        [TestCase("3", "Premium Account", 1000, AccountType.Premium, -100, false)]
        [TestCase("3", "Premium Account", 500, AccountType.Premium, 250, true)]
        public void PremiumAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit deposit = new NoLimitDepositRule();
            Account account = new Account();

            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountDepositResponse response = deposit.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("3", "Premium Account", 1500, AccountType.Premium, 1000, 1500, false)]
        [TestCase("3", "Premium Account", 100, AccountType.Premium, -600, -500, true)]
        [TestCase("3", "Premium Account", 150, AccountType.Basic, -50, 100, false)]
        [TestCase("3", "Premium Account", 100, AccountType.Premium, -150, -50, true)]
        
        public void PremiumAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdraw = new PremiumAccountWithdrawRule();
            Account account = new Account();

            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Type = accountType;
            account.Balance = balance;

            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);

            if (response.Success == true)
            {
                newBalance = response.Account.Balance;
            }
        }
    }
}
