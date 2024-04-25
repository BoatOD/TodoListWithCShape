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

            // command "remove" by Guid

            // command "filter" by Title


            Console.WriteLine("Your command: {0}", command);
        }
    }
}
