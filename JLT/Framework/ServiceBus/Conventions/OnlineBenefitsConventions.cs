using System;
using System.Globalization;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using JLT.Framework.Service.Data.Chronicle;

namespace JLT.Framework.ServiceBus.Conventions
{
    public class HasManyConvention : IHasManyConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IOneToManyCollectionInstance target)
        {
            target.Key.Column(target.EntityType.Name + "Id");
        }
    }

    public class HasManyToManyChildForeignKeyConvention : IHasManyToManyConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IManyToManyCollectionInstance target)
        {
            target.Relationship.Column(target.ChildType.Name + "Id");
        }
    }

    public class HasManyToManyParentForeignKeyConvention : IHasManyToManyConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IManyToManyCollectionInstance target)
        {
            target.Key.Column(target.EntityType.Name + "Id");
        }
    }

    public class ReferencesForeignKeyConvention : IReferenceConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IManyToOneInstance target)
        {
            target.Column(target.Property.Name + "Id");
        }
    }

    public class TableConvention : IClassConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IClassInstance target)
        {
            target.Table(String.Format(CultureInfo.InvariantCulture, "{0}s", target.EntityType.Name));
        }
    }

    public class SetDateTimeToTimestampConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => (typeof(DateTime).IsAssignableFrom(x.Property.PropertyType)
                || typeof(DateTime?).IsAssignableFrom(x.Property.PropertyType))
                && !typeof(IChronicleEntity).IsAssignableFrom(x.Property.DeclaringType));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IPropertyInstance target)
        {
            target.CustomType("timestamp");
        }
    }

    public class JoinedSubclassParentKeyConvention : IJoinedSubclassConvention
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Apply(IJoinedSubclassInstance target)
        {
            target.Key.Column("Id");
        }
    }
}
