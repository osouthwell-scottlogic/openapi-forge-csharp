const toParamName = require("./toParamName");

const updatePathParamPlaceholders = (path) => path.replace(/{(.*?)}/g, (match) => '{' + toParamName(match) + '}');

module.exports = updatePathParamPlaceholders;