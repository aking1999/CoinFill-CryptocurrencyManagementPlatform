namespace CoinFill.ViewModels
{
    public class BankAccountViewModel
    {
        public string AccountType { get; set; }
        public string Id { get; set; }
        public string IdLabel { get; set; }
        public string Fee { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Currency { get; set; }
        public string BankAccountNumber { get; set; }
        public string BicSwift { get; set; }
        public string RoutingNumber { get; set; }
        public string TransitNumber { get; set; }
        public string InstitutionNumber { get; set; }

        public string Ifsc { get; set; } //for INR transfers

        public BankAccountViewModel()
        {
            IdLabel = "Bank";
        }
    }
}
