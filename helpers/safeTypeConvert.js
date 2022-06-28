const Handlebars = require("handlebars");
const typeConvert = require('./typeConvert');

const safeTypeConvert = (prop) => {
    return new Handlebars.SafeString(typeConvert(prop));
};

module.exports = safeTypeConvert;