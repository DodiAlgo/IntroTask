namespace IntroTask.Models
{
    public class ToDoText
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public bool IsComplete { get; set; }

        public int numWords { get; set; }
    }
}
