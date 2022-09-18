﻿using HotelListing.API.Data;

namespace HotelListing.API.Core.Interfaces
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetails(int id);
    }
}
