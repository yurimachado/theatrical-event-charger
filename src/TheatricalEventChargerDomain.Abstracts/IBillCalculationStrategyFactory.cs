namespace TheatricalEventChargerDomain.Abstracts
{
    public interface IBillCalculationStrategyFactory
    {
        IBillCalculationStrategy GetBillCalculationStrategy(string kinfOfPlay);
    }
}
