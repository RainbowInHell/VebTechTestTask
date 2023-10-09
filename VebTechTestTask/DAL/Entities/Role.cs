namespace VebTechTestTask.DAL.Entities
{
    using Interfaces;

    public class Role : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        #region Navigation Properties

        public virtual List<UserLink> UserLinks { get; set; }

        #endregion
    }
}