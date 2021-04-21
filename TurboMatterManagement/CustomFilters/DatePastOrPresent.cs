using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TurboMatterManagement.CustomFilters
{
    sealed public class DatePastOrPresent : ValidationAttribute
    {
        // Check if date is not a future one
        public override bool IsValid(object value)
        {
            bool result = true;
            DateTime? date = value as DateTime?;
            if (date.HasValue)
            {
                if (date.Value > DateTime.Now)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}