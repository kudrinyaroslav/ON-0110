namespace BankAccountNS
{
    /// <summary> 
    /// Bank Account demo class. 
    /// </summary> 
    public class BankAccount
    {
        public enum accountType
        {
            Gold,
            Platinum,
        }

        public BankAccount(string customerName, double balance)
        {
            m_customerName = customerName;
            m_balance = balance;
        }

        public string CustomerName
        {
            get { return m_customerName; }
        }

        public double Balance
        {
            get { return m_balance; }
        }

        public accountType GetAccountType
        {
            get
            {
                if (Balance > 15.00)
                    return accountType.Platinum;
                else
                    return accountType.Gold;
            }
        }
        private string m_customerName;
        private double m_balance;
    }
}
