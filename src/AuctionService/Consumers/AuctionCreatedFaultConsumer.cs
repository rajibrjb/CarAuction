using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            Console.WriteLine("AuctionCreatedFaultConsumer: " + context.Message.Message);

            var exception = context.Message.Exceptions.First();

            if(exception.ExceptionType == typeof(ArgumentException).FullName)
            {
                context.Message.Message.Model = "TestBar";

                await context.Publish(context.Message.Message);
            }
            else 
            {
                Console.WriteLine("Not an ArgumentException- update some other service");
            }
        }
    }
}