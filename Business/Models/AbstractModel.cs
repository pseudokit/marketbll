﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public abstract class AbstractModel
    {
        public int Id { get; set; }

        public AbstractModel()
        {
            
        }
        protected AbstractModel(int id)
        {
            this.Id = id;
        }
    }
}
