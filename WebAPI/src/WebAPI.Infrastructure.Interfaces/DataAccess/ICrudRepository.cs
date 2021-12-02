﻿using System;
using System.Collections;

namespace WebAPI.Infrastructure.Interfaces.DataAccess
{
    public interface ICrudRepository<T>
    {
        public void Create(T model);
        public IEnumerable Read();
        public T Read(Guid id);
        public IEnumerable ReadAll();
        public string ReadPhotoName(Guid id);
        public void Update(T model);
        public void Delete(Guid id);
    }
}