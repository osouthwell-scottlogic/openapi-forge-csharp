const isQueryParameter = (param) => param.in === 'query' && param.name !== 'body';

module.exports = isQueryParameter;