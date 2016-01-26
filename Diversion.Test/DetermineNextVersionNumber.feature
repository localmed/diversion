Feature: Determine Next Version Number
	In order to automate version number incrementing
	As a developer
	I want to be told the next version number based on the currently released assembly and the newly built assembly.

Scenario: The patch part of the version number should be incremented if neither the major or minor versions are incremented
	Given the currently released assembly version number is 1.1.1
	And NextVersion has been initialized with major and minor version triggers that never trigger
	Then NextVersion should determine that the next version number should be 1.1.2

Scenario: The major part of the version number should be incremented if a major version trigger is triggered
	Given the currently released assembly version number is 1.1.1
	And NextVersion has been initialized with a major version trigger that always triggers and a minor version trigger that never triggers
	Then NextVersion should determine that the next version number should be 2.0.0

Scenario: The minor part of the version number should be incremented if only a minor version trigger is triggered
	Given the currently released assembly version number is 1.1.1
	And NextVersion has been initialized with a major version trigger that never triggers and a minor version trigger that always triggers
	Then NextVersion should determine that the next version number should be 1.2.0

Scenario: The minor part of the version number should not be incremented if a major version trigger is triggered, even if a minor version trigger is triggered
	Given the currently released assembly version number is 1.1.1
	And NextVersion has been initialized with a major and minor version triggers that always trigger
	Then NextVersion should determine that the next version number should be 2.0.0
