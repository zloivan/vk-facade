# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [2.1.4] - 2024-08-12
### Add
* Add component to check ads available
* Change signature of methods: StorageGet, StorageGetKeys StorageSet. Now storage should be used only via Storage class

## [2.1.3] - 2024-08-11
### Bugfix/Refactoring
* Fixed issue with multiple requests of a single type are not handled properly

## [2.1.2] - 2024-08-04
### Bugfix/Refactoring
* Fixed error in Invite Friends method
* Null checks now done inside VKParams class

## [2.1.1] - 2024-08-04
### Bugfix/Refactoring
* Improved login system, track bug with save data to vk storage

## [2.1.0] - 2024-08-02
### Added
* Added storage requests

## [2.0.0] - 2024-08-02
### Refactor
* Update namespaces, major change requires changes in source code after update of package

## [1.0.3] - 2024-08-02
### Fixed
* Change type of params that are then passed to vkbridge

## [1.0.2] - 2024-08-01
### Added
* Fixed issue with undefined symbol in .jslib file

## [1.0.1] - 2024-07-31
### Added
* Added new method to get launch parameters

## [1.0.0] - 2024-07-30
### Initial version.
*Initial version to release*
