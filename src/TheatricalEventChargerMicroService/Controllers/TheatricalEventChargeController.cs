using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheatricalEventChargerApplication.Commands;
using TheatricalEventChargerApplication.Commands.Entities;
using TheatricalEventChargerMicroService.Models;

namespace TheatricalEventChargerMicroService.Controllers
{
    [Route("theatrical/customers")]
    public class TheatricalEventChargeController : Controller
    {
        IMediator _mediator;

        public TheatricalEventChargeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{customerName}/performances/charge")]
        public async Task<IActionResult> Charge(string customerName, [FromBody] EventChargeInputModel input)
        {
            var chargeCustomer = new ChargeCustomerCommand {
                CustomerName = customerName,
                Performances = new List<Performance>()
            };

            input.Performances?.ForEach((performance) =>
            {
                chargeCustomer.Performances.Add(new Performance {
                    Play = performance.Play,
                    Audience = performance.Audience
                });
            });

            var response = await _mediator.Send(chargeCustomer).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                var result = new EventChargeOutputModel
                {
                    CustomerName = response.Result.CustomerName,
                    ChargeId = response.Result.ChargeId,
                    ProcessedOn = response.Result.ProcessedOn,
                    Charge = response.Result.Charge,
                    Performances = new List<PerfomanceOutputModel>()
                };

                response.Result.Performances.ForEach((performance) =>
                {
                    result.Performances.Add(new PerfomanceOutputModel
                    {
                        Play = performance.Play,
                        KindOfPlay = performance.KindOfPlay,
                        Audience = performance.Audience,
                        Price = performance.Price,
                        SubTotalPrice = performance.SubTotalPrice,
                    });
                });

                return Ok(response.Result);
            }

            return UnprocessableEntity();
        }
    }
}
