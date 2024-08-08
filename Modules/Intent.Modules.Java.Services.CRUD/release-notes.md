### Version 4.0.1

- New Feature: Added pagination feature for Service operations.

### Version 4.0.0

- Improvement: Updated dependencies to use separate `Intent.Code.Weaving.Java` module.

### Version 3.6.0

- Improvement: Updated dependencies.

### Version 3.5.1

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 3.5.0

- Supports new CRUD approach which is agnostic to whether the services are exposed as Http Endpoints or not.

### Version 3.3.12

- Implementations will no longer generate code which causes compilation errors when classes in the domain designer have been modelled starting with a lower case letter.

### Version 3.3.11

- Updated module description.

### Version 3.3.10

- Fixed: Setting a surrogate key type as a return type for the Create CRUD operation will now return the newly created Entity's Identifier.