using Microsoft.Extensions.Options;
using Nib.Career.Core.Configs;
using Nib.Career.Core.Entities;
using Nib.Career.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nib.Career.Core.Services
{
    public class FileStorageService<T> : IFileStorageService<T> where T : BaseEntity
    {
        private readonly FileStorageOptions _fileStorageOptions;
        private readonly ISimpleMemoryCache _simpleMemoryCache;

        public FileStorageService(IOptionsMonitor<FileStorageOptions> optionsAccessor, ISimpleMemoryCache simpleMemoryCache)
        {
            _fileStorageOptions = optionsAccessor.CurrentValue;
            _simpleMemoryCache = simpleMemoryCache;
        }

        public async Task<IList<T>> GetAsync()
        {
            var result = await _simpleMemoryCache.GetOrCreate<IList<T>>($"list-{nameof(T)}", async () =>
            {
                using (StreamReader sr = File.OpenText(_fileStorageOptions.FileName))
                {
                    StringBuilder sb = new StringBuilder();

                    string s = String.Empty;
                    while ((s = await sr.ReadLineAsync()) != null)
                    {
                        sb.Append(s);
                    }

                    if (sb.Length == 0)
                        return null;

                    MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));
                    return await JsonSerializer.DeserializeAsync<IList<T>>(stream);
                }
            });

            if (result != null)
                return result;

            return new List<T>();
        }
    }
}
