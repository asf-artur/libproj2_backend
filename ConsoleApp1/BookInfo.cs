using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleApp1
{
    public class BookInfoCa
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public List<string> Authors { get; set; }

        public DateTime? PublishedDate { get; set; }

        public int PageCount { get; set; }

        public List<string> Categories { get; set; }

        public string Language { get; set; }

        public string MainCategory { get; set; }

        public List<IndustryIdentifierCa> IndustryIdentifiers { get; set; }

        public string? ImagePath { get; set; }

        public string? Barcode { get; set; }

        public BookInfoCa()
        {

        }
    }

    public class IndustryIdentifierCa
    {
        public static List<IndustryIdentifierCa> DefaultIsbn(string isbn) => new List<IndustryIdentifierCa>
            {new IndustryIdentifierCa(IndustryIdentifierType.ISBN_13, isbn)};


        [JsonConverter(typeof(StringEnumConverter))]
        public IndustryIdentifierType IndustryIdentifierType { get; set; }

        public string Identifier { get; set; }

        public IndustryIdentifierCa(IndustryIdentifierType industryIdentifierType, string identifier)
        {
            IndustryIdentifierType = industryIdentifierType;
            Identifier = identifier;
         }
     }

    public enum IndustryIdentifierType //industry_identifier_type
    {
        ISBN_10,ISBN_13,ISSN ,OTHER,

        [EnumMember(Value = "Europe/London")]
        VENDOR_CODE
    }
}