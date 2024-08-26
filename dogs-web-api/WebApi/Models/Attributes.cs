namespace WebApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Attributes
    {
        public string name { get; set; }
        public int min_life { get; set; }
        public int max_life { get; set; }
        public string description { get; set; }
        public bool hypoallergenic { get; set; }
    }


}
