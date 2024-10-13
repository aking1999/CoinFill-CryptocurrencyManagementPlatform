using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.PartialViewModels
{
    public class CaptureTransaction
    {
        private const string REQUIRED_FIELD = "Field is required.";

        public string Id { get; set; }

        public string AlertMessage { get; set; }

        public string HeaderText { get; set; }

        public string  DropdownPlaceholderText { get; set; }

        public string ChosenTransactionIdLabel { get; set; }

        public bool IsBank { get; set; } = false;

        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Chosen_TransactionMethodId { get; set; }

        public List<SelectListItem> ToChooseFrom_TransactionMethods { get; set; }

        public CaptureTransaction()
        {
            ToChooseFrom_TransactionMethods = new List<SelectListItem>();
        }
    }
}
