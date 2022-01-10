using AutoMapper;
using CycleHire.Core.Dtos;
using CycleHire.Core.Models;
using CycleHire.Core.ViewModels;
using CycleHire.Core.ViewModels.BookingViewModels;
using CycleHire.Core.ViewModels.ListingViewModels;
using CycleHire.Core.ViewModels.ReviewViewModels;
using System;
using System.Linq;

namespace CycleHire.Utilites.Mapping
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            // ViewModel to Domain Model
            CreateMap<ListingForm, Listing>()
                .ForMember(dest => dest.Accessories,
                    src => src.MapFrom(a =>
                        a.IncludedAccessories.Select(aid => new ListingAccessory() { AccessoryId = aid })));

            CreateMap<AvailabilityViewModel, Availability>()
                .AfterMap((avm, av) =>
                {
                    av.Monday = avm.SelectedDayOfWeek.Contains("Monday");
                    av.Tuesday = avm.SelectedDayOfWeek.Contains("Tuesday");
                    av.Wenesday = avm.SelectedDayOfWeek.Contains("Wenesday");
                    av.Thursday = avm.SelectedDayOfWeek.Contains("Thursday");
                    av.Friday = avm.SelectedDayOfWeek.Contains("Friday");
                    av.Saturday = avm.SelectedDayOfWeek.Contains("Saturday");
                    av.Sunday = avm.SelectedDayOfWeek.Contains("Sunday");
                });

            CreateMap<HostReviewViewModel, HostReview>();
            CreateMap<TenantReviewViewModel, TenantReview>();

            // Domain Model to ViewModel
            CreateMap<ApplicationUser, ApplicationUserViewModel>();

            CreateMap<Listing, ListingDetailsViewModel>()
                .ForMember(dest => dest.Accessories, src => src.Ignore())
                .AfterMap((l, ldvm) =>
                {
                    var accessories = l.Accessories
                        .Select(acc => new KeyValueViewModel()
                        {
                            Name = acc.Accessory.Name,
                            Id = acc.AccessoryId
                        })
                        .ToList();

                    ldvm.Accessories = accessories;
                });

            CreateMap<Accessory, KeyValueViewModel>();
            CreateMap<ListingImage, string>().ConvertUsing(l => l.ImagePublicId);

            CreateMap<BookingFormViewModel, Booking>()
                .ForMember(dest => dest.UserMessage, src => src.MapFrom(b => b.MessageToOwner))
                .ForMember(dest => dest.ListingId, src => src.MapFrom(b => b.Listing.Id))
                .ForMember(dest => dest.From, src => src.MapFrom(b => b.Listing.From))
                .ForMember(dest => dest.To, src => src.MapFrom(b => b.Listing.To))
                .ForMember(dest => dest.UserMobileNumber, src => src.MapFrom(b => b.UserMobileNumber))
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<Booking, BookingFormViewModel>()
                .ForMember(dest => dest.Listing, src => src.MapFrom(b => b.Listing))
                .ForMember(dest => dest.MessageToOwner, src => src.MapFrom(b => b.UserMessage))
                .ForMember(dest => dest.StripeToken, src => src.MapFrom(b => b.StripeCustomerIdToken));

            CreateMap<Booking, BookingDetailsViewModel>()
                .ForMember(dest => dest.Booking, src => src.MapFrom(b => b))
                .ForMember(dest => dest.CardLast4Digits, src => src.Ignore())
                .ForMember(dest => dest.CardType, src => src.Ignore());

            //string to image model
            CreateMap<string, ListingImage>()
                .ForMember(dest => dest.ImagePublicId, src => src.MapFrom(a => a));

            //domain to domain
            CreateMap<Listing, Booking>()
                .ForMember(dest => dest.PricePerDay, src => src.MapFrom(l => l.Price))
                .ForMember(dest => dest.Owner, src => src.MapFrom(l => l.User))
                .ForMember(dest => dest.Listing, src => src.MapFrom(l => l))
                .ForAllOtherMembers(src => src.Ignore());

            //domain to dtos
            CreateMap<Node, NodeDto>()
                .ForMember(b => b.Text, b => b.MapFrom(f => f.Name))
                .ForMember(b => b.Id, b => b.MapFrom(f => f.Id))
                .ForMember(b => b.Parent, b => b.MapFrom(f => f.ParentId == null ? "#" : f.ParentId.ToString()))
                .ForMember(b => b.Type, b => b.MapFrom(f => f.GetType().Name))
                .ForMember(b => b.ReadOnly, b => b.MapFrom(f => f.ReadOnly));

            CreateMap<FolderItem, NodeDto>()
                .ForMember(b => b.Text, b => b.MapFrom(f => f.Name))
                .ForMember(b => b.Id, b => b.MapFrom(f => f.Id))
                .ForMember(b => b.Parent, b => b.MapFrom(f => f.ParentId == null ? "#" : f.ParentId.ToString()))
                .ForMember(b => b.Type, b => b.MapFrom(f => f.GetType().Name))
                .ForMember(b => b.ReadOnly, b => b.MapFrom(f => f.ReadOnly));

            CreateMap<RouteItem, NodeDto>()
                .ForMember(b => b.Text, b => b.MapFrom(f => f.Name))
                .ForMember(b => b.Id, b => b.MapFrom(f => f.Id))
                .ForMember(b => b.Parent, b => b.MapFrom(f => f.ParentId == null ? "#" : f.ParentId.ToString()))
                .ForMember(b => b.Type, b => b.MapFrom(f => f.GetType().Name))
                .ForMember(b => b.ReadOnly, b => b.MapFrom(f => f.ReadOnly));

            //CreateMap<RouteItem, RouteItemDto>()
            //    .ForMember(b => b.Origin.Latitude, b => b.MapFrom(f => f.OriginLatitude))
            //    .ForMember(b => b.Origin.Longitude, b => b.MapFrom(f => f.OriginLongitude))
            //    .ForMember(b => b.Destination.Latitude, b => b.MapFrom(f => f.DestinationLatitude))
            //    .ForMember(b => b.Destination.Longitude, b => b.MapFrom(f => f.DestinationLongitude))
            //    .ForAllMembers(b => b.Ignore());
        }
    }
}
