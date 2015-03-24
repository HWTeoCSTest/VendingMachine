using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VendingMachine
{
    public class CashCard
    {
        private int _balance;
		private bool isThreadSafe = true;

		#region Mock Factory
        public static CashCard CreateNonThreadSafeMock(int amount)
        {
            return new CashCard(false, amount);
        }

		private CashCard(bool isThreadSafe, int amount) : this(amount)
		{
			isThreadSafe = isThreadSafe;
		}
		#endregion

        public CashCard(int amount)
        {
            _balance = amount;
        }
			
        public int Balance 
        { 
            get { return Thread.VolatileRead(ref _balance); } 
        }

        public bool HasSufficientFundsFor(int amount)
        {
            return Balance - amount >= 0;
        }

        public void TopUp(int amount)
        {
            if (isThreadSafe)
                Interlocked.Add(ref _balance, amount);
            else
                _balance += amount;
        }

        public void Deduct(int amount)
        {
            if (!HasSufficientFundsFor(amount))
                return;

            if (isThreadSafe)
                Interlocked.Add(ref _balance, -amount);
            else
                _balance -= amount;
        }
    }
}
