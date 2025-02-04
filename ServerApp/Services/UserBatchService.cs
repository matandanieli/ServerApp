using ServerApp.Controller;

namespace ServerApp.Services
{
    public class UserBatchService
    {
        private readonly UserController userController;

        public UserBatchService()
        {
            userController = new UserController();
        }

        public void ProcessBatch()
        {
            Console.WriteLine($"Batch process started at {DateTime.Now}");
            var users = userController.GetAllUsers();
            foreach (var user in users)
            {
                // Simulate sending an email
                Console.WriteLine($"Sending email to {user.Email}...");
                // I can add more logic here (update a 'LastNotified' etc)
            }
            Console.WriteLine("Batch process completed.\n");
        }


        //for scheduled
        public void StartScheduledBatch(TimeSpan dueTime, TimeSpan interval)
        {
            Timer timer = new Timer(x => ProcessBatch(), null, dueTime, interval);
            Console.WriteLine("Scheduled batch service started.");
        }
    }
}
