### Version 3.3.12

- It's now possible to choose between "singularized" and "pluralized" conventions for default table names in Application Settings. Table names can still be overwritten using the `Table` stereotype.

### Version 3.3.11

- Fixed: `.FindById(...)` would fail to find items when `UUID` was used as the primary key type.

### Version 3.3.10

- Fixed: JPA limited us in having Foreign Key properties and Navigational Properties. So for many-to-one relationships, the Foreign Key Id fields can be set but the Navigational properties can only be read.

### Version 3.3.9

- Changed eager loading behaviour to be `LAZY` by default.
- Fixed: Generated column names now respect column names as specified in designers.
