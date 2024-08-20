### Version 4.0.1

- Fixed: Mapped boolean fields have `is` prefix instead of `get`.
- Fixed: `mapFrom` mapping methods in DTOs will check for possible naming clashes.

### Version 4.0.0

- Improvement: Updated dependencies to use separate `Intent.Code.Weaving.Java` module.

### Version 3.4.0

- Improvement: Updated dependencies.

### Version 3.3.11

- Improvement: Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 3.3.9

- Improvement: PascalCasing is now applied to entity names for `mapFrom<EntityName>(s)` methods.
- Improvement: `MatchingStrategy` changed to `MatchingStrategies.STRICT` to avoid exceptions being thrown due to ambiguous mappings for certain scenarios.
