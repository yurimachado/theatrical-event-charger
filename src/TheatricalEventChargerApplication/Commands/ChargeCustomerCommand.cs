using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheatricalEventChargerApplication.Commands.Entities;
using TheatricalEventChargerDomain.Abstracts;
using TheatricalEventChargerDomain.Entities;
using TheatricalEventChargerRepository.Abstracts;

namespace TheatricalEventChargerApplication.Commands
{
    public class ChargeCustomerCommand : IRequest<Response<CustomerCharge>>
    {
        public string CustomerName { get; set; }

        public List<Performance> Performances { get; set; }

        internal class ChargeCustomerCommandHandler : IRequestHandler<ChargeCustomerCommand, Response<CustomerCharge>>
        {
            private IBillCalculationStrategyFactory _billCalculationStrategyFactory;
            private IRepository<string, CustomerCharge> _customerChargeRepository;
            private IReadOnlyRepository<string, TheatricalPlayCatalogItem> _theatricalPlayCatalogReadOnlyRepository;

            public ChargeCustomerCommandHandler(
                IBillCalculationStrategyFactory billCalculationStrategyFactory,
                IRepository<string, CustomerCharge> customerChargeRepository,
                IReadOnlyRepository<string, TheatricalPlayCatalogItem> theatricalPlayCatalogReadOnlyRepository
                )
            {
                _billCalculationStrategyFactory = billCalculationStrategyFactory;
                _customerChargeRepository = customerChargeRepository;
                _theatricalPlayCatalogReadOnlyRepository = theatricalPlayCatalogReadOnlyRepository;
            }

            public Task<Response<CustomerCharge>> Handle(ChargeCustomerCommand request, CancellationToken cancellationToken)
            {
                CustomerCharge customerCharge = new CustomerCharge(request.CustomerName, _billCalculationStrategyFactory);

                foreach (var performance in request.Performances)
                {
                    var theatricalPlayCatalogItem = _theatricalPlayCatalogReadOnlyRepository.FindByKey(performance.Play);

                    if (theatricalPlayCatalogItem != null)
                    {
                        customerCharge.RegisterPerformance(
                            play: performance.Play,
                            audience: performance.Audience,
                            kindOfPlay: theatricalPlayCatalogItem.KindOfPlay,
                            price: theatricalPlayCatalogItem.Price);
                    }
                }

                customerCharge.CalculateBill();

                _customerChargeRepository.Add(customerCharge);

                return Task.FromResult(Response<CustomerCharge>.Ok(customerCharge));
            }
        }
    }
}
