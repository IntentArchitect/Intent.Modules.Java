### Version 4.1.1

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 4.1.0

- New: Default values are now taken into account for Domain Entity Attributes.

### Version 4.0.4

- Provided at least one of an enum's literals has a numeric value specified, all literals without values specified for the same enum will automatically have values assigned in the generated code.

### Version 4.0.3

- Enums now support serialization.

### Version 4.0.2

- Maven dependencies weren't always being applied.
- `NotNull` and `Max` attributes are now applied on domain entity fields.

### Version 4.0.1

- Names of generated Domain Models are now always PascalCased.
- Updated `DomainModelDecorator` so that all non-overridden members will no longer return `null` causing potential exceptions.
- Enums improvements:
    - Now generate with snake_cased and all upper cased literals.
    - Literal values are now applied. If all literal values are blank, then literals have no value, if all can be parsed as an integer, then the value type will be `int`, otherwise `long` is attempted and if that fails then it will be of type `String`.
    - Enum names are now always PascalCased.
- `DomainModelDecorator` now has an overridable `BeforeTemplateExecution` method.

### Version 4.0.0

- Removed `AbstractEntity` and its base decorator.
- Generic types are now supported.

### Version 3.3.9

- New: Enum template added.
- Fix: Inheritance on Entities is now applied.

### Version 3.3.8

- Update: Modernized `pom.xml` file entries.
