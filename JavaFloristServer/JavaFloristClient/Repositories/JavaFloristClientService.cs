using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;

namespace JavaFloristClient.Repositories
{
    public class JavaFloristClientService : IJavaFloristClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public JavaFloristClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<SelectList> GetSelectList<TEntity>(string apiEndpoint, string valueField, string textField)
        {
            try
            {
                // Create an HttpClient instance
                var client = _httpClientFactory.CreateClient("MyApiClient"); // Replace "MyApiClient" with the name of your HttpClient instance.

                // Call the API to get the list of entities
                var response = await client.GetAsync(apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var entityJson = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON data to a list of TEntity objects
                    var entities = JsonConvert.DeserializeObject<List<TEntity>>(entityJson);

                    // Create a SelectList from the list of entities and return it
                    return new SelectList(entities, valueField, textField);
                }
                else
                {
                    // If the API request fails, return an empty SelectList and handle the error in the controller
                    return new SelectList(new List<TEntity>(), valueField, textField);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                return new SelectList(new List<TEntity>(), valueField, textField);
            }
        }
    }
}
