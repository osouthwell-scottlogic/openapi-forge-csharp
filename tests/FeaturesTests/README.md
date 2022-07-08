# BDD Test implementations

This projects implements the [Openap-Forge features](https://github.com/ScottLogic/openapi-forge/tree/master/features){:target="\_blank"}

## Details

The project utilizes [Xunit.Gherkin.Quick](https://github.com/ttutisani/Xunit.Gherkin.Quick){:target="\_blank"} to integrate Gherkin specifications with XUnit.

Each feature file is implemented in its own test Class.
Each scenario has it's own API project generated, built and its types are loaded back into the test execution context.
