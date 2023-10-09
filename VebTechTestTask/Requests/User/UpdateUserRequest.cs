namespace VebTechTestTask.Requests.User
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    public class UpdateUserRequest : UserRequestBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }
    }
}