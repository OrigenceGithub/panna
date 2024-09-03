﻿using Microsoft.AspNetCore.Mvc;
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
        public Task<Breed?> Get(string id)
        {
            // Call the DogBreedService to get the breed object.

            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("dogbreed/hypoallergenic-breeds")]
        public async Task<List<Breed>?> GetHypoallergenicBreeds()
        {
            // Call the DogBreedService to get the list of breeds that have the 'hypoallergenic' property set to true.
            return await _dogBreedService.GetHypoAllergenics();
        }
    }
}
