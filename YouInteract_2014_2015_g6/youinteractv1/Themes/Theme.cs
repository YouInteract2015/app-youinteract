namespace YouInteractV1.Themes
{
    public class Theme
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Font { get; set; }

        public string Background { get; set; }
        //Possible to add more things in the future
        //that don't use the file system to operate

        public Theme()
        {
            Name = "Theme1";
            Id = 1;
            Font = null;
            Background = "background.jpg";
        }
    }
}
