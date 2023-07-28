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
    public class FreeAccountTests
    {
        [Test]
        public void CanLoadFreeAccountTestData()
        {
            AccountManager manager = AccountManagerFactory.Create();

            AccountLookupResponse response = manager.LookupAccount("1");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("1", response.Account.AccountNumber);
        }
        // too much deposited
        [TestCase("1", "Free Account", 100, AccountType.Free, 250, false)]
        // negative number deposited
        [TestCase("1", "Free Account", 100, AccountType.Free, -100, false)]
        // not a free account type
        [TestCase("1", "Free Account", 100, AccountType.Basic, 50, false)]
        // success
        [TestCase("1", "Free Account", 100, AccountType.Free, 50, true)]
        public void FreeAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit deposit = new FreeAccountDepositRule();
            Account account = new Account();

            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountDepositResponse response = deposit.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }
        [Test]
        // positive withdrawal amount
        [TestCase("1", "Free Account", 100, AccountType.Free, 100, false)]
        // negative withdrawal over limit
        [TestCase("1", "Free Account", 100, AccountType.Free, -150, false)]
        // wrong account type
        [TestCase("1", "Free Account", 100, AccountType.Basic, -50, false)]
        // overdraft
        [TestCase("1", "Free Account", 50, AccountType.Free, -80, false)]
        // success
        [TestCase("1", "Free Account", 100, AccountType.Free, -50, true)]
        public void FreeAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IWithdraw withdraw = new FreeAccountWithdrawRule();
            Account account = new Account();

            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
