namespace Garden
{
    public class SetInArmSignal : ISignal
    {
        public readonly EntityCreationRequestSignal Request;
        public SetInArmSignal(EntityCreationRequestSignal arm)
        {
            Request = arm;
        }
    }
}