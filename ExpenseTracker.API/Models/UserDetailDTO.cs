using ExpenseTracker.API.Entities;

namespace ExpenseTracker.API.Models
{
    public class UserDetailDTO
    {

        public Guid Id { get; set; }
        public string Email { get; set; }

        public UserDetailDTO(User user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}
