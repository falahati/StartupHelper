using System;

namespace StartupHelper.Sample
{
    internal class Program
    {
        public static StartupManager StartupController =
            new StartupManager("SAMPLE PROGRAM",
                RegistrationScope.Local);

        private static void Main()
        {
            Console.WriteLine("---------------------INFO---------------------");
            Console.WriteLine("Arguments: " + string.Join(" ", StartupController.CommandLineArguments));
            Console.WriteLine("Is this session elevated? " + StartupManager.IsElevated);
            if (StartupController.IsStartedUp)
            {
                Console.WriteLine("This program started automatically");
                Console.WriteLine("Press 'Enter' to exit.");
                Console.ReadLine();
            }
            else
            {
                if (!StartupController.IsRegistered)
                {
                    Console.WriteLine("Program not registered, registering ...");
                    Console.WriteLine(StartupController.Register("--nice /argument") ? "Done" : "Failed");
                }
                else
                {
                    Console.WriteLine("Program already registered");
                }
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("This program started manually");
                Console.WriteLine("Press 'Enter' to exit, or any other key to unregister.");
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.WriteLine("Unregistering ...");
                    Console.WriteLine(StartupController.Unregister() ? "Done" : "Failed");
                    Console.WriteLine("Press 'Enter' to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}