### Version 4.2.0

- Improvement: Support for Spring Boot v3.1.3.
- Improvement: Removed `@Data` and replaced with `@Getter`, `@Setter` and `@RequiredArgsConstructor`. Read this for more details: [Lombok & Hibernate - How to Avoid Common Pitfalls](https://thorben-janssen.com/lombok-hibernate-how-to-avoid-common-pitfalls/).
- Improvement: Updated modules.

### Version 4.1.1

- Improvement: Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 4.1.0

- New Feature: Default values are now taken into account for Domain Entity Attributes.

### Version 4.0.4

- Improvement: Provided at least one of an enum's literals has a numeric value specified, all literals without values specified for the same enum will automatically have values assigned in the generated code.

### Version 4.0.3

- Improvement: Enums now support serialization.

### Version 4.0.2

- Fixed: Maven dependencies weren't always being applied.
- Fixed: `NotNull` and `Max` attributes are now applied on domain entity fields.

### Version 4.0.1

- Fixed: Names of generated Domain Models are now always PascalCased.
- Improved: Updated `DomainModelDecorator` so that all non-overridden members will no longer return `null` causing potential exceptions.
- Enums improvements:
    - Now generate with snake_cased and all upper cased literals.
    - Literal values are now applied. If all literal values are blank, then literals have no value, if all can be parsed as an integer, then the value type will be `int`, otherwise `long` is attempted and if that fails then it will be of type `String`.
    - Enum names are now always PascalCased.
- Improvement: `DomainModelDecorator` now has an overridable `BeforeTemplateExecution` method.

### Version 4.0.0

- Improvement: Removed `AbstractEntity` and its base decorator.
- New Feature: Generic types are now supported.

### Version 3.3.9

- New Feature: Enum template added.
- Fixed: Inheritance on Entities is now applied.

### Version 3.3.8

- Improvement: Modernized `pom.xml` file entries.
