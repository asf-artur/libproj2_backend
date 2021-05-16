using System.Collections.Generic;

namespace WebApplication2.Contracts
{
    public class Book
    {
        public string? Author { get; }

        public string? Title { get; }

        public string? PublishersImprint { get; }

        public string? PhysicalDescription { get; }

        public string? Volume { get; }

        public string? Isbn { get; }

        public List<string>? Tags { get; }

        public string S;

        public Book(string? author, string? title, string? publishersImprint, string? physicalDescription, string? volume, string? isbn, List<string>? tags)
        {
            Author = author;
            Title = title;
            PublishersImprint = publishersImprint;
            PhysicalDescription = physicalDescription;
            Volume = volume;
            Isbn = isbn;
            Tags = tags;
        }
    }
}