using System.ComponentModel.DataAnnotations;

namespace TOT.ToolDownloadImage.Models
{
    public class ImageViewModel
    {
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string URL { get; set; }
    }
}
