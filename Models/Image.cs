using System.ComponentModel.DataAnnotations;

namespace TOT.ToolDownloadImage.Models
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        public string URL { get; set; }
    }
}
