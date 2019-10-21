using System;
using System.Collections.Generic;
using TheatricalEventChargerDomain.Abstracts;

namespace TheatricalEventChargerDomain.Entities
{
    public class CustomerCharge
    {
        public string CustomerName { get; private set; }

        public string ChargeId { get; private set; }

        public double Charge { get; private set; }

        public DateTime ProcessedOn { get; private set; }

        public List<TheatricalPlayPerformance> Performances { get; private set; }

        private IBillCalculationStrategyFactory _billCalculationStrategy;

        public CustomerCharge(string customerName, IBillCalculationStrategyFactory billCalculationStrategy)
        {
            CustomerName = customerName;
            _billCalculationStrategy = billCalculationStrategy;
            Performances = new List<TheatricalPlayPerformance>();
        }

        public void RegisterPerformance(string play, string kindOfPlay, int audience, double price)
        {
            Performances.Add(TheatricalPlayPerformance.Create(
                play: play,
                kindOfPlay: kindOfPlay,
                price: price,
                audience: audience));
        }

        public double CalculateBill()
        {
            ChargeId = Guid.NewGuid().ToString();

            ProcessedOn = DateTime.Now;

            foreach(var performance in Performances)
            {
                var billCalculationStragedy = _billCalculationStrategy.GetBillCalculationStrategy(performance.KindOfPlay);
                var subTotalPrice = billCalculationStragedy.CalculateBill(performance.Audience, performance.Price);
                performance.UpdateSubTotalPrice(subTotalPrice);
                Charge += subTotalPrice;
            }

            return Charge;
        }
    }
}
