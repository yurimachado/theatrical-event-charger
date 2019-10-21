using System.Collections.Generic;

namespace TheatricalEventChargerMicroService.Models
{
    public class EventChargeInputModel
    {
        public List<PerformanceInputModel> Performances { get; set; }
    }
}
