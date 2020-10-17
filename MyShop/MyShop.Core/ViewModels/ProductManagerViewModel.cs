using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    // A model might not contain all the data a view needs
    // This is where ViewModel comes in hand
    // we are using this ViewModel to pass both Product and a list of ProductCategory's through
    public class ProductManagerViewModel
    {
        public Product Product { get; set; }

        // implements IEnumerable so that it can be used
        // with ForEach syntax.
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
