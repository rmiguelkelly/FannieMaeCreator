using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;

namespace FNMBuilder
{
    class ApplicationCreator
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private readonly StringBuilder raw = new StringBuilder();

        /// <summary>
        /// The primary borrower/applicant which must be assigned a value
        /// </summary>
        public Contact Borrower;

        /// <summary>
        /// The Co-applicant of the loan request which can be null (default) if there is none
        /// </summary>
        public Contact? CoBorrower = null;

        /// <summary>
        /// Indicates whether the applicant /co-applicant completed 
        /// the application jointly with the associated applicant/co-applicant.
        /// </summary>
        public bool IsCompletedJointly = false;

        /// <summary>
        /// Social Security Number of applicant /co-applicant whose
        /// Assets/ Liabilities are reported jointly.
        /// REQUIRED if a Co-Applicant exists
        /// </summary>
        public Contact CrossReferenceContact = null;

        /// <summary>
        /// Mandatory class which gives the builder information regarding the loan 
        /// </summary>
        public LoanInformation LoanInformation;

        /// <summary>
        /// Information regarding the property which the borrower is looking to buy/refinance/construct, etc...
        /// </summary>
        public PropertyInfo PropertyInformation;

        /// <summary>
        /// Must be filled out if the LoanInformation value for LoanType is set to construction or refinance
        /// </summary>
        public RefinanceAndConstructionInfo RefinanceAndConstructionInfo = null;

        /// <summary>
        /// General information regarding the down payment 
        /// </summary>
        public DownPayment DownPayment;

        //Gotta love generics...
        private List<KeyValuePair<string, List<KeyValuePair<int, string>>>> _form = new List<KeyValuePair<string, List<KeyValuePair<int, string>>>>();

        private StreamWriter _writer = null;

        public ApplicationCreator()
        {
            
        }

        public void Create()
        {
            //Envelope Header
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("EH", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(6, ""),
                new KeyValuePair<int, string>(25, ""),
                new KeyValuePair<int, string>(11, DateTime.Now.ToString("yyyyMMdd")),
                new KeyValuePair<int, string>(9, "")
            }));

            //Transaction Header
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("TH", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(11, "T100099-002"),
                new KeyValuePair<int, string>(9, ""),
            }));

            //Transaction Processing Info
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("TPI", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(5, "1.00"),
                new KeyValuePair<int, string>(2, "01"),
                new KeyValuePair<int, string>(30, ""),
                new KeyValuePair<int, string>(1, "N")
            }));

            //File Identification
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("000", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(3, "1"),
                new KeyValuePair<int, string>(5, "3.20"),
                new KeyValuePair<int, string>(1, "W"),
            }));

            //Top of form
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("00A", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(1, "N"),
                new KeyValuePair<int, string>(1, "N"),
            }));

            //Mortgage types and terms
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("01A", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(2, "01"),
                new KeyValuePair<int, string>(80, ""),
                new KeyValuePair<int, string>(30, ""),
                new KeyValuePair<int, string>(15, ""),
                new KeyValuePair<int, string>(15, LoanInformation.LoanAmount.ToString("0.##")),
                new KeyValuePair<int, string>(7, LoanInformation.InterestRate.ToString("0.##")),
                new KeyValuePair<int, string>(3, LoanInformation.NoOfMonths.ToString()),
                new KeyValuePair<int, string>(2, new string[] { "01", "04", "05", "06", "13" }[(int)LoanInformation.AmortizationType]),
                new KeyValuePair<int, string>(80, ""),
                new KeyValuePair<int, string>(80, ""),
            }));

            //Property information
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02A", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(50, PropertyInformation.Address.Street),
                new KeyValuePair<int, string>(35, PropertyInformation.Address.City),
                new KeyValuePair<int, string>(2, PropertyInformation.Address.State),
                new KeyValuePair<int, string>(5, PropertyInformation.Address.ZipCode),
                new KeyValuePair<int, string>(4, ""),
                new KeyValuePair<int, string>(3, PropertyInformation.NoOfUnits.ToString()),
                new KeyValuePair<int, string>(2, "02"),
                new KeyValuePair<int, string>(80, PropertyInformation.Description),
                new KeyValuePair<int, string>(4, PropertyInformation.YearBuilt.ToString()),
            }));

            //Purpose of loan
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02B", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(2, ""),
                new KeyValuePair<int, string>(2, "04,05,13,15,16".Split(',')[(int)LoanInformation.LoanType]),
                new KeyValuePair<int, string>(80, ""),
                new KeyValuePair<int, string>(1, "1,2,D".Split(',')[(int)PropertyInformation.PropertyReason]),
                new KeyValuePair<int, string>(60, PropertyInformation.TitleManner),
                new KeyValuePair<int, string>(1, "1"),
                new KeyValuePair<int, string>(8, "")
            }));

            //Title holder
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02C", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(60, Borrower.Firstname + " " + Borrower.Lastname)
            }));

            //If there is a co-borrower, add them to the title
            if (CoBorrower != null)
            {
                _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02C", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(60, CoBorrower.Firstname + " " + CoBorrower.Lastname)
            }));
            }

            //Construction and refinance data, throws an error if there is none provided
            if (LoanInformation.LoanType == LoanType.Construction || LoanInformation.LoanType == LoanType.Refinance)
            {
                if (RefinanceAndConstructionInfo == null)
                {
                    throw new NullReferenceException("Refinance/Construction information must be supplied");
                }

                _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02D", new List<KeyValuePair<int, string>>() {
                    new KeyValuePair<int, string>(4, RefinanceAndConstructionInfo.YearAquired.ToString()),
                    new KeyValuePair<int, string>(15, RefinanceAndConstructionInfo.OriginalCost.ToString("0.##")),
                    new KeyValuePair<int, string>(15, RefinanceAndConstructionInfo.ExistingLiens.ToString("0.##")),
                    new KeyValuePair<int, string>(15, LoanInformation.LoanType == LoanType.Construction ? RefinanceAndConstructionInfo.MarketValueOfLot.ToString("0.##") : ""),
                    new KeyValuePair<int, string>(15, RefinanceAndConstructionInfo.CostOfImprovements.ToString("0.##")),
                    new KeyValuePair<int, string>(2, "F1,01,04,11,13".Split(',')[(int)RefinanceAndConstructionInfo.RefinancePurpose]),
                    new KeyValuePair<int, string>(80, RefinanceAndConstructionInfo.ImprovementsDescription),
                    new KeyValuePair<int, string>(1, ""),
                    new KeyValuePair<int, string>(15, ""),
                }));
            }

            //Down payment information
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("02E", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(2, "F1,F2,F3,03,F4,04,F5,10,09,01,F6,F7,11,F8,14,07,06,02,13,28,H0,H1,H3,H6,H4,H5".Split(',')[(int)DownPayment.Source]),
                new KeyValuePair<int, string>(15, DownPayment.DownPaymentAmount.ToString("0.##")),
                new KeyValuePair<int, string>(80, DownPayment.DownPaymentExplaination)
            }));

            //Applicant Data 
            if (IsCompletedJointly && CrossReferenceContact == null)
            {
                throw new NullReferenceException("If the form is being completed jointly, a cross reference contact must be supplied");
            }
            _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("03A", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(2, "BW"),
                new KeyValuePair<int, string>(9, Borrower.SocialSecurity),
                new KeyValuePair<int, string>(35, Borrower.Firstname),
                new KeyValuePair<int, string>(35, Borrower.Middlename),
                new KeyValuePair<int, string>(35, Borrower.Lastname),
                new KeyValuePair<int, string>(4, Borrower.Generation),
                new KeyValuePair<int, string>(10, Borrower.HomePhoneNumber),
                new KeyValuePair<int, string>(3, (DateTime.Now.Year - Borrower.Birthday.Year).ToString()),
                new KeyValuePair<int, string>(2, Borrower.YearsInSchool.ToString()),
                new KeyValuePair<int, string>(1, "M,S,U".Split(',')[(int)Borrower.MaritalStatus]),
                new KeyValuePair<int, string>(2, Borrower.Dependents.Count.ToString()),
                new KeyValuePair<int, string>(1, IsCompletedJointly ? "Y" : "N"),
                new KeyValuePair<int, string>(9, CrossReferenceContact != null ? CrossReferenceContact.SocialSecurity : ""),
                new KeyValuePair<int, string>(8, Borrower.Birthday.ToString("yyyyMMdd")),
                new KeyValuePair<int, string>(80, Borrower.Email)
            }));

            //If there is a co borrower, add their data as well to the form
            if (CoBorrower != null)
            {
                _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("03A", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(2, "BW"),
                new KeyValuePair<int, string>(9, CoBorrower.SocialSecurity),
                new KeyValuePair<int, string>(35, CoBorrower.Firstname),
                new KeyValuePair<int, string>(35, CoBorrower.Middlename),
                new KeyValuePair<int, string>(35, CoBorrower.Lastname),
                new KeyValuePair<int, string>(4, CoBorrower.Generation),
                new KeyValuePair<int, string>(10, CoBorrower.HomePhoneNumber),
                new KeyValuePair<int, string>(3, (DateTime.Now.Year - CoBorrower.Birthday.Year).ToString()),
                new KeyValuePair<int, string>(2, CoBorrower.YearsInSchool.ToString()),
                new KeyValuePair<int, string>(1, "M,S,U".Split(',')[(int)CoBorrower.MaritalStatus]),
                new KeyValuePair<int, string>(2, CoBorrower.Dependents.Count.ToString()),
                new KeyValuePair<int, string>(1, IsCompletedJointly ? "Y" : "N"),
                new KeyValuePair<int, string>(9, CrossReferenceContact != null ? CrossReferenceContact.SocialSecurity : ""),
                new KeyValuePair<int, string>(8, CoBorrower.Birthday.ToString("yyyyMMdd")),
                new KeyValuePair<int, string>(80, CoBorrower.Email)
            }));
            }


            //Begin adding dependents 03B
            foreach (Dependent d in Borrower.Dependents)
            {
                _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("03B", new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string>(9, Borrower.SocialSecurity),
                new KeyValuePair<int, string>(3, d.Age.ToString()),
                }));
            }

            if (CoBorrower != null)
            {
                foreach (Dependent d in CoBorrower.Dependents)
                {
                    _form.Add(new KeyValuePair<string, List<KeyValuePair<int, string>>>("03B", new List<KeyValuePair<int, string>>() {
                        new KeyValuePair<int, string>(9, CoBorrower.SocialSecurity),
                        new KeyValuePair<int, string>(3, d.Age.ToString()),
                    }));
                }
            }



            __build();
        }

        public void Save(string path)
        {
            _writer = new StreamWriter(path);
            _writer.Write(this.raw.ToString());
            _writer.Close();
            _writer.Dispose();
            _writer = null;
        }

        public async void SaveAsync(string path)
        {
            _writer = new StreamWriter(path);
            await _writer.WriteAsync(this.raw.ToString());
            _writer.Close();
            _writer.Dispose();
            _writer = null;
        }

        private void __build()
        {
            foreach (var keypair in _form)
            {
                raw.Append(keypair.Key.PadRight(3));
                foreach (var kp in keypair.Value)
                {
                    raw.Append(kp.Value.PadRight(kp.Key));
                }
                raw.Append("\r\n");
            }
        }

        /// <summary>
        /// Returns the FNM textual reprentation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.raw.ToString();
        }
    }
}  
