const toParamName = require("./toParamName");

const updatePathParamPlaceholders = (path) => path.replace(/{(.*?)}/g, (match, p1) => '{' + toParamName(p1) + '}');

module.exports = updatePathParamPlaceholders;