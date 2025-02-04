using ServerApp.Services;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1 - Start HTTP Server");
            Console.WriteLine("2 - Run Batch Service Manually");
            string option = Console.ReadLine();

            if (option == "1")
            {
                // Start the HTTP server on port 8080
                Server.Server server = new Server.Server("http://localhost:8080/");
                server.Start();

                Console.WriteLine("Press ENTER to stop the server.");
                Console.ReadLine();
                server.Stop();
            }
            else if (option == "2")
            {
                // Run the batch service once
                UserBatchService batchService = new UserBatchService();
                batchService.ProcessBatch();

                // Optionally, schedule the batch service.
                // For example, to run after 10 seconds and then every 60 seconds:
                // batchService.StartScheduledBatch(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(60));

                Console.WriteLine("Batch service executed. Press ENTER to exit.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid option. Exiting.");
            }
        }
    }
}
