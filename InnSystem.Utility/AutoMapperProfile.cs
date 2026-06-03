using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InnSystem.Model;
using InnSystem.DTO.Users;
using InnSystem.DTO.Rooms;
using InnSystem.DTO.Bookings;
using InnSystem.DTO.Payments;
using InnSystem.DTO.Roles;
using InnSystem.DTO.Catalogs;

namespace InnSystem.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Roles
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<RoleCreateDTO, Role>();
            CreateMap<RoleUpdateDTO, Role>();
            CreateMap<Module, ModuleDTO>();
            #endregion Roles

            #region Users
            CreateMap<User, UserDTO>()
                .ForMember(destino => destino.RoleName, opt => opt.MapFrom(origen => origen.IdRoleNavigation.RoleName));

            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<LoginDTO, User>();
            CreateMap<UserChangeRoleDTO, User>();
            CreateMap<User, SessionDTO>()
                .ForMember(destino =>
                    destino.RoleName,
                    opt => opt.MapFrom(origen => origen.IdRoleNavigation.RoleName))

                .ForMember(destino =>
                        destino.AllowedModules,
                        opt => opt.MapFrom(origen => origen.IdRoleNavigation.IdModules)
                    );

            #endregion Users

            #region Rooms
            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceCreateDTO, Service>();
            CreateMap<ServiceUpdateDTO, Service>();

            CreateMap<RoomImage, RoomImageDTO>();

            CreateMap<Room, RoomDTO>()
                .ForMember(destino => destino.RoomType, opt => opt.MapFrom(origen => origen.IdRoomTypeNavigation != null ? origen.IdRoomTypeNavigation.Name : null))
                .ForMember(destino => destino.GuestCapacity, opt => opt.MapFrom(origen => origen.IdRoomTypeNavigation != null ? origen.IdRoomTypeNavigation.GuestCapacity : 0))
                .ForMember(destino => destino.OperationalStatus, opt => opt.MapFrom(origen => origen.IdStatusNavigation.Name))
                .ForMember(destino => destino.Services, opt => opt.MapFrom(origen => origen.IdServices))
                .ForMember(destino => destino.Images, opt => opt.MapFrom(origen => origen.RoomImages));

            CreateMap<RoomCreateDTO, Room>()
                .ForMember(dest => dest.IdServices, opt => opt.Ignore())
                .ForMember(dest => dest.RoomImages, opt => opt.Ignore());

            CreateMap<RoomUpdateDTO, Room>()
                .ForMember(dest => dest.IdServices, opt => opt.Ignore())
                .ForMember(dest => dest.RoomImages, opt => opt.Ignore());
            #endregion Rooms

            #region Bookings
            CreateMap<Booking, BookingDTO>()
                .ForMember(destino => destino.FirstName, opt => opt.MapFrom(origen => origen.IdUserNavigation.FirstName))
                .ForMember(destino => destino.LastName,  opt => opt.MapFrom(origen => origen.IdUserNavigation.LastName))
                .ForMember(destino => destino.Email,     opt => opt.MapFrom(origen => origen.IdUserNavigation.Email))
                .ForMember(destino => destino.Phone,     opt => opt.MapFrom(origen => origen.IdUserNavigation.Phone))
                .ForMember(destino => destino.DocumentId,opt => opt.MapFrom(origen => origen.IdUserNavigation.DocumentId))
                .ForMember(destino => destino.Status,    opt => opt.MapFrom(origen => origen.IdStatusNavigation.Name))
                .ForMember(destino => destino.RoomNumber, opt => opt.MapFrom(origen => origen.IdRoomNavigation.RoomNumber));

            CreateMap<BookingCreateDTO, Booking>();
            #endregion Bookings

            #region Payments
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<PaymentCreateDTO, Payment>();
            #endregion Payments

            #region Catalogs
            CreateMap<RoomType, RoomTypeDTO>().ReverseMap();
            CreateMap<RoomTypeCreateDTO, RoomType>();
            CreateMap<RoomTypeUpdateDTO, RoomType>();
            
            CreateMap<Season, SeasonDTO>().ReverseMap();
            CreateMap<SeasonCreateDTO, Season>();
            CreateMap<SeasonUpdateDTO, Season>();

            CreateMap<RoomStatus, StatusDTO>()
                .ForMember(destino => destino.Id, opt => opt.MapFrom(origen => origen.IdStatus))
                .ReverseMap()
                .ForMember(destino => destino.IdStatus, opt => opt.MapFrom(origen => origen.Id));
                
            CreateMap<BookingStatus, StatusDTO>()
                .ForMember(destino => destino.Id, opt => opt.MapFrom(origen => origen.IdStatus))
                .ReverseMap()
                .ForMember(destino => destino.IdStatus, opt => opt.MapFrom(origen => origen.Id));
                
            CreateMap<PaymentStatus, StatusDTO>()
                .ForMember(destino => destino.Id, opt => opt.MapFrom(origen => origen.IdStatus))
                .ReverseMap()
                .ForMember(destino => destino.IdStatus, opt => opt.MapFrom(origen => origen.Id));

            #endregion Catalogs
        }
    }
}
