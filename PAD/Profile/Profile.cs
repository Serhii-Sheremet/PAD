using System;

namespace PAD
{
    public class Profile
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PlaceOfBirthId { get; set; }
        public int PlaceOfLivingId { get; set; }
        public string Message { get; set; }
        public int Checked {  get; set; }

    }
}
