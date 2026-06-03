using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Catalogs;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InnSystem.BLL.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IGenericRepository<RoomType> _roomTypeRepository;
        private readonly IGenericRepository<RoomStatus> _roomStatusRepository;
        private readonly IGenericRepository<BookingStatus> _bookingStatusRepository;
        private readonly IGenericRepository<Service> _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            IGenericRepository<RoomType> roomTypeRepository, 
            IGenericRepository<RoomStatus> roomStatusRepository,
            IGenericRepository<BookingStatus> bookingStatusRepository,
            IGenericRepository<Service> serviceRepository,
            IMapper mapper, 
            ILogger<CatalogService> logger)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomStatusRepository = roomStatusRepository;
            _bookingStatusRepository = bookingStatusRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CatalogDTO> GetAllCatalogsAsync()
        {
            try
            {
                //extraccion de tablas 
                var types = await _roomTypeRepository.Query().ToListAsync();
                var roomStatuses = await _roomStatusRepository.Query().ToListAsync();
                var bookingStatuses = await _bookingStatusRepository.Query().ToListAsync();
                var services = await _serviceRepository.Query().ToListAsync();

                return new CatalogDTO
                {
                    RoomTypes = _mapper.Map<List<RoomTypeDTO>>(types),
                    RoomStatuses = _mapper.Map<List<StatusDTO>>(roomStatuses),
                    BookingStatuses = _mapper.Map<List<StatusDTO>>(bookingStatuses),
                    Services = _mapper.Map<List<InnSystem.DTO.Rooms.ServiceDTO>>(services)
                };

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los catalogos de habitaciones");
                throw;
            }
        }
    }
}
