using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EasyBay.ViewModels
{
    public class ChangeViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Не указан email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Не указан баланс")]
        public decimal Balance { get; set; }
    }
}
