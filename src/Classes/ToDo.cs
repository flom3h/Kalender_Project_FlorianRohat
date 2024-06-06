using System;       
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kalender_Project_FlorianRohat
{
    public class ToDo
    {
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime TodoDate { get; set; }
        public ToDo() { }
        
 
        public ToDo(string title, bool isDone, DateTime todoDate)
        {
            Title = title;
            IsDone = isDone;
            TodoDate = todoDate;
        }

        public override string ToString()
        {
            return $"{Title} - {TodoDate} - {IsDone}";
        }

        

    }
}
