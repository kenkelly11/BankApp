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
    public class BasicAccountTests
    {
        [TestCase("2", true)]
        public void CanLoadBasicAccountTestData(string name, bool expected)
        {
            AccountManager manager = AccountManagerFactory.Create();
            AccountLookupResponse response = manager.LookupAccount(name);

            //Assert.IsNotNull(response.Account);
            //Assert.IsTrue(response.Success);
            Assert.AreEqual(name, response.Account.AccountNumber);
        }

        // wrong account type
        [TestCase("2", "Basic Account", 100, AccountType.Free, 250, false)]
        // negative number deposited
        [TestCase("2", "Basic Account", 100, AccountType.Basic, -100, false)]
        // success
        [TestCase("2", "Basic Account", 100, AccountType.Basic, 250, true)]
        public void BasicAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
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

        // too much withdrawn
        [TestCase("2", "Basic Account", 1500, AccountType.Basic, -1000, 1500, false)]
        // not a basic account type
        [TestCase("2", "Basic Account", 100, AccountType.Free, -100, 100, false)]
        // positive number withdrawn
        [TestCase("2", "Basic Account", 100, AccountType.Basic, 100, 100, false)]
        // success
        [TestCase("2", "Basic Account", 150, AccountType.Basic, -50, 100, true)]
        // success, overdraft fee
        [TestCase("2", "Basic Account", 100, AccountType.Basic, -150, -60, true)]
        public void BasicAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            // we are testing the methods in BasicAccountWithdrawRule class
            IWithdraw withdraw = new BasicAccountWithdrawRule();
            Account account = new Account();

            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);

            
            if (response.Success == true)
            {
                Assert.AreEqual(newBalance, response.Account.Balance);
            }
            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}
