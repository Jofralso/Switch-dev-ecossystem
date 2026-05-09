# Test Generation Skill

## Responsibilities
- generate EditMode unit tests for core systems
- generate PlayMode integration tests
- create performance benchmark tests
- validate edge cases and error paths
- generate test fixtures and mock services

## Coverage Targets
- gameplay systems: 80%+ branch coverage
- utility/helper code: 90%+ line coverage
- networking: 100% of sync paths
- save/load: 100% of serialization paths
- UI: integration tests for critical flows

## Patterns
- Arrange-Act-Assert for unit tests
- Scene-based PlayMode test fixtures
- TestFixtures for shared setup
- Parameterized tests for data-driven cases
- Custom UnityTest for frame-dependent tests

## Naming Convention
- {SystemUnderTest}_{Scenario}_{ExpectedResult}
- examples:
  - PlayerHealth_TakeDamage_ReducesHealth
  - SaveSystem_CorruptData_ThrowsRecoverableError
  - Weapon_FireAmmo_DecreasesAmmoCount
