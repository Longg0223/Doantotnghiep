
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IProductQuantityService
    {           
        void Add(List<QuantityProductModelView> view);
        bool Update(List<QuantityProductModelView> view);
        bool Deleted(List<int> ids);
        void UpdateTotal(QuantityProductModelView view);
        List<QuantityProductModelView> GetProductQuantity(int productId);
        QuantityProductModelView GetQuantityId(int id);
        QuantityProductModelView GetQuantityByProductIdSizeId(int productId,int sizeId);
        void Save();
    }

    public class ProductQuantityService : IProductQuantityService
    {
        private readonly IProductQuantityRepository _productQuantityRepository;
        private IUnitOfWork _unitOfWork;
        public ProductQuantityService(IProductQuantityRepository productQuantityRepository,
            IUnitOfWork unitOfWork)
        {
            _productQuantityRepository = productQuantityRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(List<QuantityProductModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var image = new ProductQuantity
                        {
                            product_id = item.ProductId,
                            size_id = item.AppSizeId,
                            color_id = item.ColorId,
                            totalimport = item.TotalImported
                        };
                        _productQuantityRepository.Add(image);
                    }
                                
                }
            }
            catch (Exception ex)
            {
            }

        }
        public List<QuantityProductModelView> GetProductQuantity(int productId)
        {
            if (productId > 0)
            {
                var productQuantity = _productQuantityRepository.FindAll(p => p.product_id == productId).Select(p => new QuantityProductModelView
                {
                    Id = p.id,
                    ProductId = p.product_id,
                    ColorId = p.color_id,
                    AppSizeId = p.size_id,
                    TotalImported = p.totalimport,
                    TotalSell = p.totalsell
                }).ToList();
                return productQuantity;
            }
            return null;
        }
        public QuantityProductModelView GetQuantityByProductIdSizeId(int productId, int sizeId)
        {
            if (productId > 0)
            {
                var productQuantity = _productQuantityRepository.FindAll(p => p.product_id == productId && p.size_id == sizeId).Select(p => new QuantityProductModelView
                {
                    Id = p.id,
                    ProductId = p.product_id,
                    ColorId = p.color_id,
                    AppSizeId = p.size_id,
                    TotalSell = p.totalsell,
                    TotalImported = p.totalimport,
                }).FirstOrDefault();
                return productQuantity;
            }
            return null;

        }
        public QuantityProductModelView GetQuantityId(int id)
        {
            if (id > 0)
            {
                var productQuantity = _productQuantityRepository.FindAll(p => p.id == id).Select(p => new QuantityProductModelView
                {
                    Id = p.id,
                    ProductId = p.product_id,
                    AppSizeId = p.size_id,
                    TotalSell = p.totalsell,
                    TotalImported = p.totalimport,
                }).FirstOrDefault();
                return productQuantity;
            }
            return null;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(List<QuantityProductModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var dataServer = _productQuantityRepository.FindById(item.Id);
                        if (dataServer != null)
                        {
                            dataServer.product_id = item.ProductId;
                            dataServer.size_id = item.AppSizeId;
                            dataServer.color_id = item.ColorId;
                            dataServer.totalimport = item.TotalImported;
                            _productQuantityRepository.Update(dataServer);
                           
                        }
                    }
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public void UpdateTotal(QuantityProductModelView view)
        {
            try
            {
                if (view != null)
                {
                    var dataServer = _productQuantityRepository.FindById(view.Id);
                    if (dataServer != null)
                    {
                        if (view.TotalSell.HasValue)
                        {
                            dataServer.totalsell = view.TotalSell.Value;
                        }
                        _productQuantityRepository.Update(dataServer);

                    }
                  
                }

            }
            catch (Exception ex)
            {
               
            }

            
        }

        public bool Deleted(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    foreach (var item in ids)
                    {
                        var dataServer = _productQuantityRepository.FindById(item);
                        if (dataServer != null)
                        {
                            _productQuantityRepository.Remove(dataServer);
                        }                        
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
       
    }
}
