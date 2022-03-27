using Reddington.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reddington.Core.Extenstions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reddington.Data
{
    public static class Extension
    {

        public static void SetCreateOn(this ModelBuilder modelBuilder)
        {
            var ListIDateEntityClasses = typeof(IDateEntity).GetAllClassNames();
            var ListEntityMaps = modelBuilder.Model.GetEntityTypes().Where(p=>ListIDateEntityClasses.Contains(p.ClrType.FullName));
            foreach (var EntityMap in ListEntityMaps)
            {
                //var props = EntityMap.GetProperties().Where(p=>p.ClrType==typeof(string));
                //foreach (var item in props)
                //{
                //    item.SetColumnType("nvarchar50");
                //}
                var property = EntityMap.FindProperty("CreateOn");
                if(property!=null)
                {
                    property.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    property.SetDefaultValueSql("GetDate()");
                }
            }
         
        }
        public static void GenrateSP(this MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetCustomers]
                   
                AS
                BEGIN
                 Select * From Customers
                END";
            migrationBuilder.Sql(sp);
        }

    }
}
