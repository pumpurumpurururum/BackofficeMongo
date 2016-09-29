namespace BaseCms.CRUDRepository.Core
{
    public class ContextEnvelope<TContext, TInput>
    {
        public ContextEnvelope(TContext context, TInput input)
        {
            Context = context;
            Input = input;
        }

        public TContext Context { get; set; }
        public TInput Input { get; set; }
    }
}
