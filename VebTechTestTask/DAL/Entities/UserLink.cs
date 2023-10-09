namespace VebTechTestTask.DAL.Entities
{
    using Interfaces;

    public class UserLink :IEntity<int>
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int UserId { get; set; }

        #region Navigation Properties

        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
        
        #endregion  
    }
}