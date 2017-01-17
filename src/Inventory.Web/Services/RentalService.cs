using System;
using System.Collections.Generic;
using System.Linq;
using Athene.Inventory.Web.Data;
using Athene.Inventory.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Athene.Inventory.Web.Services
{
    public class RentalService : IRentalService
    {
        private readonly InventoryDbContext _db;
        public RentalService(InventoryDbContext dbContext)
        {
            _db = dbContext;
        }

        public void RentBook(int studentId, int[] bookItemIds)
        {
            var bookItems = _db.BookItems
                .Where(bi => bookItemIds.Contains(bi.Id))
                .ToArray();
            var student = _db.Students.Single(s => s.StudentId == studentId);
            foreach (var bookItem in bookItems) 
            {
                bookItem.RentedBy = student;
                bookItem.RentedAt = DateTime.Now;
            }
            _db.SaveChanges();
        }

        public IEnumerable<BookItem> FindRentedBooks(int studentId)
        {
            var bookItems = _db.BookItems
                .Include(bi => bi.StockLocation)
                .Include(bi => bi.Book)
                .Where(bi => bi.RentedBy.StudentId == studentId)
                .ToList();

            return bookItems;
        }
    }
}
