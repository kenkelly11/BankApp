using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using SGBank.Data;
using SGBank.Models;

namespace SGBankTests
{
    [TestFixture]
    public class FileAccountTests
    {
        private const string _filepath = @"C:\Data\SGBank\SGBankAccounts.txt";
        private const string _path = @"C:\Data\SGBank\SGBankAccountsData.txt";
        [SetUp]
        public void Setup()
        {
            if (File.Exists(_filepath))
            {
                File.Delete(_filepath);
            }
            File.Copy(_path, _filepath);
        }

        [Test]
        public void CanReadData()
        {
            FileAccountRepository repo = new FileAccountRepository(_filepath);

            List<Account> accounts = repo.ListAccounts();

            Assert.AreEqual(3, accounts.Count());

            Account verify = accounts[0];

            Assert.AreEqual("1", verify.AccountNumber);
            Assert.AreEqual("Free Customer", verify.Name);
            Assert.AreEqual(100, verify.Balance);
            Assert.AreEqual(AccountType.Free, verify.Type);
        }

        [Test]
        public void CanAddandUpdateAccount()
        {
            FileAccountRepository repo = new FileAccountRepository(_filepath);

            List<Account> accounts = repo.ListAccounts();

            Account newAccount = new Account();

            newAccount.AccountNumber = "2";
            newAccount.Name = "Basic Customer";
            newAccount.Balance = 500;
            newAccount.Type = AccountType.Basic;

            accounts.Add(newAccount);

            Assert.AreEqual(4, accounts.Count());

            Account verify = accounts[3];

            Assert.AreEqual("2", verify.AccountNumber);
            Assert.AreEqual("Basic Customer", verify.Name);
            Assert.AreEqual(500, verify.Balance);
            Assert.AreEqual(AccountType.Basic, verify.Type);
        }

    }
}
