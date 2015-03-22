using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VendingMachine;

namespace VendingMachine.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var vendingMachine = new VendingMachine();
            var card = new CashCard(300);

            bool run = true;
            while(run)
            {
                System.Console.Title = " Han Wei Teo : CS Test";
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("==========  Welcome To CS Vending Machine  =========");
                System.Console.WriteLine("|       Cans Avail    : {0}                        |", vendingMachine.Inventory);
                System.Console.WriteLine("|       Card Balance  : {0}                       |", card.Balance);
                System.Console.WriteLine("========      [Please Select An Option]    =========");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("========      Press 1 - Buy Can            =========");
                System.Console.WriteLine("========      Press 2 - Top Up Card (100)  =========");
                System.Console.WriteLine("========      Press 3 - Refill Machine     =========");
                System.Console.WriteLine("========      Press 4 - Quit               =========");
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("====================================================");

                var option = System.Console.ReadKey();
                switch(option.KeyChar)
                {
                    case '1':
                        try
                        {
                            vendingMachine.BuyCan(card);
                            System.Console.ForegroundColor = ConsoleColor.Blue;
                            System.Console.WriteLine("=============== Vending......  ==================");
                            Thread.Sleep(500);
                        }
                        catch(InsufficientFundsException)
                        {
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("============= Not enough funds on card!!  ==============");
                            Thread.Sleep(500);
                        }
                        catch(VendingMachineEmptyException)
                        {
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("=============== Out Of Stock!! ================");
                            Thread.Sleep(500);
                        }
                        break;
                    case '2':
                        card.TopUp(100);
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine("=============== Topping Up......  ==================");
                        Thread.Sleep(500);
                        break;
                    case '3':
                        vendingMachine = new VendingMachine();
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine("=============== Refilling ......  ==================");
                        Thread.Sleep(500);
                        break;
                    case '4':
                        run = false;
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("=============== Bye !!  ==================");
                        Thread.Sleep(500);
                        break;
                    default:
                        break;
                }
                System.Console.Clear();
            }
        }
    }
}
