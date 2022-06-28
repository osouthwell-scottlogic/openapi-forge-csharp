const Handlebars = require("handlebars");
const typeConvert = require('./typeConvert');

const nullableTypeConvert = (schema, optional) => {
    const schemaType = typeConvert(schema);
    if (!optional) {
        return new Handlebars.SafeString(schemaType);
    }
    return ['bool', 'DateTime', 'float', 'double', 'int', 'long'].some(s => s === schemaType.toLowerCase()) ? `${schemaType}?` : schemaType;
};

module.exports = nullableTypeConvert;