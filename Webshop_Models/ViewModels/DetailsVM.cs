﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop_Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCard { get; set; }
    }
}
