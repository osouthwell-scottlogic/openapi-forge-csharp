const Handlebars = require("handlebars");

const getQueryParameters = (params) => {
    return Array.isArray(params)
        ? params.filter(p => p.in === 'query' && p.name !== 'body')
        : [];
};

const isArrayParam = (param) => param.schema.type === 'array';

const isStringArrayParam = (param) => param.schema.type === 'array' && param.schema.items && param.schema.items.type === 'string';

const isStringParam = (param) => param.schema.type === 'string';

const assQueryParam = (param) => isStringParam(param) ? `WebUtility.UrlEncode(${param.name})` : param.name;

const createQueryString = (params) => {

    const queryParams = getQueryParameters(params);

    if (queryParams.length === 0) {
        return "";
    }

    let queryStringCode = `var queryString = new StringBuilder();` + "\n\t\t\t\t\t";

    for (let i = 0; i < queryParams.length; i++) {
        const queryString = isArrayParam(queryParams[i])
            ? `{string.Join("&", ${queryParams[i].name}.Select(p => $"${queryParams[i].name}={${isStringArrayParam(queryParams[i]) ? "WebUtility.UrlEncode(p)" : "p"}}"))}`
            : `${queryParams[i].name}={${assQueryParam(queryParams[i])}}`;

        queryStringCode += (`queryString.Append($"${i === 0 ? '?' : '&'}${queryString}` + "\");\n\t\t\t\t\t");
    }

    return new Handlebars.SafeString(queryStringCode);
};

module.exports = createQueryString;