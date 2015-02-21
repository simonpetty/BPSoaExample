using System;
using System.Linq.Expressions;
using JLT.Framework.Core;
using JLT.Framework.Service.Data;
using JLT.Framework.ServiceBus.Extensions;
using NHibernate;
using NHibernate.Type;


namespace JLT.Framework.ServiceBus.Interceptors
{
    /// <remarks />
    public class ContextIdentityAuditInterceptor : EmptyInterceptor
    {
        private static readonly string CreatedDate;
        private static readonly string CreatedBy;
        private static readonly string ChangedBy;
        private static readonly string ChangedDate;

        static ContextIdentityAuditInterceptor()
        {
            CreatedBy = GetPropertyName(x => x.CreatedBy);
            CreatedDate = GetPropertyName(x => x.CreatedDate);
            ChangedBy = GetPropertyName(x => x.ChangedBy);
            ChangedDate = GetPropertyName(x => x.ChangedDate);
        }

        private static string GetPropertyName<TProperty>(Expression<Func<IAuditable, TProperty>> property)
        {
            var memberExpression = (MemberExpression)property.Body;
            return memberExpression.Member.Name;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var auditableEntity = entity as IAuditable;
            if (auditableEntity == null)
                return true;

            UpdateState(state, propertyNames, CreatedBy, ConsumeContext.Current.GetContextIdentity());
            UpdateState(state, propertyNames, CreatedDate, DateTime.Now);
            return true;
        }

        
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
            string[] propertyNames, IType[] types)
        {
            var auditableEntity = entity as IAuditable;
            if (auditableEntity == null)
                return true;

            UpdateState(currentState, propertyNames, ChangedBy, ConsumeContext.Current.GetContextIdentity());
            UpdateState(currentState, propertyNames, ChangedDate, DateTime.Now);
            return true;
        }
        

        private static int GetPropertyIndex(string[] properties, string property)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                if (properties[i] == property)
                    return i;
            }
            return -1;
        }

        private static void UpdateState(object[] state, string[] propertyNames, string property, object updatedState)
        {
            int index = GetPropertyIndex(propertyNames, property);
            if (index == -1)
            {
                throw new JLTFrameworkException("Can't audit this entity. Audit property for " + property + " does not exist!");
            }
            state[index] = updatedState;
        }

        public override bool? IsTransient(object entity)
        {
            var persistent = entity as IPersistent;

            return (persistent != null) ? persistent.Version == 0 : base.IsTransient(entity);
        }
    }
}
