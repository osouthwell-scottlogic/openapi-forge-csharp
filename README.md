## OpenAPI Forge - CSharp

This repository is the C# template for the [OpenAPI Forge](https://github.com/ColinEberhardt/openapi-forge), see that repository for usage instructions:

https://github.com/ScottLogic/openapi-forge

## various TODOs

### add support for:

    - various object inheritence types
    - non-default parameter serialisation styles
    
## Quick Start

Clone and then navigate to root directory of the repository.

Install all the dependencies needed:
~~~
npm install
~~~
Once you have a local version, you can reference it's location as the 'generator' argument of the 'forge' command of openapi-forge. 
~~~
$ openapi-forge forge
 \ https://petstore3.swagger.io/api/v3/openapi.json
 \ {location of local generator}
 \ -o api
~~~

## Testing

Using the one command below you can automatically run the testing:
~~~
npm test
~~~
