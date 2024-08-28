using WebApi.Models;
using System.Text.Json;
using WebApi.Controllers;
using System.Reflection.Metadata.Ecma335;

namespace WebApi.DogsService
{
    public interface IDogBreedService
    {
         Task<IEnumerable<Data>> GetBreeds(bool hypoallergenic);
    }


    public class DogBreedService : IDogBreedService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DogBreedService> _logger;

        public DogBreedService(HttpClient httpClient, ILogger<DogBreedService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// *** CHALLENGE #1 *************************************************************
        /// Create a method that will call the following api and returns a Breed object.
        /// Url: https://dogapi.dog/api/v2/breeds/{id} . Use id '68f47c5a-5115-47cd-9849-e45d3c378f12' to get back a breed object.
        /// If 'id' does not return a Breed, then return a null object.
        /// </summary>
        /// <param name="id"></param>



        /// <summary>
        /// *** CHALLENGE #2 *************************************************************
        /// Create a method that will return a list of Breed objects where the 
        /// 'hypoallergenic' property set to true. You will need to call the 'breeds' api
        /// for each page number. For every Breed object returned from each page, you are 
        /// going to check if the attributes.hypoallergenic property is set to true. Example 
        /// URL for Breeds with page number: https://dogapi.dog/api/v2/breeds/?page[number]={pageNum}
        /// Assume that the total number of pages is 29 for this test.
        /// <param name="hypoallergenic">true or false</param>
        /// </summary>

        public async Task<IEnumerable<Data>> GetBreeds(bool hypoallergenic)
        {
            List<Data> result = new List<Data>();
            BreedPage page;
            for (int i = 1; i <= 29; i++)
            {
                string uri = $"https://dogapi.dog/api/v2/breeds/?page[number]={i}";
                try
                {
                    var pageresponse = await _httpClient.GetAsync(uri);

                    if (pageresponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        page = await pageresponse.Content.ReadFromJsonAsync<BreedPage>();
                        var breedData = page.data.Where(x => x.attributes.hypoallergenic == true);
                        result.AddRange(breedData);
                    }
                } catch (Exception ex)
                {
                    _logger.LogWarning($"Page {i} threw an exception with message " + ex.ToString());
                }

            }
            if (result.Count == 0)
                return null;
            return result;
        }
    }
        
        /// <summary>
        /// *** CHALLENGE #3 *************************************************************
        /// Lets enhance the performance of Challenge #2 to make the api call to each of the 
        /// Breeds page number in parallel. We still want to return a list of 'Data' objects
        /// where the attributes.hypoallergenic property is set to true. Assume that the total 
        /// number of pages is 29 for this test.
        /// <param name="hypoallergenic">true or false</param>
        /// </summary>
    }

