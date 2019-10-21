using TheatricalEventChargerDomain.Abstracts;

namespace TheatricalEventChargerDomain.BillCalculationStrategies
{
    public class DefaultBillCalculationStrategy : IBillCalculationStrategy
    {
        public double CalculateBill(int audience, double price)
        {
            return audience * price;
        }
    }
}
