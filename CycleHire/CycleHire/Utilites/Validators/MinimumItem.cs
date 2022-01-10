using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Utilites.Validators
{
    public class MinimumItem : ValidationAttribute
    {
        private int _minimumItems;
        public MinimumItem(int minElements)
        {
            _minimumItems = minElements;
        }

        public override bool IsValid(object value)
        {
            IList x = value as IList;

            if (x != null)
            {
                return x.Count >= _minimumItems;
            }
            return false;
        }
    }
}
