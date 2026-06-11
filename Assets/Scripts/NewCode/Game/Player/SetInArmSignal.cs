namespace Garden
{
    public class SetInArmSignal : ISignal
    {
        public readonly EntityType Type;
        public readonly int Seed;
        public SetInArmSignal(int seed, EntityType type)
        {
            Seed = seed;
            Type = type;
        }
        
    }
}