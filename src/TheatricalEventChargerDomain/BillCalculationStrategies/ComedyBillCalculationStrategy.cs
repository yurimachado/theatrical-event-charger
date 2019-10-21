using TheatricalEventChargerDomain.Abstracts;

namespace TheatricalEventChargerDomain.BillCalculationStrategies
{
    public class ComedyBillCalculationStrategy : IBillCalculationStrategy
    {
        public double CalculateBill(int audience, double price)
        {
            if (audience <= 20)
                return audience * price;

            var priceForAudienceSizeEqualTo20 = price * 20;

            var audienceSizeExceeded = audience - 20;

            var additionalCost = 500;

            var priceForExceededAudienceSize = audienceSizeExceeded * (price + additionalCost);

            return priceForExceededAudienceSize + priceForAudienceSizeEqualTo20;
        }
    }
}
