const applicationJson = "application/json";

const bodyParameterExists = (params) => {
    return Array.isArray(params) && params.some(p => p.name === 'body');
};

module.exports = bodyParameterExists;