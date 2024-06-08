
namespace Kalender_Project_FlorianRohat
{
    public class ToDo
    {
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime TodoDate { get; set; }
        
        public bool IsImportant { get; set; }
        public ToDo() { }
        
 
        public ToDo(string title, bool isDone, DateTime todoDate, bool isImportant)
        {
            Title = title;
            IsDone = isDone;
            TodoDate = todoDate;
            IsImportant = isImportant;
        }

        public override string ToString()
        {
            return $"{Title} - {TodoDate} - {IsDone} - {IsImportant}";
        }

        

    }
}
