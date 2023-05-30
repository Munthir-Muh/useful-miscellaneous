using System.Linq;

namespace System.Collections.Generic
{
    public class PaginatedCollection<T> : IEnumerable<IEnumerable<T>>
    {
        private readonly IEnumerable<T> _internalCollection;
        public int PageSize { get; private set; }
        public int CurrentPageNo { get; private set; }
        public int TotalPages => (int)Math.Ceiling((double)_internalCollection.Count() / PageSize);
        public PaginatedCollection(IEnumerable<T> collection, int pageSize)
        {
            PageSize = pageSize;
            _internalCollection = collection;
        }
        public IEnumerable<T> this[int pageNo] => GoToPageNo(pageNo);
        public void ChangePageCapacity(int pagesize)
        {
            PageSize = pagesize;
            CurrentPageNo = 1;
        }
        public IEnumerable<T> GoToPageNo(int pageNo)
        {
            CurrentPageNo = pageNo;

            return _internalCollection.Skip(PageSize * (pageNo - 1))
                .Take(PageSize);
        }
        public IEnumerable<T> NextPage()
        {
            return GoToPageNo(PageSize * CurrentPageNo++);
        }
        public IEnumerable<T> PreviousPage()
        {
            return GoToPageNo(PageSize * (CurrentPageNo--) - 2);
        }
        public IEnumerable<T> CurrentPage()
        {
            return GoToPageNo(PageSize * CurrentPageNo - 1);
        }    
        public IEnumerable<T> FirstPage()
        {
            return GoToPageNo(1);
        }
        public IEnumerable<T> LastPage()
        {
            return GoToPageNo(TotalPages);
        }
        public IEnumerable<T> GoToPageOfRecordNo(int recordNo)
        {
            var recortPageNo = Math.Ceiling((double)recordNo / PageSize);
            return GoToPageNo(PageSize);
        }
        private IEnumerable<IEnumerable<T>> Browse()
        {
            for (int pages = 1; pages <= TotalPages; pages++)
            {
                yield return GoToPageNo(pages);
            }
        }
        IEnumerator<IEnumerable<T>> IEnumerable<IEnumerable<T>>.GetEnumerator()
        {
            return Browse().GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Browse().GetEnumerator();
        }
    }
}
