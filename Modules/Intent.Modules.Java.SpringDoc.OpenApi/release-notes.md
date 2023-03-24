﻿### Version 1.0.2

- Comments captured for `DTO`s and `DTO Field`s are now generated as `@Schema(description = "<comment>")` on Data Transfer Models.

### Version 1.0.1

- The following annotations are now managed:
    - `@Operation`: Comments for an `Operation` in Intent will populate the '`description` field for the annotaton.
    - `@Tag`: Can be controlled by applying one or more `OpenAPI Tag` stereotypes to a `Service` or `Operation` stereotype.
    - `@Parameter`: Will automatically have `required = true` applied when the parameter does not have `Is Nullable` set in Intent. Comments for a `Parameter` in Intent will populate the '`description` field for the annotaton.

### Version 1.0.0

- Initial release.