using System;
using BankAccountNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetAccountTypeTest()
        {
            string customerName = "Mr. Bryan Walton";
            double balance = 15.01;
            BankAccount target = new BankAccount(customerName, balance);
            BankAccount.accountType actual;
            actual = target.GetAccountType;
            Assert.AreEqual(actual, BankAccount.accountType.Platinum);
        }

        [TestMethod()]
        public void GetAccountTypeTest1()
        {
            string customerName = "Mr. Bryan Walton";
            double balance = 14.99;
            BankAccount target = new BankAccount(customerName, balance);
            BankAccount.accountType actual;
            actual = target.GetAccountType;
            Assert.AreEqual(actual, BankAccount.accountType.Gold);
        }
    }
}
