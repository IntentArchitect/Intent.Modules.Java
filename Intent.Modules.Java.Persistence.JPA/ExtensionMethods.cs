using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;

namespace Intent.Modules.Java.Persistence.JPA
{
    public static class ExtensionMethods
    {
        public static (IReadOnlyList<AttributeModel> PrimaryKeys, ClassModel FromClass) GetPrimaryKeys(this ClassModel model)
        {
            while (model != null)
            {
                var primaryKeys = model.Attributes.Where(x => x.HasPrimaryKey()).ToArray();
                if (primaryKeys.Length > 0)
                {
                    return (primaryKeys, model);
                }

                model = model.ParentClass;
            }

            return (Array.Empty<AttributeModel>(), null);
        }

        public static IEnumerable<ClassModel> GetParentClasses(this ClassModel model)
        {
            while (model.ParentClass != null)
            {
                yield return model.ParentClass;
                model = model.ParentClass;
            }
        }

        public static IEnumerable<ClassModel> GetChildClasses(this ClassModel model)
        {
            foreach (var childClass in model.ChildClasses)
            {
                yield return childClass;
                foreach (var classModel in childClass.GetChildClasses())
                {
                    yield return classModel;
                }
            }
        }
    }
}
