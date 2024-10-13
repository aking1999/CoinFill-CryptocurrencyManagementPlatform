using CoinFill.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.PartialViewModels
{
    public class CaptureStakingDepositTransaction
    {
        private const string REQUIRED_FIELD = "Field is required.";

        public string AlertMessage { get; set; }
        public string HeaderText { get; set; }
        public string DropdownPlaceholderText { get; set; }
        public string ChosenNetworkIdLabel { get; set; }

        public ValidatorsCryptocurrencies ValidatorCryptocurrency { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Chosen_NetworkId { get; set; }

        public List<SelectListItem> ToChooseFrom_Networks { get; set; }

        public CaptureStakingDepositTransaction(ValidatorsCryptocurrencies validatorCryptocurrency)
        {
            validatorCryptocurrency ??= new ValidatorsCryptocurrencies();
            ValidatorCryptocurrency = validatorCryptocurrency;
            ValidatorCryptocurrency.Validator = validatorCryptocurrency.Validator;
            ValidatorCryptocurrency.Cryptocurrency = validatorCryptocurrency.Cryptocurrency;

            ToChooseFrom_Networks = new List<SelectListItem>();
        }
    }
}
