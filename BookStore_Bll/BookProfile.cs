using AutoMapper;
using BookStore_Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Bll
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookDTO, Book>();
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
