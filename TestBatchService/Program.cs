using ServerApp.Services;

namespace TestBatchService
{
    class Program
    {
        static void Main(string[] args)
        {
            UserBatchService batchService = new UserBatchService();
            // Run the batch processing once
            batchService.ProcessBatch();

            // Alternatively, to test scheduling, uncomment the following lines:
            // TimeSpan dueTime = TimeSpan.FromSeconds(5);
            // TimeSpan interval = TimeSpan.FromMinutes(1);
            // batchService.StartScheduledBatch(dueTime, interval);
            // Console.WriteLine("Press ENTER to stop the scheduled batch service.");
            // Console.ReadLine();

            Console.WriteLine("Batch service test completed. Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}

