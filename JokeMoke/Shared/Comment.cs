﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JokeMoke.Shared
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Joke Joke { get; set; }
    }
}