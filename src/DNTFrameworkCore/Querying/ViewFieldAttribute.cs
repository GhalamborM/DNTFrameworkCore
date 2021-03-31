﻿using System;

namespace DNTFrameworkCore.Querying
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ViewFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Sorting { get; set; }
        public bool Filtering { get; set; }
    }
}