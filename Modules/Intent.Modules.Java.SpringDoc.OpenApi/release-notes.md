### Version 1.1.0

- Improvement: Support for Spring Boot v3.1.3.
- Improvement: Updated modules.

### Version 1.0.5

- Fixed: Deleted operation http status response will either return 200 or 204 depending on whether there is content returned or not. 

### Version 1.0.4

- Improvement: Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 1.0.3

- Improvement: Updated supported client version to [3.3.16, 5.0.0).

### Version 1.0.2

- Improvement: Comments captured for `DTO`s and `DTO Field`s are now generated as `@Schema(description = "<comment>")` on Data Transfer Models.

### Version 1.0.1

- New Feature: The following annotations are now managed:
    - `@Operation`: Comments for an `Operation` in Intent will populate the '`description` field for the annotaton.
    - `@Tag`: Can be controlled by applying one or more `OpenAPI Tag` stereotypes to a `Service` or `Operation` stereotype.
    - `@Parameter`: Will automatically have `required = true` applied when the parameter does not have `Is Nullable` set in Intent. Comments for a `Parameter` in Intent will populate the '`description` field for the annotaton.

### Version 1.0.0

- Initial release.
