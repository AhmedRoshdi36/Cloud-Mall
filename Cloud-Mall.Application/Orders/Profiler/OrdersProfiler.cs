using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Application.Common.Mappings
{
    public class OrdersProfiler : Profile
    {
        public OrdersProfiler()
        {
            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name)
            )
            .ForMember(
                dest => dest.ProductImageUrl,
                opt => opt.MapFrom(src => src.Product.ImagesURL)
            );

            CreateMap<VendorOrder, VendorOrderDto>()
                .ForMember(
                    dest => dest.OrderDate,
                    opt => opt.MapFrom(src => src.OrderDate.ToString("yyyy-MM-dd")) // <-- ADD THIS MAPPING RULE
                )
                .ForMember(
                    dest => dest.StoreName,
                    opt => opt.MapFrom(src => src.Store.Name)
                )
                .ForMember(
                    dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.CustomerOrder.Client.Name)
                )
                .ForMember(
                    dest => dest.ShippingAddress,
                    opt => opt.MapFrom(src => $"{src.ShippingStreetAddress}, {src.ShippingCity}")
                );

            // --- THIS IS THE UPDATED PART ---
            CreateMap<CustomerOrder, CustomerOrderDetailsDto>()
                .ForMember(
                    dest => dest.OverallStatus,
                    opt => opt.MapFrom(src => CalculateOverallStatus(src.VendorOrders))
                );
        }

        // We moved the calculation logic here so it's reusable for any mapping.
        private string CalculateOverallStatus(ICollection<VendorOrder> vendorOrders)
        {
            if (vendorOrders == null || !vendorOrders.Any()) return "Pending";

            var statuses = vendorOrders.Select(vo => vo.Status).Distinct().ToList();

            if (statuses.Count == 1)
            {
                var singleStatus = statuses.First();
                return singleStatus == VendorOrderStatus.Fulfilled ? "Completed" : singleStatus.ToString();
            }
            if (statuses.All(s => s == VendorOrderStatus.Fulfilled || s == VendorOrderStatus.Cancelled))
            {
                return "Completed";
            }
            if (statuses.Any(s => s == VendorOrderStatus.Cancelled))
            {
                return "Partially Fulfilled";
            }
            return "In Progress";
        }
    }
}