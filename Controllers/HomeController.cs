using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using scalingtestwa56webapp.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace scalingtestwa56webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            int input = 5;

            try
            {
                if(Request.Query["value"].ToString() is not null)
            {
                    input = Convert.ToInt32(Request.Query["value"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                input = 5;
            }

            Task.Factory.StartNew(() =>
            {
                RunForFiveMinutes(input);
            });

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static void RunForFiveMinutes(int seconds)
        {
            int coreCount = Environment.ProcessorCount;

            CancellationTokenSource cts = new CancellationTokenSource();

            // Start a CPU-intensive task on each core
            Task[] tasks = new Task[coreCount];

            for (int i = 0; i < coreCount; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Work(cts.Token), TaskCreationOptions.LongRunning);
            }

            // Delay for 5 minutes and then cancel all CPU-intensive tasks
            Thread.Sleep(TimeSpan.FromSeconds(5));
            cts.Cancel();

            Task.WhenAll(tasks).Wait();
        }

        static void Work(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Perform some CPU-intensive work here
            }
        }
    }
}
