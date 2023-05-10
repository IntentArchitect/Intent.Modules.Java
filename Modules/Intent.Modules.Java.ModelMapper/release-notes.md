### Version 3.3.11

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 3.3.9

- PascalCasing is now applied to entity names for `mapFrom<EntityName>(s)` methods.
- `MatchingStrategy` changed to `MatchingStrategies.STRICT` to avoid exceptions being thrown due to ambiguous mappings for certain scenarios.
