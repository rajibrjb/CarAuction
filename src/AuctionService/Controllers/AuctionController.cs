using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDbContext context;
        private readonly IMapper mapper;
        public AuctionController(AuctionDbContext _context, IMapper _mapper)
        {
            this.context = _context;
            this.mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAuctions()
        {
            var auctions = await context.Auctions
                .Include(x=>x.Item)
                .OrderBy(x =>x.Item.Make)
                .ToListAsync();

            return mapper.Map<List<AuctionDto>>(auctions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await context.Auctions
                .Include(x=>x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null)
            {
                return NotFound();
            }

            return mapper.Map<AuctionDto>(auction);
        }
        [HttpPost]
        public async Task<ActionResult> CreateAuction(CreateAuctionDto createAuctionDto)
        {
            var auction = mapper.Map<Auction>(createAuctionDto);
            // TODO: Add Current user as seller
            auction.Seller = "Fake Seller";
            context.Add(auction);
            var result = await context.SaveChangesAsync() > 0;
            if(!result)
            {
                return BadRequest("Failed to create auction! Please try again.");
            }

            return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            // TODO: Add Current user as seller
            
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.UpdatedAt = DateTime.UtcNow;

            var result = await context.SaveChangesAsync() > 0;
            if(!result)
            {
                return BadRequest("Failed to update auction! Please try again.");
            }
            

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            // TODO: Add Current user as seller
            context.Auctions.Remove(auction);
            var result = await context.SaveChangesAsync() > 0;
            if(!result)
            {
                return BadRequest("Failed to delete auction! Please try again.");
            }

            return Ok();

        }
    }
}