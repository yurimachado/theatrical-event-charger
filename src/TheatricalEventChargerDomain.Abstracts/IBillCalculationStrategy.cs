namespace TheatricalEventChargerDomain.Abstracts
{
    public interface IBillCalculationStrategy
    {
        double CalculateBill(int audience, double price);
    }
}
