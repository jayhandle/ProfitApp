using NUnit.Framework;

namespace ProfitLibrary.Test
{
    public class PaymentDetailTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCaseSource(typeof(PaymentDetailData), "ConvertDollarsToPenniesDatas")]
        public void ConvertDollarsToPennies(PaymentDetailData.ConvertDollarsToPenniesData data)
        {
            var pennies = PaymentDetail.ConvertDollarstoPennies(data.StringValue);
            Assert.AreEqual(data.Pennies, pennies);
        }
    }
}