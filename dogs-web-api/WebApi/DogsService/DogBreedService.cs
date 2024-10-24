using WebApi.Models;
using System.Text.Json;
using WebApi.Controllers;
using System;

namespace WebApi.DogsService
{
    public interface IDogBreedService
    {
        public Task<List<Breed>> GetBreedsByHypoallergenic(bool hypoallergenic);
        public Task<List<Breed>> GetBreedsByHypoallergenicParallel(bool hypoallergenic);

        public Task<List<Breed>> GetBreedsByHypoallergenicParallelForEach(bool hypoallergenic);
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
        public async Task<List<Breed>> GetBreedsByHypoallergenic(bool hypoallergenic)
        {
            var result = new List<Breed>();

            var pageUri = "https://dogapi.dog/api/v2/breeds/?page[number]=1";
            do
            {
                var page = await _httpClient.GetFromJsonAsync<BreedPage>(pageUri)
                    ?? throw new Exception("Error getting breed page");

                result.AddRange(page.data.Where(d => d.attributes.hypoallergenic == hypoallergenic).Select(d => new Breed { data = d }));

                pageUri = page.links.next;
            } while (pageUri is not null);

            return result;
        }


        /// <summary>
        /// *** CHALLENGE #3 *************************************************************
        /// Lets enhance the performance of Challenge #2 to make the api call to each of the
        /// Breeds page number in parallel. We still want to return a list of 'Data' objects
        /// where the attributes.hypoallergenic property is set to true. Assume that the total
        /// number of pages is 29 for this test.
        /// <param name="hypoallergenic">true or false</param>
        /// </summary>
        public async Task<List<Breed>> GetBreedsByHypoallergenicParallel(bool hypoallergenic)
        {
            var pageTasks = Enumerable
                .Range(1, 29)
                .Select(e => $"https://dogapi.dog/api/v2/breeds/?page[number]={e}")
                .Select(pageUri => _httpClient.GetFromJsonAsync<BreedPage>(pageUri));

            await Task.WhenAll(pageTasks);

            var result = pageTasks
                .SelectMany(t => t.Result!.data)
                .Where(d => d.attributes.hypoallergenic == hypoallergenic)
                .Select(d => new Breed { data = d })
                .ToList();

            return result;
        }

        public async Task<List<Breed>> GetBreedsByHypoallergenicParallelForEach(bool hypoallergenic)
        {
            var pageTasks = Enumerable.Range(1, 29);
            var pages = new BreedPage[29];

            Parallel.ForEach(pageTasks, (e) =>
            {
                var pageUri = $"https://dogapi.dog/api/v2/breeds/?page[number]={e}";
                var x = _httpClient.GetFromJsonAsync<BreedPage>(pageUri).Result;
                var result = x!.data.Where(d => d.attributes.hypoallergenic == hypoallergenic).ToList();
                pages[e - 1] = new BreedPage { data = result };
            });

            var result = pages.SelectMany(p => p.data).Select(e => new Breed { data = e }).ToList();
            return result;
        }
    }
}
