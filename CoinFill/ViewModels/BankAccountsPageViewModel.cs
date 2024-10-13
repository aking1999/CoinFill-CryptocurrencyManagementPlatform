using CoinFill.ViewModels.BankAccountTypes;
using System.Collections.Generic;

namespace CoinFill.ViewModels
{
    public class BankAccountsPageViewModel
    {
        public string BankIdToDeposit { get; set; }
        public bool StartDeposit { get; set; }

        public List<BankAccountViewModel> BankAccounts { get; set; }

        public AddBankAccountEur EurAccount { get; set; }
        public AddBankAccountUsd UsdAccount { get; set; }
        public AddBankAccountGbp GbpAccount { get; set; }
        public AddBankAccountChf ChfAccount { get; set; }
        public AddBankAccountCny CnyAccount { get; set; }
        public AddBankAccountHkd HkdAccount { get; set; }
        public AddBankAccountRub RubAccount { get; set; }
        public AddBankAccountIrr IrrAccount { get; set; }
        public AddBankAccountKrw KrwAccount { get; set; }
        public AddBankAccountCad CadAccount { get; set; }
        public AddBankAccountAud AudAccount { get; set; }
        public AddBankAccountNok NokAccount { get; set; }
        public AddBankAccountDkk DkkAccount { get; set; }
        public AddBankAccountSek SekAccount { get; set; }
        public AddBankAccountInr InrAccount { get; set; }
        public AddBankAccountHuf HufAccount { get; set; }
        public AddBankAccountRsd RsdAccount { get; set; }
        public AddBankAccountBam BamAccount { get; set; }
        public AddBankAccountPln PlnAccount { get; set; }
        public AddBankAccountUah UahAccount { get; set; }
        public AddBankAccountCzk CzkAccount { get; set; }
        public AddBankAccountRon RonAccount { get; set; }
        public AddBankAccountBgn BgnAccount { get; set; }
        public AddBankAccountTry TryAccount { get; set; }
        public AddBankAccountAed AedAccount { get; set; }
        public AddBankAccountMxn MxnAccount { get; set; }
        public AddBankAccountIls IlsAccount { get; set; }
        public AddBankAccountPkr PkrAccount { get; set; }
        public AddBankAccountMad MadAccount { get; set; }

        public BankAccountsPageViewModel()
        {
            BankAccounts = new List<BankAccountViewModel>();

            EurAccount = new AddBankAccountEur();
            UsdAccount = new AddBankAccountUsd();
            GbpAccount = new AddBankAccountGbp();
            ChfAccount = new AddBankAccountChf();
            CnyAccount = new AddBankAccountCny();
            HkdAccount = new AddBankAccountHkd();
            RubAccount = new AddBankAccountRub();
            IrrAccount = new AddBankAccountIrr();
            KrwAccount = new AddBankAccountKrw();
            CadAccount = new AddBankAccountCad();
            AudAccount = new AddBankAccountAud();
            NokAccount = new AddBankAccountNok();
            DkkAccount = new AddBankAccountDkk();
            SekAccount = new AddBankAccountSek();
            InrAccount = new AddBankAccountInr();
            HufAccount = new AddBankAccountHuf();
            RsdAccount = new AddBankAccountRsd();
            BamAccount = new AddBankAccountBam();
            PlnAccount = new AddBankAccountPln();
            UahAccount = new AddBankAccountUah();
            CzkAccount = new AddBankAccountCzk();
            RonAccount = new AddBankAccountRon();
            BgnAccount = new AddBankAccountBgn();
            TryAccount = new AddBankAccountTry();
            AedAccount = new AddBankAccountAed();
            MxnAccount = new AddBankAccountMxn();
            IlsAccount = new AddBankAccountIls();
            PkrAccount = new AddBankAccountPkr();
            MadAccount = new AddBankAccountMad();
        }
    }
}
