using Reddington.Data;
using Reddington.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Reddington.Data
{
    public class SqlServerApplicationContext:DbContext, IApplcationDbContext
    {
        public SqlServerApplicationContext(DbContextOptions option)
            :base(option)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Shop;Integrated Security=true;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new ProductCategoryMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new ProductPictureMap());
            modelBuilder.ApplyConfiguration(new ShoppingCartItemMap());
            modelBuilder.SetCreateOn();
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public List<T> RunSp<T>(string StoreName, List<DbParamter> ListParamert) where T : new()
        {
            this.Database.OpenConnection();
            DbCommand cmd = this.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = StoreName;
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (var item in ListParamert)
            {
                cmd.Parameters.Add(new SqlParameter { ParameterName = item.ParametrName, Value = item.Value });
            }


            List<T> list = new List<T>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader != null && reader.HasRows)
                {
                    var entity = typeof(T);
                    var propDict = new Dictionary<string, PropertyInfo>();
                    var props = entity.GetProperties
           (BindingFlags.Instance | BindingFlags.Public);
                    propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (reader.Read())
                    {
                        T newobject = new T();

                        for (int index = 0; index < reader.FieldCount; index++)
                        {
                            if (propDict.ContainsKey(reader.GetName(index).ToUpper()))
                            {
                                var info = propDict[reader.GetName(index).ToUpper()];
                                if ((info != null) && info.CanWrite)
                                {
                                    var val = reader.GetValue(index);
                                    info.SetValue(newobject, (val == DBNull.Value) ? null : val, null);
                                }
                            }
                        }
                        list.Add(newobject);
                    }

                }
                this.Database.CloseConnection();
                return list;
            }
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                CleanContext();
                throw ex;
            }
        }

        private void CleanContext()
        {
            if (this.ChangeTracker.HasChanges())
            {
                var _list = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Modified || p.State == EntityState.Added || p.State == EntityState.Deleted).ToList();
                foreach (var item in _list)
                {
                    item.State = EntityState.Unchanged;
                }
            }
        }


    }
}
