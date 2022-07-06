const Handlebars = require("handlebars");
const toParamName = require("./toParamName");
const getParametersByType = require("./getParametersByType");
const newLineAndAlignment = "\n\t\t\t\t\t";

const isStringType = (typeDef) => typeDef.type === 'string' && (typeDef.format === undefined || typeDef.format === 'string');

const isStringArrayParam = (param) => param.schema.type === 'array' && param.schema.items && isStringType(param.schema.items);

const asQueryParam = (param) => isStringType(param.schema) ? `Uri.EscapeDataString(${toParamName(param.name)})` : toParamName(param.name);

const serialiseArrayParam = (param) => `{string.Join("&", ${toParamName(param.name)}.Select(p => $"${param.name}={${isStringArrayParam(param) ? "Uri.EscapeDataString(p)" : "p"}}"))}`;

const serialiseObjectParam = (param) => {
    console.log(param);
    const safeParamName = toParamName(param.name);
    let serialisedObject = "";
    for (const [propName, objProp] of Object.entries(param.schema.properties)) {
        serialisedObject += isStringType(objProp)
            ? `${propName}={Uri.EscapeDataString(${safeParamName}.${propName})}`
            : `${propName}=${safeParamName}.${propName}`;
    }
    console.log(serialisedObject);
    return serialisedObject;
};

const createQueryStringSnippet = (params) => {

    const queryParams = getParametersByType(params, "query");

    if (queryParams.length === 0) {
        return "";
    }

    let queryStringSnippet = `var queryString = new StringBuilder();` + newLineAndAlignment;

    for (const queryParam of queryParams) {
        let serialisedQueryParam;
        switch (queryParam.schema.type) {
            case "array":
                serialisedQueryParam = serialiseArrayParam(queryParam);
                break;
            case "object":
                serialisedQueryParam = serialiseObjectParam(queryParam);
                break;
            default:
                serialisedQueryParam = `${queryParam.name}={${asQueryParam(queryParam)}}`;
                break;
        }

        queryStringSnippet += (`queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}${serialisedQueryParam}` + "\");" + newLineAndAlignment);
    }

    return new Handlebars.SafeString(queryStringSnippet);
};

module.exports = createQueryStringSnippet;
