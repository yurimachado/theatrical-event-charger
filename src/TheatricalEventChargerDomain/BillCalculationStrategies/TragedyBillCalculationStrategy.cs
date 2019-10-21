using TheatricalEventChargerDomain.Abstracts;

namespace TheatricalEventChargerDomain.BillCalculationStrategies
{
    public class TragedyBillCalculationStrategy : IBillCalculationStrategy
    {
        public double CalculateBill(int audience, double price)
        {
            if (audience <= 30)
                return audience * price;

            var priceForAudienceSizeEqualTo30 = price * 30;

            var audienceSizeExceeded = audience - 30;

            var additionalCost = 1000;

            var priceForExceededAudienceSize = audienceSizeExceeded * (price + additionalCost);

            return priceForExceededAudienceSize + priceForAudienceSizeEqualTo30;
        }
    }
}
