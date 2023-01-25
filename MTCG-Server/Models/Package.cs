namespace MTCGServer.Models
{
    public class Package
    {
        public List<Card> PackageOfCards { get; set; }
        public int Price { get; }

        public Package(List<Card> package)
        {
            PackageOfCards = package;
            Price = 5;
        }
    }
}
