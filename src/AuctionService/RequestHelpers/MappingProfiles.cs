using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>().ForMember(x => x.Item, opt => opt.MapFrom(src => src));
            CreateMap<CreateAuctionDto, Item>();
            CreateMap<AuctionDto,AuctionCreated>();
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(x => x.Item);
            CreateMap<Item,AuctionUpdated>();
        }
    }
}