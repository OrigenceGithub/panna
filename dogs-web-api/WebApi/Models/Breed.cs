namespace WebApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Breed
    {
        public Data data { get; set; }
        public Links links { get; set; }
    }


}
