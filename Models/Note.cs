namespace NotesApi.Models
{
    public class Note
    {
        public int Id { get; set; }         // Primary key.
        public string Title { get; set; }   // Note title.
        public string Content { get; set; } // Note content.
    }
}
