using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
        private ILogger<GeoCoordsService> _logger;

        public GeoCoordsService(ILogger<GeoCoordsService> logger)
        {
            _logger = logger;
        }

        //public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        //{
        //    var result = new GeoCoordsResult()
        //    {
        //        Success = false,
        //        Message = "Failed to get coordinates"
        //    };

        //    var apiKey = "";
        //}
    }
}
