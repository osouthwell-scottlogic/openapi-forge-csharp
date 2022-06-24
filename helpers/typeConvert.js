fromFormat = (propFormat) => {
  switch (propFormat) {
    case "int32":
      return "int";
    case "int64":
      return "long";
    case "float":
      return "float";
    case "double":
      return "double";
    case "byte":
    case "binary":
      return "string";
    case "date":
    case "date-time":
      return "DateTime";
    default:
      return "";
  }
};

fromType = (propType, additionalProperties, items) => {
  switch (propType) {
    case "boolean":
      return "bool";
    case "string":
      return "string";
    case "array":
      return `${typeConvert(items)}[]`;
    // inline object definition
    case "object":
      if (additionalProperties && additionalProperties.type === "object") {
        return typeConvert(additionalProperties);
      } else {
        return "object";
      }
    default:
      return "";
  }
};

function typeConvert(prop) {
  if (prop == undefined) return "";

  // resolve references
  if (prop.$ref) {
    return prop.$ref.split("/").pop();
  }

  const type = prop.format
    ? fromFormat(prop.format)
    : fromType(prop.type, prop.additionalProperties, prop.items);

  return type === ""
    ? "object"
    : type;
}

module.exports = typeConvert;
