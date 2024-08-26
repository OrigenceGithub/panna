using WebApi.Models;
using System.Text.Json;
using WebApi.Controllers;

namespace WebApi.DogsService
{
    public interface IDogBreedService
    {
        Task<Breed> GetBreed(string id);
        Task<List<Data>> GetHypoAllergenicBreeds();

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
        public async Task<Breed> GetBreed(string id)
        {
            string serviceLoc = "https://dogapi.dog/api/v2/breeds/";
            string url = serviceLoc + id;
            HttpResponseMessage response = new HttpResponseMessage();
            try {
                response = await _httpClient.GetAsync(url);
            }
            catch(Exception e)
            {
                _logger.LogError($"Error Returned: {e.Message}");
            }
            
            if(!response.IsSuccessStatusCode)
                return null;
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                if(responseBody is null)
                    return null;
                else
                    return JsonSerializer.Deserialize<Breed>(responseBody);
            }



        }



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
        public async Task<List<Data>> GetHypoAllergenicBreeds()
        {
            string serviceLoc = "https://dogapi.dog/api/v2/breeds/?page[number]=";
            List<Data> hypoList = new List<Data>();
            
            for(int i = 1; i < 30; i++)
            {
                BreedPage newBreed = new BreedPage();

                string url = serviceLoc  + i.ToString();
                HttpResponseMessage response = new HttpResponseMessage();
                try {
                    response = await _httpClient.GetAsync(url);
                }
                catch(Exception e)
                {
                    _logger.LogError($"Error Returned: {e.Message}");
                }
                if(!response.IsSuccessStatusCode)
                    newBreed = null;
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if(responseBody is null)
                        newBreed = null;
                    else
                    {
                        newBreed = JsonSerializer.Deserialize<BreedPage>(responseBody);
                        var hypoBreeds = newBreed.data.Select(i => i).Where(i => i.attributes.hypoallergenic == true);
                        hypoList.AddRange(hypoBreeds);
                        
                    }


                }

            }

            return hypoList;
            
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
}
