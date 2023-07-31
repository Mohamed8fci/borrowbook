namespace bookTask.Models
{
    public class BorrowedBook
    {
        public int BorrowedBookId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; }
        public int BorrowedVersions { get; set; }
    }
}
