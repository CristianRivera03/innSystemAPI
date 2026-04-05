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
            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<RoomCreateDTO, Room>();
            CreateMap<RoomUpdateDTO, Room>();
            #endregion Rooms

            #region Bookings
            CreateMap<Booking, BookingDTO>()
                .ForMember(destino => destino.FirstName, opt => opt.MapFrom(origen => origen.IdUserNavigation.FirstName))
                .ForMember(destino => destino.LastName,  opt => opt.MapFrom(origen => origen.IdUserNavigation.LastName))
                .ForMember(destino => destino.Email,     opt => opt.MapFrom(origen => origen.IdUserNavigation.Email))
                .ForMember(destino => destino.Phone,     opt => opt.MapFrom(origen => origen.IdUserNavigation.Phone))
                .ForMember(destino => destino.DocumentId,opt => opt.MapFrom(origen => origen.IdUserNavigation.DocumentId));

            CreateMap<BookingCreateDTO, Booking>();
            #endregion Bookings

            #region Payments
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<PaymentCreateDTO, Payment>();
            #endregion Payments
        }
    }
}
