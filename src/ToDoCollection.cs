﻿using System.Windows.Controls;
//using Firebase.Database;
using System.Windows.Media;
//using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat
{
    public class ToDoCollection
    {
        public List<ToDo> ToDoList { get; set; } = new List<ToDo>();

        
        public void Add(ToDo todo)
        {
            ToDoList.Add(todo);
        }

        public void Remove(ToDo todo)
        {
            ToDoList.Remove(todo);
        }

        public void Edit(ToDo todo, string title, DateTime todoDate)
        {
            todo.Title = title;
            todo.TodoDate = todoDate;
        }

        public void Draw(StackPanel stackPanel/*, FirebaseClient firebaseClient*/)
        {
            stackPanel.Children.Clear();
            foreach (ToDo todo in ToDoList)
            {
                TodoItemControl todoItemControl = new TodoItemControl();
                todoItemControl.TodoText.Text = todo.Title;
                todoItemControl.TodoDate.Text = todo.TodoDate.ToString("dd.MM.yyyy");

                todoItemControl.CheckButton.Click += async (sender, e) =>
                {
                    todo.IsDone = !todo.IsDone;
                    if (todo.IsDone)
                    {
                        todoItemControl.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    else
                    {
                        todoItemControl.Background = null;
                    }
                    /*await firebaseClient
                        .Child("Todo")
                        .Child(todo.Key)
                        .PutAsync(todo);*/
                };
                todoItemControl.DeleteButton.Click += async (sender, e) =>
                {
                    Remove(todo);
                    stackPanel.Children.Remove(todoItemControl);
                    /*await firebaseClient
                        .Child("Todo")
                        .Child(todo.Key)
                        .DeleteAsync();*/
                };
                if (todo.IsDone)
                {
                    todoItemControl.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                }

                // Code me a edit button, where when the user clicks on it, a new window opens up, where the user can edit the todo item. Do it now.
                
                

             
                stackPanel.Children.Add(todoItemControl);
            }
        }
    }
}
