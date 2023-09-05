### Version 4.2.0

- Improvement: Support for Spring Boot v3.1.3.
- Improvement: Updated modules.

### Version 4.1.1

- Update: It will now look at the Identity checkbox on the Primary Key stereotype to determine the GenerationType.

### Version 4.1.0

- Fixed: Database configuration even when no Domain entity is available.

### Version 4.0.5

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 4.0.3

- Fixed: Stereotype Indexes generated incorrectly in code.

### Version 4.0.2

- Fixed: `@OneToMany` annotations were missing corresponding `@JoinColumn` annotation.

### Version 4.0.1

- Names of generated CompositeIds are now always PascalCased.
- Fixed: `@Table` annotation wasn't being added for derived types.
- Regard attributes named `id` implicitly as being primary keys when no explicit primary key.
- Fixed: `cascade = javax.persistence.CascadeType.ALL` was being indiscriminately applied to all relationships, it will now only be applied on navigations from a composite owner.
- Fixed: Snake-casing was being properly applied to `@JoinColumn` `name`s for many-to-one relationships.

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
