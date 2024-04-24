﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Firebase.Database;
using System.Windows.Media;
using Firebase.Database.Query;

namespace Kalender_Project_FlorianRohat
{
    public class ToDoCollection
    {
        List<ToDo> ToDoList { get; set; } = new List<ToDo>();

        public void Add(ToDo todo)
        {
            ToDoList.Add(todo);
        }

        public void Remove(ToDo todo)
        {
            ToDoList.Remove(todo);
        }

        public void Draw(StackPanel todoListPanel, FirebaseClient firebaseClient)
        {
            todoListPanel.Children.Clear();
            foreach (ToDo todo in ToDoList)
            {
                TodoItemControl todoItemControl = new TodoItemControl();
                todoItemControl.TodoText.Text = todo.Title;
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
                    await firebaseClient
                        .Child("Todo")
                        .Child(todo.Key)
                        .PutAsync(todo);
                };
                todoItemControl.DeleteButton.Click += async (sender, e) =>
                {
                    Remove(todo);
                    todoListPanel.Children.Remove(todoItemControl);
                    await firebaseClient
                        .Child("Todo")
                        .Child(todo.Key)
                        .DeleteAsync();
                };
                if (todo.IsDone)
                {
                    todoItemControl.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                }
                todoListPanel.Children.Add(todoItemControl);
            }
        }
    }
}