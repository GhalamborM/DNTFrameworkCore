﻿using System;
using System.Collections.Generic;
using DNTFrameworkCore.Domain;

namespace DNTFrameworkCore.Application
{
    public abstract class MasterModel : MasterModel<int>
    {
    }

    public abstract class MasterModel<TKey> : ReadModel<TKey>, IHasRowVersion where TKey : IEquatable<TKey>
    {
        public byte[] Version { get; set; }
        public virtual bool IsNew() => EqualityComparer<TKey>.Default.Equals(Id, default);
    }
}