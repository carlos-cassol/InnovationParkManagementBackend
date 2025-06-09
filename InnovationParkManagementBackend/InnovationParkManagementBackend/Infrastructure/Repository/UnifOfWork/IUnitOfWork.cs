namespace InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork
{
    public interface IUnitOfWork
    {
        IBusinessPartnerRepository BusinessPartnerRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        void Commit();
    }
}
