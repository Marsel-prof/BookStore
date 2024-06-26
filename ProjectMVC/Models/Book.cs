﻿namespace ProjectMVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishDate { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { set; get; } = DateTime.Now;
        public List<BookCategory> Categories { get; set; } = new List<BookCategory>();
    }
}
