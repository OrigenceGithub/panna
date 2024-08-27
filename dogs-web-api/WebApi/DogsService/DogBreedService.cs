using WebApi.Models;
using System.Text.Json;
using WebApi.Controllers;
using System.Diagnostics;

namespace WebApi.DogsService
{
    public interface IDogBreedService
    {
        public Task<IEnumerable<Breed>> GetBreeds();
        public Task<IEnumerable<Breed>> GetBreedsParallel();
    }


    public class DogBreedService : IDogBreedService
    {
        private readonly HttpClient _httpClient;

        public DogBreedService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
        /// 
        public async Task<IEnumerable<Breed>> GetBreeds()
        {
            var sw = Stopwatch.StartNew();

            var ret = new List<Breed>();

            for (int i = 1; i <= 29; i++)
            {
                var url = $"https://dogapi.dog/api/v2/breeds/?page[number]={i}";
               
                var breedPage = await _httpClient.GetFromJsonAsync<BreedPage>(url);

                var hypoallergenicBreeds = breedPage.data.Where(x => x.attributes.hypoallergenic)
                    .Select(x => new Breed
                    {
                        data = x
                    });

                ret.AddRange(hypoallergenicBreeds);
            }

            sw.Stop();

            return ret;
        }


        /// <summary>
        /// *** CHALLENGE #3 *************************************************************
        /// Lets enhance the performance of Challenge #2 to make the api call to each of the 
        /// Breeds page number in parallel. We still want to return a list of 'Data' objects
        /// where the attributes.hypoallergenic property is set to true. Assume that the total 
        /// number of pages is 29 for this test.
        /// <param name="hypoallergenic">true or false</param>
        /// </summary>
        /// 
        public async Task<IEnumerable<Breed>> GetBreedsParallel()
        {
            var sw = Stopwatch.StartNew();

            var ret = new List<Breed>();

            var tasks = new List<Task<BreedPage>>();

            for (int i = 1; i <= 29; i++)
            {
                var url = $"https://dogapi.dog/api/v2/breeds/?page[number]={i}";

                tasks.Add(_httpClient.GetFromJsonAsync<BreedPage>(url));
            }

            var responses = await Task.WhenAll(tasks);

            foreach (var res in responses)
            {
                var hypoallergenicBreeds = res.data.Where(x => x.attributes.hypoallergenic)
                    .Select(x => new Breed
                    {
                        data = x
                    });

                ret.AddRange(hypoallergenicBreeds);
            }

            sw.Stop();

            return ret;
        }
    }
}
