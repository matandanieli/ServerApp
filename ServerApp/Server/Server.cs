using ServerApp.Controller;
using ServerApp.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ServerApp.Server
{
    public class Server
    {
        private readonly HttpListener listener;
        private readonly UserController userController;

        public Server(string prefix)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(prefix); // my case - "http://localhost:8080/"
            userController = new UserController();
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Server started. Listening for incoming requests...");
            Listen();
        }

        private async void Listen()
        {
            while (listener.IsListening)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    // Process the request on a thread pool thread
                    Task.Run(() => ProcessRequest(context));
                }
                catch (HttpListenerException) { /* Listener closed */ }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            string responseString = "";
            string method = context.Request.HttpMethod;
            string path = context.Request.Url.AbsolutePath;
            Console.WriteLine($"Received {method} request for {path}");

            try
            {
                if (path.Equals("/user/add", StringComparison.OrdinalIgnoreCase) && method == "POST")
                {
                    // Read JSON from the request body and deserialize into a User object
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        var json = reader.ReadToEnd();
                        var user = JsonSerializer.Deserialize<User>(json);
                        userController.AddUser(user);
                        responseString = "User added successfully.";
                    }
                }
                else if (path.Equals("/user/get", StringComparison.OrdinalIgnoreCase) && method == "GET")
                {
                    var idParam = context.Request.QueryString["id"];
                    if (int.TryParse(idParam, out int id))
                    {
                        var user = userController.GetUserById(id);
                        responseString = user != null ? JsonSerializer.Serialize(user) : "User not found.";
                    }
                    else
                    {
                        responseString = "Invalid id parameter.";
                    }
                }
                else if (path.Equals("/user/update", StringComparison.OrdinalIgnoreCase) && method == "PUT")
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        var json = reader.ReadToEnd();
                        var user = JsonSerializer.Deserialize<User>(json);
                        userController.UpdateUser(user);
                        responseString = "User updated successfully.";
                    }
                }
                else if (path.Equals("/user/delete", StringComparison.OrdinalIgnoreCase) && method == "DELETE")
                {
                    var idParam = context.Request.QueryString["id"];
                    if (int.TryParse(idParam, out int id))
                    {
                        userController.DeleteUser(id);
                        responseString = "User deleted successfully.";
                    }
                    else
                    {
                        responseString = "Invalid id parameter.";
                    }
                }
                else
                {
                    responseString = "Invalid endpoint or method.";
                }
            }
            catch (Exception ex)
            {
                responseString = "Error: " + ex.Message;
            }

            // Send response
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "application/json";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        public void Stop()
        {
            listener.Stop();
            listener.Close();
            Console.WriteLine("Server stopped.");
        }
    }
}
