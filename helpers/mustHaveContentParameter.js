const mustHaveContentParameter = (verb) => {
    switch (verb.toLowerCase()) {
        case "patch":
        case "post":
        case "put":
            return true;
        default:
            return false;
    }
};

module.exports = mustHaveContentParameter;