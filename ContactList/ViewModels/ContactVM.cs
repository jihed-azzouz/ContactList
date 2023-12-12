namespace ContactList.ViewModels
{
    public class ContactVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
        public DateTime Date { get; set; }
    }
}
