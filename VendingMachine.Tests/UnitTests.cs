using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace VendingMachine.Tests
{
    [TestFixture]
    public class UnitTests
    {
		private VendingMachine vendingMachine;
		private CashCard cashCard, emptyCard;

        [SetUp]
        public void Setup()
        {
            vendingMachine = new VendingMachine();
            cashCard = new CashCard(10000);
            emptyCard = new CashCard(0);
        }

        [Test]
        public void VendingMachineHasAnInitialInventoryOf25()
        {
            Assert.AreEqual(25, vendingMachine.Inventory);
        }

        [Test]
        public void VendingMachineCanVendUpTo25Cans()
        {
            foreach (var i in Enumerable.Range(0, 25))
                vendingMachine.BuyCan(cashCard);
        }

        [Test, ExpectedException(typeof(VendingMachineEmptyException))]
        public void VendingMachineCannotVendMoreThan25Cans()
        {
            foreach(var i in Enumerable.Range(0, 26))
                vendingMachine.BuyCan(cashCard);
        }

        [Test]
        public void VendingMachineHasBuyCanMethodWhichTakesCashCard()
        {
            vendingMachine.BuyCan(cashCard);
        }

        [Test]
        public void VendingMachineInventoryDecreasesWhenCanIsBought()
        {
            var initial = vendingMachine.Inventory;
            vendingMachine.BuyCan(cashCard);
            Assert.AreEqual(initial - 1, vendingMachine.Inventory);
        }

        [Test]
        public void WhenIBuyACanFromVendingMachine50pIsDeductedFromMyCashCardBalance()
        {
            var initial = cashCard.Balance;
            vendingMachine.BuyCan(cashCard);
            Assert.AreEqual(initial - 50, cashCard.Balance);
        }

        [Test, ExpectedException(typeof(InsufficientFundsException))]
        public void VendingMachineWillNotVendIfCardDoesNotHaveSufficientFunds()
        {
            vendingMachine.BuyCan(emptyCard);
        }

        [Test]
        public void CardDeductMethodIsThreadSafe()
        {
            var card = new CashCard(100000);
            Action<int> deduct = x => { card.Deduct(10); };
            Parallel.For(0, 10000, deduct);
            Assert.AreEqual(0, card.Balance);
        }

        [Test]
        public void CardTopUpIsThreadSafe()
        {
            var card = new CashCard(0);
            Action<int> deduct = x => { card.TopUp(10); };
            Parallel.For(0, 10000, deduct);
            Assert.AreEqual(100000, card.Balance);
        }

        [Test]
        public void VendingMachineBuyCanIsThreadSafe()
        {
            var _card = new CashCard(500000);
            vendingMachine = VendingMachine.CreateMock(10000);
            Action<int> buy = x => { vendingMachine.BuyCan(_card); };
            Parallel.For(0, 10000, buy);
            Assert.AreEqual(0, vendingMachine.Inventory);
        }

        [Test]
        public void CardDeductMethodNonThreadSafe()
        {
            var card = CashCard.CreateNonThreadSafeMock(100000);
            Action<int> deduct = x => { card.Deduct(10); };
            Parallel.For(0, 10000, deduct);
            Assert.AreNotEqual(0, card.Balance);
        }

        [Test]
        public void CardTopUpNonThreadSafe()
        {
            var card = CashCard.CreateNonThreadSafeMock(0);
            Action<int> deduct = x => { card.TopUp(10); };
            Parallel.For(0, 10000, deduct);
            Assert.AreNotEqual(100000, card.Balance);
        }

        [Test]
        public void VendingMachineBuyCanNonThreadSafe()
        {
            var _card = new CashCard(500000);
            vendingMachine = VendingMachine.CreateNonThreadSafeMock(10000);
            Action<int> buy = x => { vendingMachine.BuyCan(_card); };
            Parallel.For(0, 10000, buy);
            Assert.AreNotEqual(0, vendingMachine.Inventory);
        }
    }
}
