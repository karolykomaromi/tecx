namespace TecX.Unity.Test.TestObjects
{
    public class CompanyWrapper2 : ICompany
    {
        private readonly ICompany company;

        public CompanyWrapper2(ICompany company)
        {
            this.company = company;
        }

        public string Foo()
        {
            return "-->Wrapper2" + this.company.Foo();
        }
    }
}