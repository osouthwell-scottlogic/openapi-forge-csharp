const Handlebars = require("handlebars");
const toParamName = require("./toParamName");
const newLineAndAlignment = "\n\t\t\t\t\t";

const getQueryParameters = (params) => {
    return Array.isArray(params)
        ? params.filter(p => p.in === 'query')
        : [];
};

const isArrayParam = (param) => param.schema.type === 'array';

const isStringArrayParam = (param) => param.schema.type === 'array' && param.schema.items && param.schema.items.type === 'string' && (param.schema.items.format === undefined || param.schema.items.format === 'string');

const isStringParam = (param) => param.schema.type === 'string' && (param.schema.format === undefined || param.schema.format === 'string');

const asQueryParam = (param) => isStringParam(param) ? `WebUtility.UrlEncode(${toParamName(param.name)})` : toParamName(param.name);

const createQueryString = (params) => {

    const queryParams = getQueryParameters(params);

    if (queryParams.length === 0) {
        return "";
    }

    let queryStringCode = `var queryString = new StringBuilder();` + newLineAndAlignment;

    for (let i = 0; i < queryParams.length; i++) {
        const queryString = isArrayParam(queryParams[i])
            ? `{string.Join("&", ${toParamName(queryParams[i].name)}.Select(p => $"${queryParams[i].name}={${isStringArrayParam(queryParams[i]) ? "WebUtility.UrlEncode(p)" : "p"}}"))}`
            : `${queryParams[i].name}={${asQueryParam(queryParams[i])}}`;

        queryStringCode += (`queryString.Append($"${i === 0 ? '?' : '&'}${queryString}` + "\");" + newLineAndAlignment);
    }

    return new Handlebars.SafeString(queryStringCode);
};

module.exports = createQueryString;