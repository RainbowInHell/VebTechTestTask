namespace VebTechTestTask.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        #region Navigation Properties

        public List<string>? Roles { get; set; }

        #endregion    
    }
}