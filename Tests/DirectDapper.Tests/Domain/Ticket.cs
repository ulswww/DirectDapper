using Abp.Domain.Entities;

namespace DirectDapper.Tests.Domain
{
    public class Ticket : Entity
    {
        public virtual string EmailAddress { get; set; }

        public virtual string Message { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int TenantId { get; set; }

        public Ticket()
        {
            IsActive = true;
        }
    }
}