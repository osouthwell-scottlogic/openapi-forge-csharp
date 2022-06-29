const toSafeName = (name) => {
    name = name.replace(/[^a-z0-9]/gi, '');
    return name.charAt(0).toUpperCase() + name.substr(1);
};

module.exports = toSafeName;