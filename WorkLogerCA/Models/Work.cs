using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkLogerCA.Models
{
    public class Work
    {
        public Work()
        {
            CreationDateTime = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Display(Name = "Дата и время создания")]
        public DateTime CreationDateTime { get; set; }

        [Display(Name = "Исполнители работ")]
        [Required]
        public string PerformersOfWork { get; set; }

        [Display(Name = "Дата завершения работ")]
        [Required]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Описание выполненных работ")]
        [Required]
        public string DescriptionOfPerformedWork { get; set; } = null!;

        [Display(Name = "Примечание")]
        public string? Note { get; set; }

        [Display(Name = "Место выполнения работ")]
        [Required]
        public string PlaceOfWork { get; set; } = null!;

        [Display(Name = "Выполнение работы")]
        public bool WorkCompletiting { get; set; }

        public int TransportId { get; set; }

        [Display(Name = "Транспортрное средство")]
        public virtual Transport Transport { get; set; } = null!;
    }
}
