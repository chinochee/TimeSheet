﻿using Data.Entities;
using Services.Dtos;

namespace Services
{
    public interface ICurrencyService
    {
        Task<CurrencyEntryDto[]> Get();
        void Post(Currency[] currency);
    }
}