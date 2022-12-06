### Version 4.0.1

- Names of generated CompositeIds are now always PascalCased.

### Version 4.0.0

- It's now possible to choose between `Singularized` and `Pluralized` conventions for default table names in Application Settings. Table names can still be overridden using the `Table` stereotype.
- Can now choose between `H2`, `MySQL`, `PostgreSQL` and `SQL Server` as the database provider.
- Updated to work after removal of `AbstractEntity` from `Intent.Java.Domain` module.
- Abstract classes will no longer be annotated as if they're normal entities and won't cause tables to be created for them. This can be overridden by applying the `Table` stereotype to them.
- Single Table inheritance strategy is now explicitly applied. The `Table` stereotype can be applied to classes to force them to have their own table.
- Added support for composite primary keys.
- When an Attribute has the `Column` stereotype applied and its `Type` is detected as being for JSON for the selected database provider, the appropriate annotation's are applied to enable automatic serialization to and from JSON.

### Version 3.3.11

- Fixed: `.FindById(...)` would fail to find items when `UUID` was used as the primary key type.

### Version 3.3.10

- Fixed: JPA limited us in having Foreign Key properties and Navigational Properties. So for many-to-one relationships, the Foreign Key Id fields can be set but the Navigational properties can only be read.

### Version 3.3.9

- Changed eager loading behaviour to be `LAZY` by default.
- Fixed: Generated column names now respect column names as specified in designers.
