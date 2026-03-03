using AutoMapper;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Application.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio, PortfolioSummaryDto>()
                .ForMember(p => p.ItemCount, d => d.MapFrom(s => s.Items.Count));

            CreateMap<Portfolio, PortfolioDetailDto>();
            CreateMap<Portfolio, PortfolioDetailDto>();
            CreateMap<PortfolioItem, PortfolioItemDto>();
            CreateMap<PositionComment, ItemCommentDto>();
            CreateMap<CreatePortfolioDto, Portfolio>();
            CreateMap<UpdatePortfolioDto, Portfolio>();

        }
    }
}
