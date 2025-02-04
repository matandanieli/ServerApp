using System.Text;
using System.Text.Json;
using ServerApp.Models;

namespace TestServerClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            string baseUrl = "http://localhost:8080";

            // 1. Add a new user
            User newUser = new User { Name = "Matan Danieli", Email = "matan@developer.com", Password = "secret" };
            string userJson = JsonSerializer.Serialize(newUser);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");
            var addResponse = await client.PostAsync($"{baseUrl}/user/add", content);
            Console.WriteLine("Add User Response: " + await addResponse.Content.ReadAsStringAsync());

            // 2. Retrieve the user (assuming the user got ID 1)
            var getResponse = await client.GetAsync($"{baseUrl}/user/get?id=1");
            Console.WriteLine("Get User Response: " + await getResponse.Content.ReadAsStringAsync());

            // 3. Update the user (change the name)
            newUser.ID = 1;
            newUser.Name = "Matan Danieli Updated";
            userJson = JsonSerializer.Serialize(newUser);
            content = new StringContent(userJson, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync($"{baseUrl}/user/update", content);
            Console.WriteLine("Update User Response: " + await updateResponse.Content.ReadAsStringAsync());

            // 4. Delete the user
            var deleteResponse = await client.DeleteAsync($"{baseUrl}/user/delete?id=1");
            Console.WriteLine("Delete User Response: " + await deleteResponse.Content.ReadAsStringAsync());
        }
    }
}

