using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Firebase.Database;
using System.Windows.Media;
using Firebase.Database.Query;
using System.Windows;
using System.Threading.Tasks;

namespace Kalender_Project_FlorianRohat
{
    public class ToDoCollection
    {
        public List<ToDo> ToDoList { get; set; } = new List<ToDo>();
        private Dictionary<ToDo, string> TodoKeys = new Dictionary<ToDo, string>();
        public event EventHandler TodoAdded;

        
        public void Add(ToDo todo, string key)
        {
            ToDoList.Add(todo);
            TodoKeys[todo] = key;
            TodoAdded?.Invoke(this, EventArgs.Empty);
        }

        public void Remove(ToDo todo)
        {
            ToDoList.Remove(todo);
            TodoKeys.Remove(todo);
        }

        public void Edit(ToDo todo, string title, DateTime todoDate)
        {
            todo.Title = title;
            todo.TodoDate = todoDate;
        }
        
        public void Serialize(string filename)
        {
            using (StreamWriter stream = new StreamWriter(filename))
            {
                foreach (var todo in ToDoList)
                {
                    string jsonStr = JsonSerializer.Serialize(todo);
                    stream.WriteLine(jsonStr);
                }
            }
        }

        public bool Any(DateTime date)
        {
            return ToDoList.Any(todo => todo.TodoDate.Date == date.Date);
        }

        public void Draw(StackPanel stackPanel, DateTime selectedDate, FirebaseClient firebaseClient)
        {
            stackPanel.Children.Clear();
            foreach (ToDo todo in ToDoList)
            {
                if (todo.TodoDate.Date == selectedDate.Date)
                {
                    string key = TodoKeys[todo];
                    TodoItemControl todoItemControl = new TodoItemControl();
                    todoItemControl.Margin = new Thickness(40,2,40,10);
                    todoItemControl.TodoText.Text = todo.Title;
                    todoItemControl.TodoDate.Text = todo.TodoDate.ToString("dd.MM.yyyy");
                    
                    todoItemControl.ImportantButton.Click += async (sender, e) =>
                    {
                        
                    };

                    todoItemControl.CheckButton.Click += async (sender, e) =>
                    {
                        todo.IsDone = !todo.IsDone;
                        if (todo.IsDone)
                        {
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                        }
                        else
                        {
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                        }
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .PutAsync(todo);
                    };
                    todoItemControl.DeleteButton.Click += async (sender, e) =>
                    {
                        Remove(todo);
                        stackPanel.Children.Remove(todoItemControl);
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .DeleteAsync();
                        
                    };
                    todoItemControl.EditButton.Click += async (sender, e) =>
                    {
                        AddTodoWindow editTodoWindow = new AddTodoWindow(todo);
                        if (editTodoWindow.ShowDialog() == true)
                        {
                            Edit(todo, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                            Draw(stackPanel, selectedDate, firebaseClient);
                            string key = TodoKeys[todo];
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };
                    if (todo.IsDone)
                    {
                        todoItemControl.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    stackPanel.Children.Add(todoItemControl);
                }
            }
        }
        public void ShowAllTodosGroupedByDate(StackPanel stackPanel, FirebaseClient firebaseClient)
        {
            stackPanel.Children.Clear();

            var groupedTodos = ToDoList
                .GroupBy(todo => todo.TodoDate) 
                .OrderBy(group => group.Key); 

            foreach (var group in groupedTodos)
            {
                Label dateLabel = new Label
                {
                    Content = group.Key.ToString("dd/MM/yyyy"),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 0),
                    Foreground = Brushes.White,
                    
                };

                stackPanel.Children.Add(dateLabel);

                foreach (var todo in group)
                {
                    TodoItemControl todoItemControl = new TodoItemControl();
                    todoItemControl.TodoText.Text = todo.Title;
                    todoItemControl.TodoDate.Text = todo.TodoDate.ToString("dd.MM.yyyy");

                    string key = TodoKeys[todo];

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
                            .Child(key)
                            .PutAsync(todo);
                    };
                    todoItemControl.DeleteButton.Click += async (sender, e) =>
                    {
                        Remove(todo);
                        stackPanel.Children.Remove(todoItemControl);
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .DeleteAsync();
                        if (!ToDoList.Any(t => t.TodoDate.Date == todo.TodoDate.Date))
                        {
                            stackPanel.Children.Remove(dateLabel);
                        }
                    };
                    todoItemControl.EditButton.Click += async (sender, e) =>
                    {
                        AddTodoWindow editTodoWindow = new AddTodoWindow(todo);
                        if (editTodoWindow.ShowDialog() == true)
                        {
                            Edit(todo, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                            ShowAllTodosGroupedByDate(stackPanel, firebaseClient);
                            string key = TodoKeys[todo];
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };
                    if (todo.IsDone)
                    {
                        todoItemControl.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }

                    stackPanel.Children.Add(todoItemControl);
                }
            }
        }
        public int DrawDay(DateTime date)
        {
            return ToDoList.Count(todo => todo.TodoDate.Date == date.Date);
        }
    }
}
