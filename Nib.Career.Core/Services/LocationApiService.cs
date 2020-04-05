using Microsoft.Extensions.Options;
using Nib.Career.Core.Configs;
using Nib.Career.Core.Entities;
using Nib.Career.Core.Errors;
using Nib.Career.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nib.Career.Core.Services
{
    public class LocationApiService : ILocationApiService
    {
        private readonly LocationApiOptions _locationApiOptions;
        private readonly ISimpleMemoryCache _simpleMemoryCache;
        private readonly HttpClient _client = new HttpClient();

        public LocationApiService(IOptionsMonitor<LocationApiOptions> optionsAccessor, ISimpleMemoryCache simpleMemoryCache)
        {
            _locationApiOptions = optionsAccessor.CurrentValue;
            _simpleMemoryCache = simpleMemoryCache;
        }

        public async Task<IList<Location>> GetAsync()
        {
            var response = await _client.GetAsync(_locationApiOptions.HostAddress);

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    return await _simpleMemoryCache.GetOrCreate<IList<Location>>($"list-{nameof(Location)}", async () =>
                    {
                        return await JsonSerializer.DeserializeAsync<IList<Location>>(await response.Content.ReadAsStreamAsync());
                    }, slidingExpiryInMin: 120);
                }

                return new List<Location>();
            }
            else
            {
                throw new HttpListenerException((int)response.StatusCode, ErrorCodes.Location.Error);
            }
        }
    }
}
