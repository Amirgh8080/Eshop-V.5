﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.ViewModels.Shared
{
    public class BasePaging<T>
    {
        public BasePaging()
        {
            Page = 1;
            TakeEntity = 9;
            HowManyShowPageAfterAndBefore = 5;
            Entities = new List<T>();
        }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public int AllEntitiesCount { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int HowManyShowPageAfterAndBefore { get; set; }

        public List<T> Entities { get; set; }

        public PagingViewModel GetCurrentPaging()
        {
            return new PagingViewModel
            {
                EndPage = EndPage,
                Page = Page,
                StartPage = StartPage
            };
        }


        public async Task<BasePaging<T>> Paging(IQueryable<T> queryable)
        {
            TakeEntity = TakeEntity;

            var allEntitiesCount = await queryable.CountAsync();

            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)TakeEntity));

            Page = Page > pageCount ? pageCount : Page;
            if (Page <= 0) Page = 1;
            AllEntitiesCount = allEntitiesCount;
            HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore;
            SkipEntity = (Page - 1) * TakeEntity;
            StartPage = Page - HowManyShowPageAfterAndBefore <= 0 ? 1 : Page - HowManyShowPageAfterAndBefore;
            EndPage = Page + HowManyShowPageAfterAndBefore > pageCount ? pageCount : Page + HowManyShowPageAfterAndBefore;
            PageCount = pageCount;
            Entities = await queryable.Skip(SkipEntity).Take(TakeEntity).ToListAsync();

            return this;
        }
    }

    public class PagingViewModel
    {
        public int Page { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }
    }
}
