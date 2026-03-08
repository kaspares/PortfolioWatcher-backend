using AutoMapper;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Application.Profiles;

public class PortfolioItemProfile : Profile
{
    public PortfolioItemProfile()
    {
        CreateMap<PortfolioItem, PortfolioItemDto>();
        CreateMap<PositionComment, ItemCommentDto>();
        CreateMap<PortfolioItemDto, PortfolioItem>();
        CreateMap<CreatePortfolioItemDto, PortfolioItem>();
        CreateMap<ItemCommentDto, PositionComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdatePortfolioItemDto, PortfolioItem>();
    }
}
