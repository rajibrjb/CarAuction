using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionFinishedConsumer: IConsumer<AuctionFinished>
    {
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("AuctionFinishedConsumer: " + context.Message.AuctionId + " " + context.Message.ItemSold + " " + context.Message.Winner + " " + context.Message.Amount);

            var auction = await DB.Find<Item>()
                .OneAsync(context.Message.AuctionId);

            if(context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            await auction.SaveAsync();

        }
    }
}