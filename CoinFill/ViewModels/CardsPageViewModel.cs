using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class CardsPageViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        public string CardIdToVerify { get; set; }
        public bool CardWalletAlreadySubmitted { get; set; }
        public bool StartTopTup { get; set; }

        public List<CardViewModel> Cards { get; set; }

        public int SupportedCountriesCount { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        [Display(Name = "Choose the cryptocurrency with which you would like to make the payment", Prompt = "Cryptocurrency payment type")]
        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Chosen_PaymentMethodId { get; set; }

        public List<SelectListItem> ToChooseFrom_PaymentMethods { get; set; }

        public CardsPageViewModel()
        {
            CardIdToVerify = null;
            Cards = new List<CardViewModel>();
            ToChooseFrom_PaymentMethods = new List<SelectListItem>();
        }
    }
}
