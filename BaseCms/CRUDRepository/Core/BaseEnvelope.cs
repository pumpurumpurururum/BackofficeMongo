namespace BaseCms.CRUDRepository.Core
{
    public class BaseEnvelope<TInput>
    {
        public BaseEnvelope(TInput input)
        {
            Input = input;
        }
        public TInput Input { get; set; }
    }
}
