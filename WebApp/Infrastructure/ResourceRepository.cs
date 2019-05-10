﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entity;

namespace DAL.Infrastructure
{
    public class ResourceRepository:BaseRepository<Resource>
    {

        public ResourceRepository(DatabaseContext db):base(db)
        {
        }

        
    }
}
