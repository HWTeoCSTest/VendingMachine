using System;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace VendingMachine
{
    public class VendingMachine
    {
        public const int PriceOfCan = 50;
		private const int Capacity = 25;
        private int _inventory;
		private bool isThreadSafe = true;

		#region Mock Factory
        public static VendingMachine CreateMock(int customCapacity)
        {
            return new VendingMachine(customCapacity);
        }

        public static VendingMachine CreateNonThreadSafeMock(int customCapcity)
        {
            return new VendingMachine(false, customCapcity);
        }

        private VendingMachine(int capacity)
        {
            _inventory = capacity;
        }

        private VendingMachine(bool isThreadSafe, int capacity) : this(capacity)
        {
            isThreadSafe = isThreadSafe;
        }
		#endregion

		public VendingMachine()
		{
			_inventory = Capacity;
		}

        public void BuyCan(CashCard cashCard)
        {
            if (isEmpty())
                throw new VendingMachineEmptyException();

            if (!cashCard.HasSufficientFundsFor(PriceOfCan))
                throw new InsufficientFundsException();

            cashCard.Deduct(PriceOfCan);

            if (isThreadSafe)
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
