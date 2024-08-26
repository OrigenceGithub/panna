
### We will use this repository to help us set up a collaborative coding excersice during the interview process.

### WebApi - Project
#### We will start with this project. Please make this the 'Start Up' project.
1. DogsService/DogBreedService.cs
    * CHALLENGE #1 - Create a method that will call the following api and returns a Breed object. Url: https://dogapi.dog/api/v2/breeds/{id} . Use id '68f47c5a-5115-47cd-9849-e45d3c378f12' to get back a breed object. If 'id' does not return a Breed, then return a null object.
    * CHALLENGE #2 - Create a method that will return a list of Breed objects where the 'hypoallergenic' property set to true. You will need to call the 'breeds' api for each page number. For every Breed object returned from each page, you are  going to check if the attributes.hypoallergenic property is set to true. Example  URL for Breeds with page number: https://dogapi.dog/api/v2/breeds/?page[number]={pageNum}. Assume that the total number of pages is 29 for this test.
    * CHALLENGE #3 - Lets enhance the performance of Challenge #2 to make the api call to each of the Breeds page number in parallel. We still want to return a list of 'Data' objects where the attributes.hypoallergenic property is set to true. Assume that the total number of pages is 29 for this test.
2. Program.cs
    * Use Depenjency Ingection to add the DogBreedService to the project.
3. Controllers/DogBreedController.cs
    * Create action method to get single Breed based on 'id' parameter. Example route: dogbreed/{id}.
    * Create action method to get all Breed.Data elements where the 'hypoallergenic' property is set to true. Example route: dogbreed/hypoallergenic-breeds.

### WebApi.Test - Project
#### Use this project to create a set of unit tests for the DogBreedsService.cs

