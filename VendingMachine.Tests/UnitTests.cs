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
        private VendingMachine _vendingMachine;
        private CashCard _cashCard, _emptyCard;

        [SetUp]
        public void Setup()
        {
            _vendingMachine = new VendingMachine();
            _cashCard = new CashCard(10000);
            _emptyCard = new CashCard(0);
        }

        [Test]
        public void VendingMachineHasAnInitialInventoryOf25()
        {
            Assert.AreEqual(25, _vendingMachine.Inventory);
        }

        [Test]
        public void VendingMachineCanVendUpTo25Cans()
        {
            foreach (var i in Enumerable.Range(0, 25))
                _vendingMachine.BuyCan(_cashCard);
        }

        [Test, ExpectedException(typeof(VendingMachineEmptyException))]
        public void VendingMachineCannotVendMoreThan25Cans()
        {
            foreach(var i in Enumerable.Range(0, 26))
                _vendingMachine.BuyCan(_cashCard);
        }

        [Test]
        public void VendingMachineHasBuyCanMethodWhichTakesCashCard()
        {
            _vendingMachine.BuyCan(_cashCard);
        }

        [Test]
        public void VendingMachineInventoryDecreasesWhenCanIsBought()
        {
            var initial = _vendingMachine.Inventory;
            _vendingMachine.BuyCan(_cashCard);
            Assert.AreEqual(initial - 1, _vendingMachine.Inventory);
        }

        [Test]
        public void WhenIBuyACanFromVendingMachine50pIsDeductedFromMyCashCardBalance()
        {
            var initial = _cashCard.Balance;
            _vendingMachine.BuyCan(_cashCard);
            Assert.AreEqual(initial - 50, _cashCard.Balance);
        }

        [Test, ExpectedException(typeof(InsufficientFundsException))]
        public void VendingMachineWillNotVendIfCardDoesNotHaveSufficientFunds()
        {
            _vendingMachine.BuyCan(_emptyCard);
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
            _vendingMachine = VendingMachine.CreateMock(10000);
            Action<int> buy = x => { _vendingMachine.BuyCan(_card); };
            Parallel.For(0, 10000, buy);
            Assert.AreEqual(0, _vendingMachine.Inventory);
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
            _vendingMachine = VendingMachine.CreateNonThreadSafeMock(10000);
            Action<int> buy = x => { _vendingMachine.BuyCan(_card); };
            Parallel.For(0, 10000, buy);
            Assert.AreNotEqual(0, _vendingMachine.Inventory);
        }
    }
}
