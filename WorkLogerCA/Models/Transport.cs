using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkLogerCA.Models;

namespace WorkLogerCA.Models
{
    public class Transport
    {
        public Transport()
        {
           
            CreationDateTime = DateTime.UtcNow;

        }

        public int Id { get; set; }

        [Display(Name = "Дата и время создания")]
        public DateTime CreationDateTime { get; set; }

        [Display(Name = "ФИО водителя")]
        [Required]
        public string DriverFullName { get; set; } = null!;

        [Display(Name = "Номер путевого листа")]
        public int? WaybillNumber { get; set; }

        [Display(Name = "Прямой рейс")]
        public string? DirectRide { get; set; }

        [Display(Name = "Обратный рейс")]
        public string? ReturnRide { get; set; }

        [Display(Name = "Место назначения")]
        [Required]
        public string Destination { get; set; } = null!;

        [Display(Name = "Обратное место назначения")]
        [Required]
        public string ReturnDestination { get; set; } = null!;

        [Display(Name = "Дата и время отправления")]
        [Required]
        public DateTime DetpartureDateTime { get; set; }

        [Display(Name = "Дата и время прибытия")]
        public DateTime ArrivalDateTime { get; set; }

        [Display(Name = "Пассажиры")]
        public string? Passengers { get; set; }

        [Display(Name = "Примечение")]
        public string? Note { get; set; }

        public List<Equipment> Equipments { get; set; } = new();

        public List<Work> Works { get; set; } = new();
    }
}
