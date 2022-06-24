const clientMethodByVerb = (httpVerb) => {
    switch (httpVerb.toUpperCase()) {
        case 'GET':
            return "GetAsync";
        case 'PUT':
            return "PutAsync";
        case 'POST':
            return "PostAsync";
        case 'DELETE':
            return 'DeleteAsync';
        case 'PATCH':
            return 'PatchAsync';
    }
};

module.exports = clientMethodByVerb;