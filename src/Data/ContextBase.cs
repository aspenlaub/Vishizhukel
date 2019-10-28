using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Microsoft.EntityFrameworkCore;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Data {
    public abstract class ContextBase : DbContext {
        protected EnvironmentType EnvironmentType;
        protected SynchronizationContext UiSynchronizationContext;
        protected static Dictionary<Type, PropertyInfo> DbSets;
        protected ConnectionStringInfos ConnectionStringInfos;
        protected string ConnectionStringPrefix;
        protected DataSources DataSources;

        protected ContextBase(EnvironmentType environmentType, SynchronizationContext uiSynchronizationContext, string connectionStringPrefix,
                ConnectionStringInfos connectionStringInfos, DataSources dataSources) {
            EnvironmentType = environmentType;
            UiSynchronizationContext = uiSynchronizationContext;
            ConnectionStringInfos = connectionStringInfos;
            ConnectionStringPrefix = connectionStringPrefix;
            DataSources = dataSources;
            SetDbSetProperties();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(ConnectionString(EnvironmentType, ConnectionStringPrefix, ConnectionStringInfos, DataSources), b => b.MigrationsAssembly(GetType().Assembly.GetName().Name));
        }

        public static string ConnectionString(EnvironmentType environmentType, string prefix, ConnectionStringInfos connectionStringInfos, DataSources dataSources) {
            var connectionStringInfo = connectionStringInfos.FirstOrDefault(c => prefix.Contains(c.Namespace) && c.EnvironmentType == environmentType);
            if (connectionStringInfo != null) {
                return connectionStringInfo.ConnectionString;
            }

            var dataSource = dataSources.FirstOrDefault(d => d.MachineId == Environment.MachineName)?.TheDataSource ?? "./SQLEXPRESS";

            switch (environmentType) {
                case EnvironmentType.UnitTest:
                    return ConnectionString(dataSource, prefix + ".UnitTest");
                case EnvironmentType.Qualification:
                    return ConnectionString(dataSource, prefix + ".Test");
                case EnvironmentType.Production:
                    return ConnectionString(dataSource, prefix);
                default:
                    throw new NotImplementedException("Unknown environment.");
            }
        }

        protected static string ConnectionString(string dataSource, string databaseName) {
            return $@"Data Source={dataSource};Initial Catalog={databaseName};Integrated Security=True;MultipleActiveResultSets=True";
        }

        public void Migrate() {
            Database.Migrate();
        }

        public override int SaveChanges() {
            return SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            if (UiSynchronizationContext == null) {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }

            var result = -1;
            UiSynchronizationContext.Send(x => {
                result = base.SaveChanges(acceptAllChangesOnSuccess);
            }, null);
            return result;
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class, IGuid {
            entities.ToList().ForEach(e => Add(e));
        }

        public new bool Add<T>(T entity) where T : class, IGuid {
            if (DbSets?.ContainsKey(typeof(T)) != true) { return false; }

            var entitySet = DbSets[typeof(T)].GetValue(this) as DbSet<T>;
            return Add(entity, entitySet);
        }

        protected bool Add<T>(object entity, DbSet<T> entitySet) where T : class, IGuid {
            if (!(entity is T)) { return false; }

            if (UiSynchronizationContext == null) {
                entitySet.Add((T)entity);
            } else {
                UiSynchronizationContext.Send(x => entitySet.Add((T)entity), null);
            }
            return true;
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class, IGuid {
            entities.ToList().ForEach(e => Remove(e));
        }

        public new bool Remove<T>(T entity) where T : class, IGuid {
            if (DbSets?.ContainsKey(typeof(T)) != true) { return false; }

            var entitySet = DbSets[typeof(T)].GetValue(this) as DbSet<T>;
            return Remove(entity, entitySet);
        }

        protected bool Remove<T>(IGuid entity, DbSet<T> entitySet) where T : class, IGuid {
            if (!(entity is T)) { return false; }

            var guid = entity.Guid;
            entity = entitySet.FirstOrDefault(c => c.Guid == guid);
            if (entity == null) { return true; }

            if (UiSynchronizationContext == null) {
                entitySet.Remove((T)entity);
            } else {
                UiSynchronizationContext.Send(x => entitySet.Remove((T)entity), null);
            }
            return true;
        }

        public void SetDbSetProperties() {
            if (DbSets != null && DbSets.Count != 0) { return; }

            DbSets = new Dictionary<Type, PropertyInfo>();
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var property in GetType().GetProperties()) {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition());
                if (!isDbSet) {
                    continue;
                }

                var type = setType.GenericTypeArguments[0];
                DbSets[type] = property;
            }
        }
    }
}
