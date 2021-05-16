using System;
using System.Collections.Generic;

namespace WebApplication2.Contracts.Books
{
    // public class BookInfo
    // {
    //     public int Id { get; }
    //
    //     public List<string> Authors { get; }
    //
    //     public string Title { get; }
    //
    //     public string PublishersImprint { get; }
    //
    //     public string PhysicalDescription { get; }
    //
    //     public string Volume { get; }
    //
    //     public string Isbn { get; }
    //
    //     public List<string> Tags { get; }
    //
    //     public BookInfo(int id, List<string> authors, string title, string publishersImprint, string physicalDescription, string volume, string isbn, List<string> tags)
    //     {
    //         Id = id;
    //         Authors = authors;
    //         Title = title;
    //         PublishersImprint = publishersImprint;
    //         PhysicalDescription = physicalDescription;
    //         Volume = volume;
    //         Isbn = isbn;
    //         Tags = tags;
    //     }
    // }

    public class BookInfo
    {
        public int Id { get; }

        public string Title { get; }

        public string SubTitle { get; }

        public List<string> Authors { get; }

        public string Publisher { get; }

        public DateTimeOffset? PublishedDate { get; }

        public int PageCount { get; }

        public List<string> Categories { get; }

        public string Language { get; }

        public string MainCategory { get; }

        public List<IndustryIdentifier> IndustryIdentifiers { get; }

        public string? ImagePath { get; }

        public string? Barcode { get; }

        public BookInfo()
        {
        }

        public BookInfo(int id, string title, string subTitle, List<string> authors, string publisher, DateTime? publishedDate, int pageCount, List<string> categories, string language, string mainCategory, List<IndustryIdentifier> industryIdentifiers, string? imagePath, string? barcode)
        {
            Id = id;
            Title = title;
            SubTitle = subTitle;
            Authors = authors;
            Publisher = publisher;
            PublishedDate = publishedDate;
            PageCount = pageCount;
            Categories = categories;
            Language = language;
            MainCategory = mainCategory;
            IndustryIdentifiers = industryIdentifiers;
            ImagePath = imagePath;
            Barcode = barcode;
        }
    }

    public class IndustryIdentifier
    {
        public static List<IndustryIdentifier> DefaultIsbn(string isbn) => new List<IndustryIdentifier>
            {new IndustryIdentifier(IndustryIdentifierType.ISBN_13, isbn)};

        public IndustryIdentifierType IndustryIdentifierType { get; }

        public string Identifier { get; }

        public IndustryIdentifier(IndustryIdentifierType industryIdentifierType, string identifier)
        {
            IndustryIdentifierType = industryIdentifierType;
            Identifier = identifier;
        }
    }

    public enum IndustryIdentifierType
    {
        ISBN_10,

        ISBN_13,

        ISSN,

        OTHER,

        VENDOR_CODE
    }
}