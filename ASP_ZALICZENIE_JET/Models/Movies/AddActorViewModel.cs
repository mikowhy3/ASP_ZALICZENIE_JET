namespace ASP_ZALICZENIE_JET.Models.Movies
{
    public class AddActorViewModel
    {
        public int MovieId { get; set; }
        public string CharacterName { get; set; }
        public string PersonName { get; set; }
        public int GenderId { get; set; }
        public int CastOrder { get; set; }
    }
}