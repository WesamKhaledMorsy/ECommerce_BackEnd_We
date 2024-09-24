﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
    }
}
