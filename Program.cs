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
                if (todoParams.Count() <= 1)
                {
                    Console.WriteLine("Please Enter id to remove like: remove <Todo-Id>");
                    continue;
                }

                Boolean hasData = false;

                var dataToRemove = todoList.Where(t => t.Id.ToString().Contains(todoParams[1], StringComparison.OrdinalIgnoreCase)).ToList();

                if (dataToRemove.Count() <= 0)
                {
                    Console.WriteLine("Sorry you don't have this todo id in your list. Please check id and try again.");
                    continue;
                } else
                {
                    Console.WriteLine("---------------------------------------------------------------------\n\nThis is your todo That you want to remove.");
                    Console.WriteLine($"Id: {dataToRemove[0].Id}\nTitle: {dataToRemove[0].Title}\nDescription: {dataToRemove[0].Description} {0}");
                    if (dataToRemove[0].DueDate != null)
                    {
                        Console.WriteLine($"Due Date: {dataToRemove[0].DueDate}");
                    }
                    Console.WriteLine("---------------------------------------------------------------------\n\nConfirm to remove by pass [y/n]");
                    while (true)
                    {
                        string comfirm = Console.ReadLine();
                        if (string.IsNullOrEmpty(comfirm))
                        {
                            Console.WriteLine("Please enter your confirmation. [y/n]");
                            continue;
                        }
                        else if (comfirm.ToLower() == "y")
                        {
                            todoList.Remove(dataToRemove[0]);
                            Console.WriteLine("Remove Succeed");
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
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
