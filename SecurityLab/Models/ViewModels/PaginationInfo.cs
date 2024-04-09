namespace SecurityLab.Models.ViewModels
{
    public class PaginationInfo
    {
        public int totalItems { get; set; }
        public int itemsPerPage { get; set; }
        public int currentPage { get; set; }
        public int totalNumPages => (int)(Math.Ceiling((decimal)totalItems / itemsPerPage));
    }
}

