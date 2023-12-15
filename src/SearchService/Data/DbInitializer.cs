using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication webApplication)
        {
            // Connect to MongoDB
            await DB.InitAsync("SearchDb",MongoClientSettings
                .FromConnectionString(webApplication.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            using var scope = webApplication.Services.CreateScope();

            var auctionServiceHttpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

            var items = await auctionServiceHttpClient.GetItemsFromSearchDb();

            Console.WriteLine($"Found {items.Count} items from AuctionService");

            if(items.Count > 0)
            {
                await DB.SaveAsync(items);
            }
        }
        
    }
}