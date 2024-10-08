### Version 4.0.2

- Fixed: Setting the Lombok package version for out the box compilations to work.

### Version 4.0.1

- New Feature: Added pagination feature for Service operations.

### Version 4.0.0

- Improvement: Updated dependencies to use separate `Intent.Code.Weaving.Java` module.

### Version 3.6.0

- Improvement: Support for Spring Boot v3.1.3.
- Improvement: Java Properties can now cater for comments as well introduced by Modules.

### Version 3.5.1

- Fixed: Parameter set to `From Body` will now add `@RequestBody`. 

### Version 3.5.0

- New: Application Template has been rebuilt with File Builder pattern.
- Update: Rest Controllers will now return Http response codes that correlate with the Open API description for successful responses.

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
