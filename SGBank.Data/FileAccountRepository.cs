using System.IO;
using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Data
{
    public class FileAccountRepository : IAccountRepository
    {
        private string _filePath;

        public FileAccountRepository(string filePath)
        {
            _filePath = filePath;
        }

        public FileAccountRepository()
        {

        }

        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (StreamReader sr = new StreamReader(_filePath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // create an object,
                    // split the line,
                    // assign data to object members,
                    // add the object to the list
                    Account newAccount = new Account();

                    string[] columns = line.Split(',');

                    newAccount.AccountNumber = columns[0];
                    newAccount.Name = columns[1];
                    newAccount.Balance = decimal.Parse(columns[2]);
                    newAccount.Type = EnumTypeConverter(columns[3]);

                    accounts.Add(newAccount);
                }
            }
            return accounts;
        }
        
        public AccountType EnumTypeConverter(string accountType)
        {
            switch (accountType)
            {
                case "P":
                    return AccountType.Premium;
                case "B":
                    return AccountType.Basic;
                case "F":
                    return AccountType.Free;
                default:
                    throw new Exception("Account Type invalid. Please try again!");
            }
        }
        
        // finds the account associated with the chosen account number
        public Account LoadAccount(string AccountNumber)
        {
            var account = ListAccounts();

            var findAccount = account.Find(x => x.AccountNumber == AccountNumber);

            return findAccount;
        }

        public void SaveAccount(Account account)
        {
            var accounts = ListAccounts();

            var indexAccount = accounts.FindIndex(s => s.AccountNumber == account.AccountNumber);

            accounts[indexAccount] = account;

            CreateAccountFile(accounts);
        }

        private string CreateCsvforAccount(Account account)
        {
            string accountType;
            switch (account.Type)
            {
                case AccountType.Basic:
                    accountType = "B";
                    break;
                case AccountType.Free:
                    accountType = "F";
                    break;
                case AccountType.Premium:
                    accountType = "P";
                    break;
                default:
                    throw new Exception("Account type invalid. Please try again!");
            }
            return string.Format("{0},{1},{2},{3}", account.AccountNumber, account.Name, account.Balance.ToString(), accountType);
        }

        // overwrites and edits the file
        private void CreateAccountFile(List<Account> account)
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);

            using (StreamWriter reader = new StreamWriter(_filePath))
            {
                reader.WriteLine("AccountNumber, Name, Balance, AccountType");
                foreach (var acct in account)
                {
                    reader.WriteLine(CreateCsvforAccount(acct));
                }
            }
        }
    }
}
