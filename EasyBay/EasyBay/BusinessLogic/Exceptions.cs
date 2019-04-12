using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBay.BusinessLogic
{
    public class EasyBayException : Exception
    {

    }

    public class FinancialException : EasyBayException
    {

    }

    public class NotEnoughMoneyException : FinancialException
    {

    }
}
