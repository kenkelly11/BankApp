using SGBank.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank
{
    public class Menu
    {
        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("SG Bank Application");
            Console.WriteLine("--------------------");
            Console.WriteLine("1. Lookup An Account");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("\nQ to Quit");
            Console.Write("\nEnter selection: ");
        }
        public static bool ProcessChoice()
        {
            string userinput = Console.ReadLine();

            switch (userinput)
            {
                case "1":
                    AccountLookupWorkflow lookupWorkflow = new AccountLookupWorkflow();
                    lookupWorkflow.Execute();
                    break;
                case "2":
                    DepositWorkflow depositWorkflow = new DepositWorkflow();
                    depositWorkflow.Execute();
                    break;
                case "3":
                    WithdrawWorkflow withdrawWorkflow = new WithdrawWorkflow();
                    withdrawWorkflow.Execute();
                    break;
                case "Q":
                    return false;
                default:
                    Console.WriteLine("That is not a valid choice. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
            return true;
        }
        // loops the menu until the user decides to quit
        public static void Show()
        {
            bool keepRunning = true;
            while (keepRunning)
            {
                Start();
                keepRunning = ProcessChoice();
            }
        }
    }
}
