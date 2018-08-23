Feature: Support Multi-Target Projects

Scenario: Changing a project from a single target project to a multi-target project where none of the new targets are compatible with the old target should force a major version change

Scenario: Changing a project from a single target project to a multi-target project by adding a new target should be a patch version change

Scenario: The version of all targets should be the greatest calculated version of any target