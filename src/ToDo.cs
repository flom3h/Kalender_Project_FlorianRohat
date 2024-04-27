using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalender_Project_FlorianRohat
{
    public class ToDo
    {
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }

        public ToDo() { }
        

        public ToDo(string title, bool isDone, DateTime dueDate)
        {
            Title = title;
            IsDone = isDone;
            DueDate = dueDate;
        }

        public override string ToString()
        {
            return $"{Title} - {DueDate} - {IsDone}";
        }

        

    }
}
