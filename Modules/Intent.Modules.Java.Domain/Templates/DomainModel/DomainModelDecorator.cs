using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.SdkEvolutionHelpers;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.Domain.Templates.DomainModel
{
    [IntentManaged(Mode.Merge)]
    public abstract class DomainModelDecorator : ITemplateDecorator
    {
        public int Priority { get; protected set; } = 0;

        public virtual IEnumerable<string> ClassAnnotations() => Enumerable.Empty<string>();

#pragma warning disable CS0618
        public virtual string FieldAnnotations(AttributeModel model) => BeforeField(model);
#pragma warning restore CS0618

        /// <summary>
        /// Obsolete. Use <see cref="FieldAnnotations(AttributeModel)"/> instead.
        /// </summary>
        [Obsolete(WillBeRemovedIn.Version4)]
        public virtual string BeforeField(AttributeModel model) => string.Empty;

#pragma warning disable CS0618
        public virtual string FieldAnnotations(AssociationEndModel model) => BeforeField(model);
#pragma warning restore CS0618

        /// <summary>
        /// Obsolete. Use <see cref="FieldAnnotations(AssociationEndModel)"/> instead.
        /// </summary>
        [Obsolete(WillBeRemovedIn.Version4)]
        public virtual string BeforeField(AssociationEndModel model) => string.Empty;

        public virtual IEnumerable<string> Fields() => Enumerable.Empty<string>();

        public virtual IEnumerable<string> Methods() => Enumerable.Empty<string>();

        public virtual void BeforeTemplateExecution() { }
    }
}