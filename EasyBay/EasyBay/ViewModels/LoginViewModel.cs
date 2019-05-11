using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
