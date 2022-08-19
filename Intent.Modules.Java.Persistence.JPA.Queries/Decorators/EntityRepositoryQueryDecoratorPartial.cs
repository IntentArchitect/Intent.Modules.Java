using System.Collections.Generic;

namespace Intent.Modules.Java.Persistence.JPA.Queries.Decorators;

public partial class EntityRepositoryQueryDecorator
{
    // This is in a partial file for now as the current RoslynWeaver (3.3.5) throws an exception when it encounters a "record struct".
    private record struct QueryData(
        IReadOnlyList<(string Name, string Alias)> Tables,
        IReadOnlyList<string> SelectColumns,
        IReadOnlyList<string> WhereClauses,
        IReadOnlyList<string> Parameters,
        IReadOnlyList<string> AnnotatedParameters);
}