namespace InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork
{
    public interface IUnitOfWork
    {
        IBusinessPartnerRepository BusinessPartnerRepository { get; }
        void Commit();
    }
}
