const Handlebars = require("handlebars");
const toParamName = require('./toParamName');
const getParametersByType = require("./getParametersByType");

const createHeaderParamsSnippet = (sortedParams) => {
    let headerSnippet = "";

    const headerParams = getParametersByType(sortedParams, "header");
    if (headerParams.length === 0) {
        return headerSnippet;
    }

    for (const headerParam of headerParams) {
        // only supports default serialization: style: simple & explode: false
        if (headerParam.content) {
            continue;
        }
        const safeParamName = toParamName(headerParam.name);
        switch (headerParam.schema.type) {
            case "array":
                headerSnippet += `request.Headers.Add("${headerParam.name}", string.Join(",", ${safeParamName}))` + ";\n";
            case "object":
                let serialisedObject = "";
                for (const [propName, objProp] of Object.entries(headerParam.schema.properties)) {
                    serialisedObject += `${propName},${safeParamName}.${propName}`;
                }
                headerSnippet += `request.Headers.Add("${headerParam.name}", ${serialisedObject})` + ";\n";
            default:
                headerSnippet += `request.Headers.Add("${headerParam.name}", ${safeParamName})` + ";\n";
        }
    }

    return new Handlebars.SafeString(headerSnippet);
};

module.exports = createHeaderParamsSnippet;
