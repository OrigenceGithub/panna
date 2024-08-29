using System.Text.Json;
using WebApi.Models;

namespace WebApi.DogsService
{
    public interface IDogBreedService
    {
        Task<IEnumerable<Breed>> GetBreedsAsync(bool isHypoallergenic = true);
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
        //public async Task<IEnumerable<Breed>> GetBreedsAsync(bool isHypoallergenic = true)
        //{
        //    var pageCount = 29;
        //    var allFilteredBreeds = new List<Breed>();

        //    for (var pageNum = 1; pageNum <= pageCount; pageNum++)
        //    {
        //        var url = $"https://dogapi.dog/api/v2/breeds/?page[number]={pageNum}";
        //        var response = await _httpClient.GetStringAsync(url, CancellationToken.None);
        //        var breedsPage = JsonSerializer.Deserialize<BreedPage>(response, JsonSerializerOptions.Default)!;

        //        if (breedsPage is null)
        //        {
        //            throw new Exception("Breeds page is null.");
        //        }

        //        var filteredBreeds = breedsPage.data.Where(d => d.attributes.hypoallergenic == isHypoallergenic);

        //        allFilteredBreeds.AddRange(filteredBreeds.Select(d => new Breed { data = d }));
        //    }

        //    return allFilteredBreeds;
        //}

        /// <summary>
        /// *** CHALLENGE #3 *************************************************************
        /// Lets enhance the performance of Challenge #2 to make the api call to each of the 
        /// Breeds page number in parallel. We still want to return a list of 'Data' objects
        /// where the attributes.hypoallergenic property is set to true. Assume that the total 
        /// number of pages is 29 for this test.
        /// <param name="hypoallergenic">true or false</param>
        /// </summary>
        public async Task<IEnumerable<Breed>> GetBreedsAsync(bool isHypoallergenic = true)
        {
            var pageCount = 29;
            var allFilteredBreeds = new List<Breed>();
            var getTasks = new List<Task<string>>();

            for (var pageNum = 1; pageNum <= pageCount; pageNum++)
            {
                var url = $"https://dogapi.dog/api/v2/breeds/?page[number]={pageNum}";
                var getTask = _httpClient.GetStringAsync(url, CancellationToken.None);
                getTasks.Add(getTask);

            }

            var result = await Task.WhenAll(getTasks);

            foreach (var response in result)
            {
                var breedsPage = JsonSerializer.Deserialize<BreedPage>(response, JsonSerializerOptions.Default)!;

                if (breedsPage is null)
                {
                    throw new Exception("Breeds page is null.");
                }

                var filteredBreeds = breedsPage.data.Where(d => d.attributes.hypoallergenic == isHypoallergenic);

                allFilteredBreeds.AddRange(filteredBreeds.Select(d => new Breed { data = d }));
            }

            return allFilteredBreeds;
        }
    }
}
