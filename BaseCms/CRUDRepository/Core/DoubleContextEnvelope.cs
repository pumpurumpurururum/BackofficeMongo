namespace BaseCms.CRUDRepository.Core
{
    public class DoubleContextEnvelope<TContext, TInput>
    {
        public DoubleContextEnvelope(TContext primaryContext, TContext secondaryContext, TInput input)
        {
            PrimaryContext = primaryContext;
            SecondaryContext = secondaryContext;
            Input = input;
        }

        public TContext PrimaryContext { get; set; }
        public TContext SecondaryContext { get; set; }
        public TInput Input { get; set; }
    }
}
