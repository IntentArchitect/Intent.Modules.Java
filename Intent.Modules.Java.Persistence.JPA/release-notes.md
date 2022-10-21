### Version 3.3.10

- Fixed: JPA limited us in having Foreign Key properties and Navigational Properties. So for many-to-one relationships, the Foreign Key Id fields can be set but the Navigational properties can only be read.

### Version 3.3.9

- Changed eager loading behaviour to be `LAZY` by default.
- Fixed: Generated column names now respect column names as specified in designers.
