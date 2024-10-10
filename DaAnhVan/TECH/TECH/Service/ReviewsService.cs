
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IReviewsService
    {
        PagedResult<ReviewsModelView> GetAllPaging(ReviewsViewModelSearch ReviewsModelViewSearch);
        ReviewsModelView GetByid(int id);
        bool Add(ReviewsModelView view);
        bool Update(ReviewsModelView view);
        bool Deleted(int id);
        void Save();
        bool UpdateStatus(int id, int status);
        ProductViews GetReviewForProduct(int productId);
    }

    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;
        private IUnitOfWork _unitOfWork;
        public ReviewsService(IReviewsRepository postsRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewsRepository = postsRepository;
            _unitOfWork = unitOfWork;
        }
        public ReviewsModelView GetByid(int id)
        {
            var data = _reviewsRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new ReviewsModelView()
                {
                    id = data.id,
                    product_id = data.product_id,
                    order_id = data.order_id,
                    comment = data.comment,
                    star = data.star,
                    status = data.star,
                    created_at = data.created_at

                };
                return model;
            }
            return null;
        }
        public bool Add(ReviewsModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Reviews
                    {
                        product_id = view.product_id,
                        order_id = view.order_id,
                        comment = view.comment,
                        star = view.star,
                        status = view.status,
                        created_at = DateTime.Now,                        
                    };
                    _reviewsRepository.Add(products);

                    return true;                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(ReviewsModelView view)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.id = view.id;
                    dataServer.comment = view.comment;  
                    _reviewsRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

      public  bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _reviewsRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(id);
                if (dataServer != null)
                {
                    _reviewsRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public ProductViews GetReviewForProduct(int productId)
        {
            var productViews = new ProductViews();
            var data = _reviewsRepository.FindAll(p => p.product_id == productId).Select(p => new ReviewsModelView()
            {
                id = p.id,
                star = p.star
            }).ToList();
            if (data != null && data.Count > 0)
            {
                var _data = data.GroupBy(p => p.star).ToList();
                var _dataMax = _data.MaxBy(p => p.Key);
                var _dataCount = _dataMax.Count();
                productViews.star = _dataMax.Key.Value;
                productViews.review_count = _dataCount;
            }
            return productViews;
        }
        public PagedResult<ReviewsModelView> GetAllPaging(ReviewsViewModelSearch ReviewsModelViewSearch)
        {
            try
            {
                var query = _reviewsRepository.FindAll();
                int totalRow = 0;
                var data = new List<ReviewsModelView>();
                if (ReviewsModelViewSearch.star.HasValue)
                {
                    query = query.Where(c => c.star == ReviewsModelViewSearch.star.Value);
                }
                if (!string.IsNullOrEmpty(ReviewsModelViewSearch.name))
                {
                    query = query.Where(c => ReviewsModelViewSearch.order_ids.Contains(c.order_id.Value));
                }

                if ((ReviewsModelViewSearch.order_ids == null || ReviewsModelViewSearch.order_ids.Count == 0)
                    && !ReviewsModelViewSearch.star.HasValue 
                    && !string.IsNullOrEmpty(ReviewsModelViewSearch.name))
                {
                    data = new List<ReviewsModelView>();
                }
                else
                {
                    totalRow = query.Count();
                    query = query.Skip((ReviewsModelViewSearch.PageIndex - 1) * ReviewsModelViewSearch.PageSize).Take(ReviewsModelViewSearch.PageSize);
                    data = query.Select(p => new ReviewsModelView()
                    {
                        id = p.id,
                        order_id = p.order_id,
                        product_id = p.product_id,
                        comment = p.comment,
                        star = p.star,
                        status = p.status,
                        created_at = p.created_at,
                        create_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                    }).ToList();
                }

                //int totalRow = query.Count();
                //query = query.Skip((ReviewsModelViewSearch.PageIndex - 1) * ReviewsModelViewSearch.PageSize).Take(ReviewsModelViewSearch.PageSize);
                //var data = query.Select(p => new ReviewsModelView()
                //{
                //    id = p.id,
                //    order_id = p.order_id,
                //    product_id = p.product_id,
                //    comment = p.comment,
                //    star = p.star,
                //    status = p.status,
                //    created_at = p.created_at,
                //    create_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                //}).ToList();
              
                var pagingData = new PagedResult<ReviewsModelView>
                {
                    Results = data,
                    CurrentPage = ReviewsModelViewSearch.PageIndex,
                    PageSize = ReviewsModelViewSearch.PageSize,
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
