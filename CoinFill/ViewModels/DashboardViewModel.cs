using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class DashboardViewModel
    {
        // Crypto Card Properties

        public bool HasCryptoCardData { get; set; } = false;
        public float WeeklyTopUpsAmount { get; set; }
        public float WeeklyTopUpsPercentage { get; set; }
        public string WeeklyTopUpsBadgeClass { get; set; }
        public float WeeklySpendingsAmount { get; set; }
        public float WeeklySpendingsPercentage { get; set; }
        public string WeeklySpendingsBadgeClass { get; set; }
        public float TotalBalanceAmount { get; set; }
        public float TotalBalancePercentage { get; set; }
        public string TotalBalanceBadgeClass { get; set; }
        public string MostUsedIcon { get; set; }
        public string MostUsedName { get; set; }
        public int MostUsedTransactionsNumber { get; set; }
        public string MostUsedBadgeClass { get; set; }
        public string MostUsedIcon2 { get; set; }
        public string MostUsedName2 { get; set; }
        public int MostUsedTransactionsNumber2 { get; set; }
        public string MostUsedBadgeClass2 { get; set; }

        // Staking Properties

        public bool HasStakingData { get; set; } = false;
        public float WeeklyStakingRewardsAmount { get; set; }
        public float WeeklyStakingRewardsPercentage { get; set; }
        public string WeeklyStakingRewardsBadgeClass { get; set; }
        public float TotalStakingRewardsAmount { get; set; }
        public float TotalStakingRewardsPercentage { get; set; }
        public string TotalStakingRewardsBadgeClass { get; set; }
        public string StakingAllocation1CoinName { get; set; }
        public string StakingAllocation1Color { get; set; }
        public string StakingAllocation2CoinName { get; set; }
        public string StakingAllocation2Color { get; set; }
        public string StakingAllocation3CoinName { get; set; }
        public string StakingAllocation3Color { get; set; }
        public float StakingAllocationTotalAmount { get; set; }
        public string TopPerformerIcon { get; set; }
        public string TopPerformerName { get; set; }
        public float TopPerformerPercentage { get; set; }
        public string TopPerformerBadgeClass { get; set; }
        public string TopPerformer2Icon { get; set; }
        public string TopPerformer2Name { get; set; }
        public float TopPerformer2Percentage { get; set; }
        public string TopPerformer2BadgeClass { get; set; }

        public List<CardViewModel> Cards { get; set; }
        public List<BankAccountViewModel> BankAccounts { get; set; }

        public DashboardViewModel()
        {
            Cards = new List<CardViewModel>();
            BankAccounts = new List<BankAccountViewModel>();

            //Cards
            HasCryptoCardData = false;
            WeeklyTopUpsAmount = 0;
            WeeklyTopUpsPercentage = 0;
            WeeklyTopUpsBadgeClass = "badge-soft-primary";
            WeeklySpendingsAmount = 0;
            WeeklySpendingsPercentage = 0;
            WeeklySpendingsBadgeClass = "badge-soft-primary";
            TotalBalanceAmount = 0;
            TotalBalancePercentage = 0;
            TotalBalanceBadgeClass = "badge-soft-primary";
            MostUsedIcon = "fas fa-ban";
            MostUsedName = "No data";
            MostUsedTransactionsNumber = 0;
            MostUsedBadgeClass = "badge-soft-secondary";
            MostUsedIcon2 = "fas fa-ban";
            MostUsedName2 = "No data";
            MostUsedTransactionsNumber2 = 0;
            MostUsedBadgeClass2 = "badge-soft-secondary";
            // Staking
            HasStakingData = false;
            WeeklyStakingRewardsAmount = 0;
            WeeklyStakingRewardsPercentage = 0;
            WeeklyStakingRewardsBadgeClass = "badge-soft-primary";
            TotalStakingRewardsAmount = 0;
            TotalStakingRewardsPercentage = 0;
            TotalStakingRewardsBadgeClass = "badge-soft-primary";
            StakingAllocation1CoinName = "No data";
            StakingAllocation1Color = "#b6c1d2";
            StakingAllocation2CoinName = "No data";
            StakingAllocation2Color = "#b6c1d2";
            StakingAllocation3CoinName = "No data";
            StakingAllocation3Color = "#b6c1d2";
            StakingAllocationTotalAmount = 0;
            TopPerformerIcon = "fas fa-ban";
            TopPerformerName = "No data";
            TopPerformerPercentage = 0;
            TopPerformerBadgeClass = "badge-soft-primary";
            TopPerformer2Icon = "fas fa-ban";
            TopPerformer2Name = "No data";
            TopPerformer2Percentage = 0;
            TopPerformer2BadgeClass = "badge-soft-primary";
        }
    }
}
