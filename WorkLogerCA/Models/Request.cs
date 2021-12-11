using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkLogerCA.Models;


namespace WorkLogerCA.Models
{
    public class Request
    {
        public Request()
        {
            CreationDateTime = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Display(Name = "Дата и время создания")]
        public DateTime CreationDateTime { get; set; }

        [Display(Name = "Номер заявки")]
        [Required]
        public string? RequestNumber { get; set; }

        [Display(Name = "Дата и время поступления заявки")]
        [Required]
        public DateTime RequestDateTime { get; set; }

        [Display(Name = "Номер буровой бригады")]
        public int? NumberDrillingCrew { get; set; }

        [Display(Name = "Описание заявки")]
        [Required]
        public string? RequestDescription { get; set; }
       
        [Display(Name = "Место выполнения работ")]
        [Required]
        public string? PlaceOfWork { get; set; }

        [Display(Name = "Примечание")]
        public string? Note { get; set; }

        [Display(Name = "Отправка результата испытаний")]
        public bool SendResult { get; set; }

        [Display(Name = "Повторная заявка")]
        public string? RepeatedRequest { get; set; }

        [Display(Name = "Дата и время подачи заявки")]
        public DateTime DateTimeSendRequest { get; set; }

        [Display(Name = "Выполнение заявки подрядной организацией")]
        public bool CompletedRequest { get; set; }

        [Display(Name = "Примечание для подрядчика")]
        public string? ContractorNote { get; set; }

        [Display(Name = "Выполнение заявки")]
        public bool RequestState { get; set; }

        public List<Equipment> Equipments { get; set; } = new();


    }
}
