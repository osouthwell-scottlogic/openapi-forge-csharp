# BDD Test implementations

This projects implements the feature definitions defined by (Openap-Forge)[https://github.com/ScottLogic/openapi-forge/tree/master/features]

## Details

The project utilizes [Xunit.Gherkin.Quick](https://github.com/ttutisani/Xunit.Gherkin.Quick) package to integrate Gherkin specifications with XUnit.

Each feature file is implemented in its own test Class. Each scenario has it's own API project generated, built and it's types are loaded back into the test execution context.
