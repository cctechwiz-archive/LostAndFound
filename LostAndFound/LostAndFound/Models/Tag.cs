namespace LostAndFound.Models
{
    public abstract class Tag
    {
        // TODO: Look into this ID thing... we need to auto-increment this ourselves - function in Provider, possibly
        //private int? Id { get; set; }

        public string Name { get; set; }
    }
}
