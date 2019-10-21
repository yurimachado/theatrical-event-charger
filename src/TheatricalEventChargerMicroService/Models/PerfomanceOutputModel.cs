namespace TheatricalEventChargerMicroService.Models
{
    public class PerfomanceOutputModel
    {
        public string Play { get; set; }

        public string KindOfPlay { get; set; }

        public int Audience { get; set; }

        public double Price { get; set; }

        public double SubTotalPrice { get; set; }
    }
}
