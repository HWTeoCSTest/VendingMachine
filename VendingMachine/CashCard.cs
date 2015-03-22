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
        private bool _isThreadSafe = true;

        public static CashCard CreateNonThreadSafeMock(int amount)
        {
            return new CashCard(false, amount);
        }

        public CashCard(int amount)
        {
            _balance = amount;
        }

        private CashCard(bool isThreadSafe, int amount) : this(amount)
        {
            _isThreadSafe = isThreadSafe;
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
            if (_isThreadSafe)
                Interlocked.Add(ref _balance, amount);
            else
                _balance += amount;
        }

        public void Deduct(int amount)
        {
            if (!HasSufficientFundsFor(amount))
                return;

            if (_isThreadSafe)
                Interlocked.Add(ref _balance, -amount);
            else
                _balance -= amount;
        }
    }
}
