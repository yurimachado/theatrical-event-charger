using System;
using System.Collections.Generic;

namespace TheatricalEventChargerMicroService.Models
{
    public class EventChargeOutputModel
    {
        public string CustomerName { get; set; }

        public string ChargeId { get; set; }

        public DateTime ProcessedOn { get; set; }

        public double Charge { get; set; }

        public List<PerfomanceOutputModel> Performances { get; set; }

    }
}
