namespace AlphaUserIdentification.Models
{
    public class Member
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}