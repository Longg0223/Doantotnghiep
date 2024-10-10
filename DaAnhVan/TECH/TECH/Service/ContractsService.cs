using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IContractsService
    {
        PagedResult<ContractModelView> GetAllPaging(ContractModelViewSearch userModelViewSearch);
        void Add(ContractModelView view);
        ContractModelView GetById(int id);
        bool Deleted(int id);
        void Save();
        bool Update(int id, int status);
    }

    public class ContractsService : IContractsService
    {
        private readonly IContractsRepository _contractsRepository;
        private IUnitOfWork _unitOfWork;
        public ContractsService(IContractsRepository contractsRepository,
            IUnitOfWork unitOfWork)
        {
            _contractsRepository = contractsRepository;
            _unitOfWork = unitOfWork;
        }


        public ContractModelView GetById(int id)
        {
            var data = _contractsRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ContractModelView()
                {
                    full_name = data.full_name,
                    phone_number = data.phone_number,
                    status = 0,
                    created_at = DateTime.Now,
                    datestr = data.created_at.HasValue ? data.created_at.Value.ToString("hh:mm") + " - " + data.created_at.Value.ToString("dd/MM/yyyy") : "",
                    note = data.note
                };
                return model;
            }
            return null;
        }

        public void Add(ContractModelView view)
        {
            try
            {
                if (view != null)
                {
                    var contracts = new Contracts
                    {
                        full_name = view.full_name,
                        phone_number  = view.phone_number,
                        status = 0,
                        created_at = DateTime.Now,
                        note = view.note
                    };
                    _contractsRepository.Add(contracts);                                                          
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(int id, int status)
        {
            //try
            //{
                var dataServer = _contractsRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _contractsRepository.Update(dataServer);                                        
                    return true;
                }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            return false;
        }
    

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _contractsRepository.FindById(id);
                if (dataServer != null)
                {
                    _contractsRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<ContractModelView> GetAllPaging(ContractModelViewSearch contractModelViewSearch)
        {
            try
            {
                var query = _contractsRepository.FindAll();

                if (contractModelViewSearch.status.HasValue)
                {
                    query = query.Where(c => c.status == contractModelViewSearch.status.Value);
                }
                
                if (!string.IsNullOrEmpty(contractModelViewSearch.name))
                {
                    query = query.Where(c => c.full_name.ToLower().Contains(contractModelViewSearch.name.ToLower())  || c.phone_number.ToLower().Contains(contractModelViewSearch.name.ToLower()));
                }

                int totalRow = query.Count();
                query = query.Skip((contractModelViewSearch.PageIndex - 1) * contractModelViewSearch.PageSize).Take(contractModelViewSearch.PageSize);
                var data = query.Select(c => new ContractModelView()
                {
                    full_name = (!string.IsNullOrEmpty(c.full_name) ? c.full_name : ""),
                    id = c.id,
                    phone_number = !string.IsNullOrEmpty(c.phone_number) ? c.phone_number : "",
                    note = !string.IsNullOrEmpty(c.note) ? c.note : "",
                    status = c.status,
                    created_at = c.created_at,
                    datestr = c.created_at.HasValue ? c.created_at.Value.ToString("hh:mm") + " - "+ c.created_at.Value.ToString("dd/MM/yyyy"):""
                }).ToList();
              
                var pagingData = new PagedResult<ContractModelView>
                {
                    Results = data,
                    CurrentPage = contractModelViewSearch.PageIndex,
                    PageSize = contractModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }

        }       
    }
}
