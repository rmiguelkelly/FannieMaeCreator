using FNMBuilder;
using System;
using System.Collections.Generic;

namespace FannieMaeCreator
{
    class Program
    {
        static void Main(string[] args)
        {

            var contact = new Contact();
            contact.Firstname = "Ronan";
            contact.Middlename = "Miguel";
            contact.Lastname = "Kelly";
            contact.Email = "thekelpez@gmail.com";
            contact.Birthday = new DateTime(1997, 9, 12);
            contact.HomePhoneNumber = "9543031004";
            contact.SocialSecurity = "999999999";
            contact.Ethnicity = Ethnicity.HispanicOrLatino;
            contact.Sex = Sex.Male;
            contact.Race = Race.White;

            var loaninfo = new LoanInformation()
            {
                AmortizationType = AmortizationType.FixedRate,
                InterestRate = 5.32,
                LoanAmount = 525000,
                NoOfMonths = 360,
                LoanType = LoanType.Purchase
            };

            var address = new PropertyAddress();
            address.State = "FL";
            address.City = "Hollywood";
            address.ZipCode = "33020";
            address.Street = "1549 Shoreline Way";

            var propinfo = new PropertyInfo();
            propinfo.Address = address;
            propinfo.Description = "My beautiful house";
            propinfo.NoOfUnits = 1;
            propinfo.Value = 1100000;
            propinfo.YearBuilt = 2002;
            propinfo.PropertyReason = PropertyReason.PrimaryResidence;

            var downpayment = new DownPayment();
            downpayment.DownPaymentAmount = 50000;
            downpayment.DownPaymentExplaination = "MY STOCKSS!!!!";
            downpayment.Source = DownPaymentSource.CheckingSavings;

            var myApp = new ApplicationCreator();
            myApp.Borrower = contact;
            myApp.LoanInformation = loaninfo;
            myApp.PropertyInformation = propinfo;
            myApp.DownPayment = downpayment;

            myApp.Create();
            Console.WriteLine(myApp);
        }
    }
}

namespace FNMBuilder
{
    /// <summary>
    /// Represents a borrower's basic information as well as a co-borrower if there is any
    /// </summary>
    class Contact
    {
        public string Firstname = "";
        public string Middlename = "";
        public string Lastname = "";
        public string Generation = "";
        public DateTime Birthday = DateTime.Now;
        public string HomePhoneNumber = "";
        public string CellPhoneNumber = "";
        public string WorkPhoneNumber = "";
        public string Email = "";
        public string SocialSecurity = "";
        public MaritalStatus MaritalStatus = MaritalStatus.Single;
        public Race Race = Race.DoNotWishToProvide;
        public Sex Sex = Sex.DoNotWishToProvide;
        public Ethnicity Ethnicity = Ethnicity.DoNotWishToProvide;
        public int YearsInSchool = 0;
        public List<Dependent> Dependents = new List<Dependent>();
        public List<EmployerInfo> EmploymentInfo = new List<EmployerInfo>();
        public List<ResidenceHistory> ResidenceHistories = new List<ResidenceHistory>();
    }


    /// <summary>
    /// Address information of a property
    /// </summary>
    class PropertyAddress
    {
        public string Street;
        public string City;
        public string State;
        public string ZipCode;
        public string Unit = "";
    }


    /// <summary>
    /// Property information such as year built and price
    /// </summary>
    class PropertyInfo
    {
        public PropertyAddress Address;
        public int YearBuilt;
        public int NoOfUnits;
        public double Value;
        public string Description;
        public PropertyReason PropertyReason;
        public string TitleManner = "";
    }


    /// <summary>
    /// The reason for the property
    /// </summary>
    enum PropertyReason
    {
        PrimaryResidence,
        SecondaryResidence,
        Investment
    }


    /// <summary>
    /// Information regarding the requested loan
    /// </summary>
    class LoanInformation
    {
        public double LoanAmount;
        public double InterestRate;
        public int NoOfMonths = 360;
        public AmortizationType AmortizationType;
        public string AmortizationTypeExplained = "";
        public LoanType LoanType;
    }


    /// <summary>
    /// Is the loan for a Refinance, Purchase, etc...
    /// </summary>
    enum LoanType
    {
        Construction,
        Refinance,
        ConstructionPerm,
        Other,
        Purchase
    }


    /// <summary>
    /// Fixed Rate, Adjustable, etc...
    /// </summary>
    enum AmortizationType
    {
        AdjustableRate,
        GEM,
        FixedRate,
        GPM,
        Other
    }


    /// <summary>
    /// Represents a dependent
    /// </summary>
    /// 
    class Dependent
    {
        public int Age;
        public Dependent(int Age)
        {
            this.Age = Age;
        }
    }


    /// <summary>
    /// Property Type
    /// </summary>
    enum ProperyType
    {
        SingleFamily,
        Condominium,
        Townhouse,
        Cooperative,
        TwoToFourUnitProperty,
        MultifamilyMoreThan4Units,
        ManufacturedMobileHome,
        CommercialNonResidential,
        MixedUseResidential,
        Farm,
        HomeandBusinessCombined,
        Land
    }


    /// <summary>
    /// Primary Income information
    /// </summary>
    enum PrimaryIncome
    {
        MilitaryBasePay,
        MilitaryRationsAllowance,
        MilitaryFlightPay,
        MilitaryHazardPay,
        MilitaryClothesAllowance,
        MilitaryQuartersAllowance,
        MilitaryPropPay,
        MilitaryOverseasPay,
        MilitaryCombatPay,
        MilitaryVariableHousingAllowance,
        AlimonyChildSupportIncome,
        NotesReceivableInstallment,
        PensionRetirementIncome,
        SocialSecurityDisabilityIncome,
        RealEstateMortgageDifferential,
        TrustIncome,
        AccessoryUnitIncome,
        UnemploymentPublicAssistance,
        AutomobileExpenseAccountIncome,
        FosterCare,
        VABenefitsNonEducation,
        NonBorrowerHouseholdIncome,
        OtherIncome,
        BaseEmploymentIncome,
        Overtime,
        Bonuses,
        Commissions,
        DividendsInterest,
        SubjectPropertyNetCashFlow,
        HousingChoiceVoucherSec8,
        NetRentalIncome,
        BoarderIncome,
        MortgageCreditCertificate,
        CapitalGains,
        EmploymentRelatedAssets,
        ForeignIncome,
        RoyaltyPayment,
        SeasonalIncome,
        TemporaryLeave,
        TipIncome,
    }


    /// <summary>
    /// Martial Status values
    /// </summary>
    enum MaritalStatus
    {
        Married,
        Seperated,
        Single
    }


    /// <summary>
    /// Citizen Status values
    /// </summary>
    enum CitizenStatus
    {
        USCitizen,
        PermanentResidentAlien,
        NonPermanentResidentAlien
    }


    /// <summary>
    /// Simple Yes or No 
    /// </summary>
    enum YesNo
    {
        Yes, No
    }


    /// <summary>
    /// Yes No and Unknown values
    /// </summary>
    enum YesNoUnknown
    {
        Yes, No, Unknown
    }


    /// <summary>
    /// Accepted Ethnicity Values
    /// </summary>
    enum Ethnicity
    {
        DoNotWishToProvide,
        HispanicOrLatino,
        NotHispanicOrLatino
    }


    /// <summary>
    /// Accepted Race Values
    /// </summary>
    enum Race
    {
        DoNotWishToProvide,
        AmericanIndianOrAlaskaNative,
        Asian,
        BlackOrAfricanAmerican,
        NativeHawaiianOrOtherPacificIslander,
        White
    }


    /// <summary>
    /// Accepted Sex Values
    /// </summary>
    enum Sex
    {
        DoNotWishToProvide,
        Female,
        Male
    }


    /// <summary>
    /// Declarations of debt, court data, etc... 
    /// All default to 'false' with the exception of
    /// the 'PrimaryResidence' field which is 'true' by default
    /// </summary>
    class Declarations
    {
        public bool PrimaryResidence = true;
        public bool OutstandingJudgments = false;
        public bool DeclaredBankrupcy = false;
        public bool PropertyForclosed = false;
        public bool PartyToLawsuit = false;
        public bool ObligatedOnLoan = false;
        public bool DelinquentOnFederalDebt = false;
        public bool MaintenanceObligations = false;
        public bool AnyDownPaymentBorrowed = false;
        public bool ComakerOrEndorserOnNote = false;
    }


    /// <summary>
    /// Generic employer information
    /// </summary>
    class EmployerInfo
    {
        public string EmployerName = "";
        public PropertyAddress EmployerAddress;
        public bool SelfEmployed = false;
        public bool CurrentEmployed = false;
        public DateTime Start = DateTime.Now;
        public DateTime End = DateTime.Now;
        public double MonthlyIncome = 0;
        public string Position = "";
        public string BusinessPhone = "";
    }


    /// <summary>
    /// Information for if the loan is for construction/refinance purposes
    /// </summary>
    class RefinanceAndConstructionInfo
    {
        public int YearAquired;
        public double OriginalCost;
        public double ExistingLiens;
        public double MarketValueOfLot;
        public double CostOfImprovements;
        public RefinancePurpose RefinancePurpose = RefinancePurpose.NoCashOut;
        public string ImprovementsDescription = "";
        public double Cost;
    }


    /// <summary>
    /// Refinance purpose values
    /// </summary>
    enum RefinancePurpose
    {
        NoCashOut,
        CashOutOther,
        CashOutHomeImprovement,
        CashOutDebtConsolidation,
        LimitedCashOut
    }


    /// <summary>
    /// Data for the down payment 
    /// </summary>
    class DownPayment
    {
        public DownPaymentSource Source;
        public double DownPaymentAmount;
        public string DownPaymentExplaination = "";
    }


    /// <summary>
    /// Accepted down payment sources
    /// </summary>
    enum DownPaymentSource
    {
        CheckingSavings,
        DepositonSalesContract,
        EquitySoldonProperty,
        EquityPendingfromSale,
        EquityPendingfromSubjectProperty,
        GiftFunds,
        Stocksandbonds,
        LotEquity,
        BridgeLoan,
        UnsecuredBorrowedFunds,
        TrustFunds,
        RetirementFunds,
        Rentwithoptiontopurchase,
        Lifeinsurancecashvalue,
        SaleofChattel,
        TradeEquity,
        SweatEquity,
        CashonHand,
        Other,
        SecuredBorrowedFunds,
        FHAGiftSourceNA,
        FHAGiftSourceRelative,
        FHAGiftSourceGovernmentAssistance,
        FHAGiftSourceEmployer,
        FHAGiftSourceNonprofitReligiousCommunitySellerFunded,
        FHAGiftSourceNonprofitReligiousCommunityNonSeller
    }


    /// <summary>
    /// Represents information regarding a property that the borrower/coborrower 
    /// has either lived in, currently living in, or a mailing address
    /// </summary>
    class ResidenceHistory
    {
        public PropertyAddress Address;
        public int NumberOfYears;
        /// <summary>
        /// Only 1 - 11 are valid
        /// </summary>
        public int NumberOfMonths;
        /// <summary>
        /// Only acceptable if the address is a mailing address
        /// </summary>
        public string Country;

        public ResidenceType ResidenceType;

        public ResidenceSituation ResidenceSituation;
    }


    /// <summary>
    /// Accepted residence types
    /// </summary>
    enum ResidenceType
    {
        FormerResidence,
        PresnetAddress,
        MailingAddress
    }


    /// <summary>
    /// Accepted residence situations
    /// </summary>
    enum ResidenceSituation
    {
        LivingRentFree,
        Rent,
        Own
    }
}
