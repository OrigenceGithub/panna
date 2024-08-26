using Microsoft.AspNetCore.Mvc;
using WebApi.DogsService;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogBreedController : ControllerBase
    {
        private readonly ILogger<DogBreedController> _logger;
        private readonly IDogBreedService _dogBreedService;

        public DogBreedController(ILogger<DogBreedController> logger, IDogBreedService dogBreedService)
        {
            _dogBreedService = dogBreedService;
            _logger = logger;
        }

        [HttpGet]
        [Route("dogbreed/{id}")]
        public async Task<Breed?> Get(string id)
        {
            return await _dogBreedService.GetBreed(id);

        }

        [HttpGet]
        [Route("dogbreed/hypoallergenic-breeds")]
        public async Task<List<Data>?> GetHypoallergenicBreeds()
        {
            return await _dogBreedService.GetHypoAllergenicBreeds();
        }
    }
}
