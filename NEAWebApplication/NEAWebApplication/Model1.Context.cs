﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NEAWebApplication
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LaunchControlSystemEntities : DbContext
    {
        public LaunchControlSystemEntities()
            : base("name=LaunchControlSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<COST> COSTS { get; set; }
        public virtual DbSet<COSTTYPE> COSTTYPEs { get; set; }
        public virtual DbSet<GLIDERTYPE> GLIDERTYPEs { get; set; }
        public virtual DbSet<LAUNCHTYPE> LAUNCHTYPEs { get; set; }
        public virtual DbSet<MEMBERCLUB> MEMBERCLUBs { get; set; }
        public virtual DbSet<MEMBERTYPE> MEMBERTYPEs { get; set; }
        public virtual DbSet<FLIGHT> FLIGHTs { get; set; }
    }
}