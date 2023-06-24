namespace BookShopping_Project.Utility
{
    public static class SD
    {

        //Roles

        public const string Role_User_Admin = "Admin";
        public const string Role_User_Employee = "Employee";
        public const string Role_User_Company = "CompanyUser";
        public const string Role_User_Individual = "IndividualUser";

        //Session
        public const string Ss_Session = "My Session";


        //order status
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProgress = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        //payment Status

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDelayPayment";
        public const string PaymentStatusRejected = "Rejected";


        public static double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price50;
            else
                return price100;
        }

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool Inside = false;
            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    Inside = true;
                    continue;
                }
                if (let == '>')
                {
                    Inside = false;
                    continue;
                }
                if (!Inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
