﻿using InnovationParkManagementBackend.Infrastructure.Context;

namespace InnovationParkManagementBackend.Infrastructure.Repository.UnifOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IBusinessPartnerRepository _repository;
        private ICompanyRepository _companyRepository;

        public AppDbContext? _context;

        public UnitOfWork(AppDbContext? context)
        {
            _context = context;
        }

        public IBusinessPartnerRepository BusinessPartnerRepository
        {
            get { return _repository = _repository ?? new BusinessPartnerRepository(_context);}
        }

        public ICompanyRepository CompanyRepository
        {
            get { return _companyRepository = _companyRepository ?? new CompanyRepository(_context);}
        }

        public void Commit()
        {
            _context?.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
