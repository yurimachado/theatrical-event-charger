using System.Collections.Generic;
using TheatricalEventChargerDomain.Abstracts;

namespace TheatricalEventChargerDomain.BillCalculationStrategies
{
    public class BillCalculationStrategyFactory : IBillCalculationStrategyFactory
    {
        Dictionary<string, IBillCalculationStrategy> _billCalculationStrategies;
        string _defaultBillCalculationStrategyName;

        public BillCalculationStrategyFactory(
            Dictionary<string, IBillCalculationStrategy> billCalculationStrategies,
            string defaultBillCalculationStrategyName = "default")
        {
            _billCalculationStrategies = (billCalculationStrategies ?? new Dictionary<string, IBillCalculationStrategy>());
            _defaultBillCalculationStrategyName = defaultBillCalculationStrategyName;

            if (!_billCalculationStrategies.ContainsKey(_defaultBillCalculationStrategyName))
                _billCalculationStrategies.Add("default", new DefaultBillCalculationStrategy());
        }

        public IBillCalculationStrategy GetBillCalculationStrategy(string kinfOfPlay)
        {
            if (!_billCalculationStrategies.ContainsKey(kinfOfPlay))
                return _billCalculationStrategies[_defaultBillCalculationStrategyName];

            return _billCalculationStrategies[kinfOfPlay];
        }
    }
}
