namespace StreamData.StockMarket.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Threading;
    using EventSource4Net;
    using Marvin.JsonPatch;
    using Marvin.JsonPatch.Operations;
    
    public class Program
    {
        public static void Main(params string[] args)
        {
            // Replace token value with your
            var token = "<YOUR STREAMDATAIO TOKEN>";
            var url = "https://streamdata.motwin.net/http://stockmarket.streamdata.io/prices" + "?X-Sd-Token=" + token;
            
            ShowMeHowToGetDataAndPatches(url);
        }


        public static void ShowMeHowToGetDataAndPatches(
            string apiUrl)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            EventSource es = new EventSource(new Uri(apiUrl), 1000);

            StockMarketOrders state = new StockMarketOrders();

            es.EventReceived += new EventHandler<ServerSentEventReceivedEventArgs>((o, e) =>
            {
            
                if (e.Message.EventType == "data")
                {
                    Console.WriteLine(e.Message.Data);
                    state = JsonConvert.DeserializeObject<StockMarketOrders>(e.Message.Data);
                    Console.WriteLine("state = {0}", state);
                }
                else if (e.Message.EventType == "patch")
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("patch is: {0} ", e.Message.Data);
                    var operations = JsonConvert.DeserializeObject<List<Operation<StockMarketOrders>>>(e.Message.Data);
                    var patchDocumentOperations = new JsonPatchDocument<StockMarketOrders>(operations);
                    patchDocumentOperations.ApplyTo(state);

                    Console.WriteLine("state = {0}", state);
                }
                else if (e.Message.EventType == "error")
                {
                    Console.WriteLine(e.Message.Data);
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        cancellationTokenSource.Cancel(false);
                    }
                    Console.WriteLine("stopped...");
                }
                else
                {
                    Console.WriteLine(e.Message.ToString());
                }
            });

            es.Start(cancellationTokenSource.Token);


            // Stop the console
            while ((Console.ReadKey().Key) != ConsoleKey.Escape)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel(false);
                }
                Console.WriteLine("stopped...");
            }

        }
    }

    public class Order
    {
        public string Title { get; set; }
        public int Price { get; set; }

        public Order Clone()
        {
            return new Order
            {
                Title = this.Title,
                Price = this.Price
            };
        }

        public override string ToString()
        {
            return "{ title:" + this.Title + ", price:" + this.Price + "}";
        }
    }

    public class StockMarketOrders : List<Order>
    {
        public int GetTotal()
        {
            return this.Select(x => x.Price).Sum();
        }

        public StockMarketOrders Clone()
        {
            var cloned = new StockMarketOrders();
            this.ForEach(order => cloned.Add(order.Clone()));
            return cloned;
        }

        public override string ToString()
        {
            return "[" + string.Join("," + Environment.NewLine, this) + "]";
        }
    }
}