using My.Hydrator;
using My.Hydrator.Conventions;
using SJCNet.Samples.Performance.Serialization.Model;
using System;

namespace SJCNet.Samples.Performance.Serialization.Data
{
    public class DataGenerator
    {
        public Orders GenerateTestOrders(int orderCount, int maxProducts)
        {
            var results = new Orders();

            var hydrator = GetAndConfigureHydrator();
            var random = new Random();

            for (var x = 1; x <= orderCount; x++)
            {
                var order = hydrator.Hydrate<Order>();

                // Generate random number of products up to maxProducts
                var productCount = random.Next(1, maxProducts);
                for (var i = 1; i <= productCount; i++)
                {
                    order.Products.Add(hydrator.Hydrate<Product>());
                }

                results.Add(order);
            }

            return results;
        }

        private Hydrator GetAndConfigureHydrator()
        {
            var hydrator = new Hydrator();

            hydrator.Configure(x =>
            {
                x.For<Product>().Property(y => y.Price).Use(new IntTypeConvention(1, 300));
                x.For<Product>().Property(y => y.Name).Is(CommonType.AmericanLastName);
            });

            return hydrator;
        }

    }
}
