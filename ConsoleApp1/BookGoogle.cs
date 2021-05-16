using System.Collections.Generic;

namespace ConsoleApp1
{
    public class BookGoogle
    {
        public string Id { get; set; }

        public VolumeInfo VolumeInfo { get; set; }

        public BookGoogle(string id, VolumeInfo volumeInfo)
        {
            Id = id;
            VolumeInfo = volumeInfo;
        }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }

        public string Publisher { get; set; }

        public string PublishedDate { get; set; }

        public string PageCount { get; set; }

        public string PrintType { get; set; }

        public string MaturityRating { get; set; }

        public string Large { get; set; }

        public string ExtraLarge { get; set; }

        public string Language { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public List<string> Categories { get; set; }

        public string MainCategory { get; set; }

        public ImageLinks ImageLinks { get; set; }
    }

    public class ImageLinks
    {
        public string Thumbnail { get; set; }

        public string Large { get; set; }

        public string ExtraLarge { get; set; }
    }

    public class ReadingModes
    {
        public string Text { get; set; }

        public string Image { get; set; }
    }
}