### Version 3.4.0

- New: Upgraded Service Implementation to make use of the Java File Builder Pattern to ease extensibility.

### Version 3.3.11

- Names of generated Data Transfer Models are now always PascalCased.
- Enums improvements:
    - Now generate with snake_cased and all upper cased literals.
    - Literal values are now applied. If all literal values are blank, then literals have no value, if all can be parsed as an integer, then the value type will be `int`, otherwise `long` is attempted and if that fails then it will be of type `String`.
    - Enum names are now always PascalCased.
- Now possible to have DTOs inherit from other DTOs.

### Version 3.3.10

- Added support for annotating classes to `DataTransferModelDecorator`.
- Generate Enums from the Services designer.

### Version 3.3.9

- Modernized `pom.xml` file entries.

### 3.3.8

- Fixed: Data Transfer Models will now output to the correct nested folder location.