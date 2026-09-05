namespace Garden
{
    public class FruitDataConfig
    {
        public TreeGenomeConfig TreeGenome;
        
        public bool IsGrowth;
        public float ColorOffset;
        public float TimerStart;

        public bool IsRoting(float time) => time >= TimerStart + TreeGenome.FruitRotingTime;
        public float GetCost() => TreeGenome.Quality * TreeGenome.FruitCostMultiplier;
    }
}