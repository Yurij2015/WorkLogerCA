using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLogerCA.Models
{
    public class Equipment
    {
        public Equipment()
        {
            CreationDateTime = DateTime.UtcNow;
            Count = 1;
        }


        public int Id { get; set; }

        [Display(Name = "Дата и время создания")]
        public DateTime CreationDateTime { get; set; }

        [Display(Name = "Наименование оборудования")]
        [Required]
        public string? EquipmentIdentification { get; set; }

        [Display(Name = "Заводской номер")]
        public string? FactoryNumber { get; set; }

        [Display(Name = "Количество")]
        [Required]
        public int Count { get; set; }

        [Display(Name = "Состояние")]
        public string? State { get; set; }

        [Display(Name = "Откуда отправлено")]
        public string? SentFrom { get; set; }

        [Display(Name = "Место расположения")]
        [Required]
        public string Location { get; set; } = null!;

        [Display(Name = "Дата и время прибытия")]
        public DateTime? ArrivalDateTime { get; set; }

        [Display(Name = "Дата и время отрправления")]
        public DateTime? DepartureDateTime { get; set; }

        [Display(Name = "Документы")]
        public string? Document { get; set; }

        [Display(Name = "Действие свидетельства о поверке")]
        public DateTime CertificateExpiryDate { get; set; }

        [Display(Name = "Транспортное средство")]
        public int TransportId { get; set; }
        [Display(Name = "Транспортное средство")]
        public Transport? Transport { get; set; }

        [Display(Name = "Заявка")]
        public int RequestId { get; set; }
        [Display(Name = "Заявка")]
        public Request? Request { get; set; }
    }
}
