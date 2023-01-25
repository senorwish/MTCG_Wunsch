namespace MTCGServer.Models
{
    public class UserData
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        //Erstmaliges Anlegen
        public UserData()
        {
            Name = "";
            Bio = "";
            Image = "";
        }

        //Aus DB lesen
        public UserData(string name, string bio, string image)
        {
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
