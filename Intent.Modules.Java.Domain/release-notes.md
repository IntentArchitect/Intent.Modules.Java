### Version 4.0.1

- Names of generated Domain Models are now always PascalCased.
- Updated `DomainModelDecorator` so that all non-overridden members will no longer return `null` causing potential exceptions.
- Enums improvements:
    - Now generate with snake_cased and all upper cased literals.
    - Literal values are now applied. If all literal values are blank, then literals have no value, if all can be parsed as an integer, then the value type will be `int`, otherwise `long` is attempted and if that fails then it will be of type `String`.
    - Enum names are now always PascalCased.

### Version 4.0.0

- Removed `AbstractEntity` and its base decorator.
- Generic types are now supported.

### Version 3.3.9

- New: Enum template added.
- Fix: Inheritance on Entities is now applied.

### Version 3.3.8

- Update: Modernized `pom.xml` file entries.
