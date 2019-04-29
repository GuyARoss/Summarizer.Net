using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Summarizer.Infrastructure.Dictionary.Entities;

namespace Summarizer.Infrastructure.Dictionary
{
    public class DictionaryApi
    {        
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://www.dictionaryapi.com/api/v3/references/thesaurus/json";

        public DictionaryApi() { }
        public DictionaryApi(string apiKey)
        {
            _apiKey = apiKey;
        }
        
        public async Task<List<DictionaryEntity>> Invoke(string word)
        {
            string uri = string.Format("{0}/{1}?key={2}", _apiUrl, word, _apiKey);

            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);

            string responseBody = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return ExtrapolateDataEntityFromResponse(responseBody);
            }
                        

            return null;
        }
        private static List<DictionaryEntity> ExtrapolateDataEntityFromResponse(string response)
        {
            if (response.Contains("meta")) // returns an array of the words for some reason?
            {
                return JsonConvert.DeserializeObject<List<DictionaryEntity>>(response);
            }

            return null;
        }
    }
}
