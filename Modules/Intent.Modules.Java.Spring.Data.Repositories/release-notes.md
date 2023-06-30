### Version 4.2.0

- Fixed Repository Query generation so that Entity name is used in the HQL query instead of the Repository name.
- Updated `EntityRepository` template role from `Data` to `Data.Repositories` to make it more generically discoverable.
- Updated the signature of `GetEntityRepositoryName()` to be more generic.

### Version 4.1.2

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 4.0.2

- Update: Repositories will now only be generated for Entities that are Aggregate Roots or have explicit Repositories defined.
- Repositories by default now have `@IntentIgnoreBody` instead of `@IntentMerge`.

### Version 4.0.1

- Update: Names of generated repositories are now always PascalCased.

### Version 4.0.0

- Update: Repositories are no longer generated for abstract classes unless they have the `Table` stereotype applied to them.
- New: Added support for composite primary keys.