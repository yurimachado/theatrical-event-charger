using System.Collections.Generic;
using TheatricalEventChargerDomain.Abstracts;
using TheatricalEventChargerDomain.BillCalculationStrategies;
using TheatricalEventChargerDomain.Entities;
using Xunit;

namespace TheatricalEventChargerDomain.UnitTests
{
    public class CalculateCustomerChargeUnitTests
    {
        Dictionary<string, IBillCalculationStrategy> _billCalculationStrategies;

        IBillCalculationStrategyFactory _billCalculationStrategyFactory;

        public CalculateCustomerChargeUnitTests()
        {
            _billCalculationStrategies = new Dictionary<string, IBillCalculationStrategy>();

            _billCalculationStrategies.Add("default", new DefaultBillCalculationStrategy());
            _billCalculationStrategies.Add("tragedy", new TragedyBillCalculationStrategy());
            _billCalculationStrategies.Add("comedy", new ComedyBillCalculationStrategy());

            _billCalculationStrategyFactory = new BillCalculationStrategyFactory(_billCalculationStrategies);
        }

        [Fact]
        public void CustomerChargeForPerformanceOfTragedyKind()
        {
            CustomerCharge customerCharge = new CustomerCharge("XYZ", _billCalculationStrategyFactory);

            customerCharge.RegisterPerformance(
                play: "hamlet",
                kindOfPlay: "tragedy",
                audience: 55,
                price: 40000);

            customerCharge.CalculateBill();

            Assert.Equal(2225000, customerCharge.Charge);
        }

        [Fact]
        public void CustomerChargeForPerformanceOfComedyKind()
        {
            CustomerCharge customerCharge = new CustomerCharge("XYZ", _billCalculationStrategyFactory);

            customerCharge.RegisterPerformance(
                play: "as-like",
                kindOfPlay: "comedy",
                audience: 35,
                price: 30000);

            customerCharge.CalculateBill();

            Assert.Equal(1057500, customerCharge.Charge);
        }

        [Fact]
        public void CustomerChargeForAllKindsOfPerformance()
        {
            CustomerCharge customerCharge = new CustomerCharge("XYZ", _billCalculationStrategyFactory);

            customerCharge.RegisterPerformance(
                play: "hamlet",
                kindOfPlay: "tragedy",
                audience: 55,
                price: 40000);

            customerCharge.RegisterPerformance(
                play: "as-like",
                kindOfPlay: "comedy",
                audience: 35,
                price: 30000);

            customerCharge.CalculateBill();

            Assert.Equal(3282500, customerCharge.Charge);
        }

        [Fact]
        public void CustomerChargeForOtherKindsOfPlay()
        {
            CustomerCharge customerCharge = new CustomerCharge("XYZ", _billCalculationStrategyFactory);

            customerCharge.RegisterPerformance(
                play: "hamilton",
                kindOfPlay: "musical",
                audience: 60,
                price: 50000);

            customerCharge.CalculateBill();

            Assert.Equal(3000000, customerCharge.Charge);
        }
    }
}
