using System;
using System.ComponentModel.DataAnnotations;

namespace ShortLink.Models
{
    public class Url
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Длинная ссылка")]
        [DataType(DataType.Url)]
        public string LongUrl { get; set; }
        [Display(Name = "Короткая ссылка")]
        [DataType(DataType.Url, ErrorMessage = "Введите корректный Url")]
        public string ShortUrl { get; set; }
        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime, ErrorMessage = "Введите корректную дату")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Количество переходов")]
        public int NumberFollowTheLink { get; set; }
    }
}
