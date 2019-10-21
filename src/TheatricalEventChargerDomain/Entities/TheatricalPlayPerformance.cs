namespace TheatricalEventChargerDomain.Entities
{
    public class TheatricalPlayPerformance
    {
        public string Play { get; private set; }

        public string KindOfPlay { get; private set; }

        public int Audience { get; private set; }

        public double Price { get; private set; }

        public double SubTotalPrice { get; private set; }

        public static TheatricalPlayPerformance Create(string play, string kindOfPlay, int audience, double price)
        {
            return new TheatricalPlayPerformance
            {
                Play = play,
                KindOfPlay = kindOfPlay,
                Audience = audience,
                Price = price
            };
        }

        public void UpdateSubTotalPrice(double subTotalPrice) => SubTotalPrice = subTotalPrice;

    }
}
