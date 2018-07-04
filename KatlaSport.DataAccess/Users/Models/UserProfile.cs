namespace KatlaSport.DataAccess.Users.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsBanned { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
    }
}