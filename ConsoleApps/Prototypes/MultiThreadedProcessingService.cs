using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApps.Prototypes
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Region { get; set; }
        public decimal Amount { get; set; }
    }

    public class MultiThreadedProcessingService
    {
        private readonly ConcurrentDictionary<int, decimal> _orderSales = new ConcurrentDictionary<int, decimal>();
        private int _totalOrders;
        private readonly object _lock = new object();
        private readonly Random _random = new Random();


        public async Task StartProcessingData()
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(async () =>
                {
                    while (true)
                    {
                        await ProcessOrdersAsync();
                    }
                }).Start();
            }
        }
        /// <summary>
        /// Processes orders from multiple batches concurrently.
        /// </summary>
        /// <param name="batchIds">A collection of batch IDs to process.</param>
        private async Task ProcessOrdersAsync()
        {
            await Task.Run(() => ProcessOrderBatch(_random.Next(1, 21)));
            PrintAnalytics();
        }

        /// <summary>
        /// Processes a single batch of orders.
        /// </summary>
        private void ProcessOrderBatch(int batchId)
        {
            List<Order> orders = FetchOrdersFromDatabase(batchId);

            foreach (var order in orders)
            {
                // Update the sales thread-safe way
                _orderSales.AddOrUpdate(order.OrderId, order.Amount, (key, currentTotal) => currentTotal + order.Amount);
                // Update the total orders count using Interlocked to ensure thread safety
                Interlocked.Increment(ref _totalOrders);
            }
        }

        /// <summary>
        /// Simulates fetching orders from a database.
        /// </summary>
        private List<Order> FetchOrdersFromDatabase(int batchId)
        {
            // Simulate delay for database access.
            Thread.Sleep(100);
            return new List<Order>
            {
                new Order { OrderId = batchId, Region = "North", Amount = 100.00m },
            };
        }

        public void PrintAnalytics()
        {
            lock (_lock)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Total Orders Processed: " + _totalOrders);
                foreach (var order in _orderSales)
                {
                    Console.WriteLine($"Order: {order.Key}, Total Sales: {order.Value}");
                }
            }
        }
    }
}
