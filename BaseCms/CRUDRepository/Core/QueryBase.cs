using BaseCms.CRUDRepository.Core.Intefaces;

namespace BaseCms.CRUDRepository.Core
{
    public abstract class QueryBase<TOutput> : IQuery //where TOutput : IEntityBase
    {
        private object _innerOutput;

        public TOutput Output
        {
            get { return (TOutput)_innerOutput; }
            set { _innerOutput = value; }
        }

        object IQuery.InnerOutput
        {
            get { return _innerOutput; }
            set { _innerOutput = value; }
        }
    }
}
