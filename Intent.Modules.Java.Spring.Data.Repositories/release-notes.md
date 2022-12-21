### Version 4.0.2

- Update: Repositories will now only be generated for Entities that are Aggregate Roots or have explicit Repositories defined.

### Version 4.0.1

- Update: Names of generated repositories are now always PascalCased.

### Version 4.0.0

- Update: Repositories are no longer generated for abstract classes unless they have the `Table` stereotype applied to them.
- New: Added support for composite primary keys.