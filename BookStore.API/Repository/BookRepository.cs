using Azure;
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using JsonPatchDocument = Microsoft.AspNetCore.JsonPatch.JsonPatchDocument;
using AutoMapper;

namespace BookStore.API.Repository
{
    public class BookRepository:IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper) 
        {
            _context=context;
            _mapper=mapper;
        } 
        public async Task<List<BookModel>> GetAllBooksAsync()
        {/*
            var records =await  _context.Books.Select(x => new BookModel()
            {
                id=x.id,
                Title= x.Title,
                Description= x.Description,

            }).ToListAsync();

            return records;*/

            var records = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(records);
        }

        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            /*var records = await _context.Books.Where(x=>x.id==bookId).Select(x => new BookModel()
            {
                id=x.id,
                Title= x.Title,
                Description= x.Description,

            }).FirstOrDefaultAsync();

            return records;*/

            var book = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book);

        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books()
            {
                Title=bookModel.Title,
                Description=bookModel.Description,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.id;
        }

        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            /* var book = await _context.Books.FindAsync(bookId);
             if(book != null)
             {
                 book.Title= bookModel.Title;
                 book.Description= bookModel.Description; /*This is hitting database twice
                 await _context.SaveChangesAsync();
             }*/
            var book = new Books()
            {
                id=bookId,
                Title=bookModel.Title,
                Description=bookModel.Description,
            };

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
           


        }

        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if(book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(int bookId)
        {
           var book=new Books() { id=bookId};/*If primary key is there*/

             _context.Books.Remove(book);

            await _context.SaveChangesAsync();
           

        }
    }
}
