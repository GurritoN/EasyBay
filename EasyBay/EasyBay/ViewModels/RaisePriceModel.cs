using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EasyBay.ViewModels
{
    public class RaisePriceModel
    {
        [Required(ErrorMessage = "Не указана цена")]
        public string Price { get; set; }
    }
}
