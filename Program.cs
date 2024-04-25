using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleAppDemo;

class Program
{
    static void Main(string[] args)
    {
        List<TodoEntry> todoList = new List<TodoEntry>()
        {
            new TodoEntry("Sample Todo"),
            new TodoEntry("Due Todo", dueDate: DateTime.Now.AddDays(3))
        };

        while (true)
        {
            Console.WriteLine("Enter command (type \"exit\" to quit): ");
            var command = Console.ReadLine();

            if (command == "exit")
            {
                break;
            }
            if (string.IsNullOrEmpty(command))
            {
                continue;
            }

            if (command.StartsWith("create"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: create <todo-name> [<todo-description>] [<todo-due-date>]");
                    continue;
                }

                DateTime dueDate = default;
                bool hasDueDate = todoParams.Length == 4 && DateTime.TryParse(todoParams[3], out dueDate);
                DateTime? dueDateParam = hasDueDate ? dueDate : null;

                var newEntry = new TodoEntry(todoParams![1], (todoParams.Length >= 3 ? todoParams[2] : null), dueDateParam);
                todoList.Add(newEntry);

                string dueDateMessage = hasDueDate ? $"(Due date: {dueDateParam})" : "";
                Console.WriteLine($"Added '{newEntry.Title}' to Todo List {dueDateMessage}");
            }

            // command "list"
            if (command.StartsWith("list"))
            {
                if (todoList.Count <= 0)
                {
                    Console.WriteLine("You don't have any todo.");
                    continue;
                }
                foreach ( var todo in todoList )
                {
                    Console.WriteLine("---------------------------------------------------------------------\n");
                    Console.WriteLine($"Id: {todo.Id}\nTitle: {todo.Title}\nDescription: {todo.Description}");
                    if ( todo.DueDate != null )
                    {
                        Console.WriteLine($"Due Date: {todo.DueDate}");
                    }
                    Console.WriteLine($"");
                }
            }

            // command "remove" by Guid
            if (command.StartsWith("remove"))
            {
                if (todoList.Count <= 0)
                {
                    Console.WriteLine("You don't have any todo to remove.");
                    continue;
                }

                string[] todoParams = command.Split(" ");
                if (todoParams.Count() == 1)
                {
                    Console.WriteLine("Please Enter id to remove like: remove <Todo-Id>");
                    continue;
                }

                Boolean hasData = false;
                for (int i = 0; i < todoList.Count; i++)
                {
                    if (todoList[i].Id.ToString() == todoParams[1])
                    {
                        Console.WriteLine("---------------------------------------------------------------------\n");
                        Console.WriteLine("This is your todo That you want to remove.");
                        Console.WriteLine($"Id: {todoList[i].Id}\nTitle: {todoList[i].Title}\nDescription: {todoList[i].Description}");
                        if (todoList[i].DueDate != null)
                        {
                            Console.WriteLine($"Due Date: {todoList[i].DueDate}");
                        }
                        Console.WriteLine("Confirm to remove by pass [y/n]");
                        while (true)
                        {
                            string comfirm = Console.ReadLine();
                            if (string.IsNullOrEmpty(comfirm))
                            {
                                Console.WriteLine("Please enter your confirmation. [y/n]");
                                continue;
                            } else if (comfirm.ToLower() == "y")
                            {
                                todoList.RemoveAt(i);
                                hasData = true;
                                Console.WriteLine("Remove Succeed");
                                break;
                            } else
                            {
                                hasData = true;
                                break;
                            }
                        }
                    }
                }

                if (!hasData)
                {
                    Console.WriteLine("Sorry you don't have this todo id in your list. Please check id and try again.");
                }
            }

            // command "filter" by Title
            if (command.StartsWith("filter"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine("USAGE: filter <title>");
                    continue;
                }

                string filterTitle = todoParams[1];
                var filteredTodos = todoList.Where(t => t.Title.Contains(filterTitle, StringComparison.OrdinalIgnoreCase)).ToList();

                if (filteredTodos.Count <= 0)
                {
                    Console.WriteLine($"No todos found with title containing '{filterTitle}'");
                }
                else
                {
                    foreach (var todo in filteredTodos)
                    {
                        Console.WriteLine("---------------------------------------------------------------------\n");
                        Console.WriteLine($"Id: {todo.Id}\nTitle: {todo.Title}\nDescription: {todo.Description}");
                        if (todo.DueDate != null)
                        {
                            Console.WriteLine($"Due Date: {todo.DueDate}");
                        }
                        Console.WriteLine($"");
                    }
                }
            }


            Console.WriteLine("Your command: {0}", command);
        }
    }
}
