using System;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace VendingMachine
{
    public class VendingMachine
    {
        public const int _priceOfCan = 50;
        private const int _capacity = 25;
        private int _inventory;
        private bool _isThreadSafe = true;

        public static VendingMachine CreateMock(int customCapacity)
        {
            return new VendingMachine(customCapacity);
        }

        public static VendingMachine CreateNonThreadSafeMock(int customCapcity)
        {
            return new VendingMachine(false, customCapcity);
        }

        public VendingMachine()
        {
            _inventory = _capacity;
        }

        private VendingMachine(int capacity)
        {
            _inventory = capacity;
        }

        private VendingMachine(bool isThreadSafe, int capacity) : this(capacity)
        {
            _isThreadSafe = isThreadSafe;
        }

        public void BuyCan(CashCard cashCard)
        {
            if (isEmpty())
                throw new VendingMachineEmptyException();

            if (!cashCard.HasSufficientFundsFor(_priceOfCan))
                throw new InsufficientFundsException();

            cashCard.Deduct(_priceOfCan);
            if (_isThreadSafe)
                Interlocked.Decrement(ref _inventory);
            else
                _inventory--;
        }

        private bool isEmpty()
        {
            return Inventory == 0;
        }

        public int Inventory { get { return Volatile.Read(ref _inventory); } }
    }
}
