// Write your Javascript code.
jQuery.validator.addMethod("nospaces", function (value, element) {
    return value.indexOf(" ") < 0 && value != "";
}, "Username has spaces.");

jQuery.validator.unobtrusive.adapters.add("nospaces", function (options) {
    options.rules["nospaces"] = true;
    if (options.message) {
        options.messages["nospaces"] = options.message;
    }
});

