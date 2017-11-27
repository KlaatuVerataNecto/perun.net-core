// Write your Javascript code.
jQuery.validator.addMethod("nospaces", function (value, element) {
    return value.indexOf(" ") < 0 && value != "";
}, "Username has invalid characters. Spaces are not allowed.");

jQuery.validator.unobtrusive.adapters.add("nospaces", function (options) {
    options.rules["nospaces"] = true;
    if (options.message) {
        options.messages["nospaces"] = options.message;
    }
});

jQuery.validator.addMethod("noweirdstuff", function (value, element) {
    return !(/\W/.test(value));
}, "Username has invalid characters. Only letters, numbres & underscores allowed.");

jQuery.validator.unobtrusive.adapters.add("noweirdstuff", function (options) {
    options.rules["noweirdstuff"] = true;
    if (options.message) {
        options.messages["noweirdstuff"] = options.message;
    }
});

jQuery.validator.unobtrusive.adapters.add("passwordmeter", function (options) {
    options.rules["passwordmeter"] = true;
    if (options.message) {
        options.messages["passwordmeter"] = options.message;
    }
});

