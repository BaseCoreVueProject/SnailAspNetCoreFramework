﻿//using DAL.Interface;
using DAL;
using DAL.Interface;
using System.Collections.Generic;
using DAL.DTO;

namespace DAL.Services
{
    public class CRUDService<T> where T : BaseEntity
    {
        private IRepository<T> repository;
        public List<T> Query(IQueryDto<T> query)
        {
            var predicate = ((IQueryDto<T>)query).GeneratePredicateExpression();
            var includeFunc = ((IQueryDto<T>)query).IncludeFunc();
            var order = ((IQueryDto<T>)query).OrderFunc();
            return repository.Query(predicate, includeFunc, order);
        }

        public PageResult<T> Query(IQueryDto<T> query)
        {
            var predicate = ((IQueryDto<T>)query).GeneratePredicateExpression();
            var includeFunc = ((IQueryDto<T>)query).IncludeFunc();
            var order = ((IQueryDto<T>)query).OrderFunc();
            repository.Query
        }


        public List<TResult> Query<TResult>(IQueryDto<T> query)
        {
            var predicate = ((IQueryDto<T>)query).GeneratePredicateExpression();
            var includeFunc = ((IQueryDto<T>)query).IncludeFunc();
            var order = ((IQueryDto<T>)query).OrderFunc();
            return repository.Query<TResult>(predicate, includeFunc, order, ((IQueryDto<T>)query).SelectorExpression<TResult>());
        }

    }
}
