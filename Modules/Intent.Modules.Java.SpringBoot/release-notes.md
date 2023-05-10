### Version 3.4.1

- Updated dependencies and supported client versions to prevent warnings when used with Intent Architect 4.x.

### Version 3.4.0

- Support for application services that are agnostic to whether they are exposed as Http Endpoints or not.

### Version 3.3.13

- Fixed: When a service operation's parameter had `Is Nullable` checked then `required = true` would be added to it.

### Version 3.3.12

- Fixed: Controllers would return 404 result for empty collections.

### Version 3.3.11

- Support for `ApplicationPropertyRequiredEvent` from the `Intent.Common.Java` module.

### Version 3.3.10

- Modernized `pom.xml` file entries.

### Version 3.3.9

- Add module description.

### Version 3.3.8

- Update: Updated Intent.Metadata.WebApi that will no longer automatically apply HttpSettings stereotypes but will auto add them using event scripts.

### Version 3.3.7

- New: Http Settings' Return Type Mediatype setting will determine if the primitive return type should be wrapped in a JsonResponse object or not.
