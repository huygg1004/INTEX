﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class PhotoContext: DbContext
    {
        public PhotoContext (DbContextOptions<PhotoContext> options) : base(options)
        {

        }

        public DbSet<Photo> Photos { get; set; }
    }
}
