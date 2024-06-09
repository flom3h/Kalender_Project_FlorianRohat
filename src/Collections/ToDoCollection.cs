using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Firebase.Database;
using System.Windows.Media;
using Firebase.Database.Query;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kalender_Project_FlorianRohat
{
    public class ToDoCollection
    {
        public List<ToDo> ToDoList { get; set; } = new List<ToDo>();
        private Dictionary<ToDo, string> TodoKeys = new Dictionary<ToDo, string>();
        public event EventHandler TodoAdded;

        
        public void Add(ToDo todo, string key)
        {
            Log.log.Information("TodoCollection: Add function called, adding todo to list");
            ToDoList.Add(todo);
            TodoKeys[todo] = key;
            TodoAdded?.Invoke(this, EventArgs.Empty);
        }

        public void Remove(ToDo todo)
        {
            Log.log.Information("TodoCollection: Remove function called, removing todo from list");
            ToDoList.Remove(todo);
            TodoKeys.Remove(todo);
        }

        public void Edit(ToDo todo, string title, DateTime todoDate)
        {
            Log.log.Information("TodoCollection: Edit function called, editing todo in list");
            todo.Title = title;
            todo.TodoDate = todoDate;
        }
        
        public void Serialize(string filename)
        {
            Log.log.Information("TodoCollection: Serialize function called, writing todos to Json file");
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
            Log.log.Information("TodoCollection: Any function called, checking if any todos exist for the date");
            return ToDoList.Any(todo => todo.TodoDate.Date == date.Date);
        }

        public void Draw(StackPanel stackPanel, DateTime selectedDate, FirebaseClient firebaseClient)
        {
            Log.log.Information("TodoCollection: Draw function called, drawing todos to stackpanel");
            stackPanel.Children.Clear();
            foreach (ToDo todo in ToDoList)
            {
                if (todo.TodoDate.Date == selectedDate.Date)
                {
                    Log.log.Information("TodoCollection: Checking if todo date is the selected date");
                    string key = TodoKeys[todo];
                    TodoItemControl todoItemControl = new TodoItemControl();
                    todoItemControl.Margin = new Thickness(40,2,40,10);
                    todoItemControl.TodoText.Text = todo.Title;
                    todoItemControl.TodoDate.Text = todo.TodoDate.ToString("dd.MM.yyyy");
                    Log.log.Information("TodoCollection: Checking if todo is important");
                    if (todo.IsImportant)
                    {
                        todoItemControl.ImportantButton.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative)),
                            Width = 20,
                            Height = 20
                        };
                    }
                    else
                    {
                        todoItemControl.ImportantButton.Content = new Image
                        {
                            
                            Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative)),
                            Width = 20,
                            Height = 20
                        };
                    }
                    Log.log.Information("TodoCollection: Checking if todo is done");
                    if (todo.IsDone)
                    {
                        todoItemControl.Background = new SolidColorBrush(Color.FromRgb(28, 28, 28));
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    else
                    {
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                    }
                    todoItemControl.ImportantButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Important button clicked, changing inportant status");
                        todo.IsImportant = !todo.IsImportant;
                        var buttonContent = todoItemControl.ImportantButton.Content as Image;
                        Log.log.Information("TodoCollection: Checking if content of button is null");
                        if (buttonContent != null)
                        {
                            Log.log.Information("TodoCollection: Checking if todo is important");
                            if (todo.IsImportant)
                            {
                                buttonContent.Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative));
                            }
                            else
                            {
                                buttonContent.Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative));
                            }
                            Log.log.Information("TodoCollection: Updating Firebase database after change");
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };
                    todoItemControl.CheckButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Check button clicked, changing done status");
                        todo.IsDone = !todo.IsDone;
                        Log.log.Information("TodoCollection: Checking if todo is done");
                        if (todo.IsDone)
                        {
                            todoItemControl.Background = new SolidColorBrush(Color.FromRgb(28, 28, 28));
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                        }
                        else
                        {
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                        }
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .PutAsync(todo);
                    };
                    todoItemControl.DeleteButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Delete button clicked, removing todo");
                        Remove(todo);
                        stackPanel.Children.Remove(todoItemControl);
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .DeleteAsync();
                        
                    };
                    todoItemControl.EditButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Edit button clicked, opening edit window");
                        AddTodoWindow editTodoWindow = new AddTodoWindow(todo);
                        Log.log.Information("TodoCollection: Checking if edit window is true");
                        if (editTodoWindow.ShowDialog() == true)
                        {
                            Edit(todo, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                            Draw(stackPanel, selectedDate, firebaseClient);
                            string key = TodoKeys[todo];
                            Log.log.Information("TodoCollection: Updating Firebase database after change");
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };
                    Log.log.Information("TodoCollection: Checking if todo is done");
                    if (todo.IsDone)
                    {
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    Log.log.Information("TodoCollection: Adding todo to stackpanel");
                    stackPanel.Children.Add(todoItemControl);
                }
            }
        }
        public void DrawAllTodos(StackPanel stackPanel, FirebaseClient firebaseClient)
        {
            Log.log.Information("TodoCollection: DrawAllTodos function called, drawing all todos to stackpanel");
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
                    Log.log.Information("TodoCollection: Checking if todo is important");
                    if (todo.IsImportant)
                    {
                        todoItemControl.ImportantButton.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative)),
                            Width = 20,
                            Height = 20
                        };
                    }
                    else
                    {
                        todoItemControl.ImportantButton.Content = new Image
                        {
                            Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative)),
                            Width = 20,
                            Height = 20
                        };
                    }
                    todoItemControl.ImportantButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Important button clicked, changing important status");
                        todo.IsImportant = !todo.IsImportant;
                        var buttonContent = todoItemControl.ImportantButton.Content as Image;
                        Log.log.Information("TodoCollection: Checking if content of button is null");
                        if (buttonContent != null)
                        {
                            Log.log.Information("TodoCollection: Checking if todo is important");
                            if (todo.IsImportant)
                            {
                                buttonContent.Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative));
                            }
                            else
                            {
                                buttonContent.Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative));
                            }
                            Log.log.Information("TodoCollection: Updating Firebase database after change");
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };

                    todoItemControl.CheckButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Check button clicked, changing done status");
                        todo.IsDone = !todo.IsDone;
                        Log.log.Information("TodoCollection: Checking if todo is done");
                        if (todo.IsDone)
                        {
                            todoItemControl.Background = new SolidColorBrush(Color.FromRgb(28, 28, 28));
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                        }
                        else
                        {
                            todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                        }
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .PutAsync(todo);
                    };
                    todoItemControl.DeleteButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Delete button clicked, removing todo");
                        Remove(todo);
                        stackPanel.Children.Remove(todoItemControl);
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .DeleteAsync();
                        Log.log.Information("TodoCollection: Checking if Date Label has todo");
                        if (!ToDoList.Any(t => t.TodoDate.Date == todo.TodoDate.Date))
                        {
                            Log.log.Information("TodoCollection: Removing Date Label from stackpanel");
                            stackPanel.Children.Remove(dateLabel);
                        }
                    };
                    todoItemControl.EditButton.Click += async (sender, e) =>
                    {
                        Log.log.Information("TodoCollection: Edit button clicked, opening edit window");
                        AddTodoWindow editTodoWindow = new AddTodoWindow(todo);
                        Log.log.Information("TodoCollection: Checking if edit window is true");
                        if (editTodoWindow.ShowDialog() == true)
                        { 
                            Edit(todo, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                            DrawAllTodos(stackPanel, firebaseClient);
                            string key = TodoKeys[todo];
                            Log.log.Information("TodoCollection: Updating Firebase database after change");
                            await firebaseClient
                                .Child("Todo")
                                .Child(key)
                                .PutAsync(todo);
                        }
                    };
                    Log.log.Information("TodoCollection: Checking if todo is done");
                    if (todo.IsDone)
                    {
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    Log.log.Information("TodoCollection: Adding todo to stackpanel");
                    stackPanel.Children.Add(todoItemControl);
                }
            }
        }
        
        public void DrawImportantTodos(StackPanel stackPanel, FirebaseClient firebaseClient)
        {
            Log.log.Information("TodoCollection: DrawImportantTodos function called, drawing important todos to stackpanel");
            stackPanel.Children.Clear();
            var importantTodos = ToDoList
                .Where(todo => todo.IsImportant)
                .OrderBy(todo => todo.TodoDate);

            foreach (var todo in importantTodos)
            {
                Label info = new Label
                {
                    Content = todo.TodoDate.ToString("dd/MM/yyyy"),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 0),
                    Foreground = Brushes.White,
                    
                };

                stackPanel.Children.Add(info);
                
                TodoItemControl todoItemControl = new TodoItemControl();
                todoItemControl.TodoText.Text = todo.Title;
                todoItemControl.TodoDate.Text = todo.TodoDate.ToString("dd.MM.yyyy");
                todo.IsImportant = true;
                string key = TodoKeys[todo];
                Log.log.Information("TodoCollection: Checking if todo is important");
                if (todo.IsImportant)
                {
                    todoItemControl.ImportantButton.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative)),
                        Width = 20,
                        Height = 20
                    };
                }
                else
                {
                    todoItemControl.ImportantButton.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative)),
                        Width = 20,
                        Height = 20
                    };
                }
                todoItemControl.ImportantButton.Click += async (sender, e) =>
                {
                    Log.log.Information("TodoCollection: Important button clicked, changing important status");
                    todo.IsImportant = !todo.IsImportant;
                    var buttonContent = todoItemControl.ImportantButton.Content as Image;
                    Log.log.Information("TodoCollection: Checking if content of button is null");
                    if (buttonContent != null)
                    {
                        Log.log.Information("TodoCollection: Checking if todo is important");
                        if (todo.IsImportant)
                        {
                            buttonContent.Source = new BitmapImage(new Uri("../Images/starfilled.png", UriKind.Relative));
                        }
                        else
                        {
                            buttonContent.Source = new BitmapImage(new Uri("../Images/star.png", UriKind.Relative));
                        }
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .PutAsync(todo);
                    }
                    DrawImportantTodos(stackPanel, firebaseClient);
                };

                todoItemControl.CheckButton.Click += async (sender, e) =>
                {
                    Log.log.Information("TodoCollection: Check button clicked, changing done status");
                    todo.IsDone = !todo.IsDone;
                    Log.log.Information("TodoCollection: Checking if todo is done");
                    if (todo.IsDone)
                    {
                        todoItemControl.Background = new SolidColorBrush(Color.FromRgb(28, 28, 28));
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                    }
                    else
                    {
                        todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
                    }
                    Log.log.Information("TodoCollection: Updating Firebase database after change");
                    await firebaseClient
                        .Child("Todo")
                        .Child(key)
                        .PutAsync(todo);
                };
                todoItemControl.DeleteButton.Click += async (sender, e) =>
                {
                    Log.log.Information("TodoCollection: Delete button clicked, removing todo");
                    Remove(todo);
                    stackPanel.Children.Remove(todoItemControl);
                    Log.log.Information("TodoCollection: Updating Firebase database after change");
                    await firebaseClient
                        .Child("Todo")
                        .Child(key)
                        .DeleteAsync();
                };
                todoItemControl.EditButton.Click += async (sender, e) =>
                {
                    Log.log.Information("TodoCollection: Edit button clicked, opening edit window");
                    AddTodoWindow editTodoWindow = new AddTodoWindow(todo);
                    Log.log.Information("TodoCollection: Checking if edit window is true");
                    if (editTodoWindow.ShowDialog() == true)
                    {
                        Edit(todo, editTodoWindow.Todo.Title, editTodoWindow.Todo.TodoDate);
                        DrawImportantTodos(stackPanel, firebaseClient);
                        string key = TodoKeys[todo];
                        Log.log.Information("TodoCollection: Updating Firebase database after change");
                        await firebaseClient
                            .Child("Todo")
                            .Child(key)
                            .PutAsync(todo);
                    }
                };
                Log.log.Information("TodoCollection: Checking if todo is done");
                if (todo.IsDone)
                {
                    todoItemControl.TodoItemBorder.Background = new SolidColorBrush(Color.FromArgb(204, 144, 238, 144));
                }
                Log.log.Information("TodoCollection: Adding todo to stackpanel");
                stackPanel.Children.Add(todoItemControl);
            }
        }
        public int DrawDay(DateTime date)
        {
            //Log.log.Information("TodoCollection: DrawDay function called, returning number of todos for the date");
            return ToDoList.Count(todo => todo.TodoDate.Date == date.Date);
        }
    }
}
