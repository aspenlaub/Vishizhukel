using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core.Data {
    public abstract class ContextBase : DbContext {
        protected EnvironmentType EnvironmentType;
        protected SynchronizationContext UiSynchronizationContext;
        protected static List<Type> DbSetTypes;

        // ReSharper disable once PublicConstructorInAbstractClass
        public ContextBase(EnvironmentType environmentType, SynchronizationContext uiSynchronizationContext, string connectionStringPrefix, ConnectionStringInfos connectionStringInfos) : base(ConnectionString(environmentType, connectionStringPrefix)) {
            EnvironmentType = environmentType;
            UiSynchronizationContext = uiSynchronizationContext;
            SetDbSetProperties();
        }

        public static void SetAutoMigration<T, TC>() where T : ContextBase where TC : DbMigrationsConfiguration<T>, new() {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<T, TC>());
        }

        public void Initialise(bool force) {
            Database.Initialize(force);
        }

        public static string ConnectionString(EnvironmentType environmentType, string prefix) {
            var componentProvider = new ComponentProvider();
            var connectionStringInfosSecret = new SecretConnectionStringInfos();
            var errorsAndInfos = new ErrorsAndInfos();
            var connectionStringInfos = componentProvider.SecretRepository.Get(connectionStringInfosSecret, errorsAndInfos);
            if (errorsAndInfos.AnyErrors()) {
                throw new Exception(string.Join("\r\n", errorsAndInfos.Errors));
            }

            var connectionStringInfo = connectionStringInfos.FirstOrDefault(c => prefix.Contains(c.Namespace) && c.EnvironmentType == environmentType);
            if (connectionStringInfo != null) {
                return connectionStringInfo.ConnectionString;
            }

            switch (environmentType) {
                case EnvironmentType.UnitTest:
                    return ConnectionString(prefix + ".UnitTest");
                case EnvironmentType.Qualification:
                    return ConnectionString(prefix + ".Test");
                case EnvironmentType.Production:
                    return ConnectionString(prefix);
                default:
                    throw new NotImplementedException("Unknown environment.");
            }
        }

        protected static string ConnectionString(string databaseName) {
            // ReSharper disable once UseStringInterpolation
            return string.Format(@"Data Source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True", databaseName);
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
            if (DbSetTypes != null && DbSetTypes.Count != 0) { return; }

            DbSetTypes = new List<Type>();
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var property in GetType().GetProperties()) {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof(IDbSet<>).FullName) != null);
                if (!isDbSet) { continue; }

                var type = setType.GetInterface(typeof(IDbSet<>).FullName).GenericTypeArguments[0];
                DbSetTypes.Add(type);
            }
        }
    }
}
