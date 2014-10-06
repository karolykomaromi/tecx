namespace TecX.Unity.Test.TestObjects
{
    public class CompanyWrapper1 : ICompany
    {
        private readonly ICompany company;

        public CompanyWrapper1(ICompany company)
        {
            this.company = company;
        }

        public string Foo()
        {
            return "-->Wrapper1" + this.company.Foo();
        }
    }
}