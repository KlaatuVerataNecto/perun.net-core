/* ------------------------------------------------------------------------
 * Validations
 * -------------------------------------------------------------------------
 */
var Validation = function () {

    var handle = function () {

        //$.validator.addMethod("nospaces",
        //    function (value, element) { return value.indexOf(" ") < 0 && value != "";},"Username has invalid characters. Spaces are not allowed.");

        //$.validator.unobtrusive.adapters.add("nospaces", function (options) {
        //    options.rules["nospaces"] = true;
        //    if (options.message) {options.messages["nospaces"] = options.message;}}
        //);

        //$.validator.addMethod("noweirdstuff", function (value, element) {
        //    return !(/\W/.test(value));}, "Username has invalid characters. Only letters, numbres & underscores allowed.");

        //$.validator.unobtrusive.adapters.add("noweirdstuff", function (options) {
        //    options.rules["noweirdstuff"] = true;
        //    if (options.message) { options.messages["noweirdstuff"] = options.message;}
        //});

        //$.validator.unobtrusive.adapters.add("passwordmeter", function (options) {
        //    options.rules["passwordmeter"] = true;
        //    if (options.message) {  options.messages["passwordmeter"] = options.message;}
        //});
    }

    return {
        init: function () {
            handle();
        }
    };

}();

$.validator.addMethod("nospaces",
    function (value, element) { return value.indexOf(" ") < 0 && value != ""; }, "Username has invalid characters. Spaces are not allowed.");

$.validator.unobtrusive.adapters.add("nospaces", function (options) {
    options.rules["nospaces"] = true;
    if (options.message) { options.messages["nospaces"] = options.message; }
}
);

$.validator.addMethod("noweirdstuff", function (value, element) {
    return !(/\W/.test(value));
}, "Username has invalid characters. Only letters, numbres & underscores allowed.");

$.validator.unobtrusive.adapters.add("noweirdstuff", function (options) {
    options.rules["noweirdstuff"] = true;
    if (options.message) { options.messages["noweirdstuff"] = options.message; }
});

$.validator.unobtrusive.adapters.add("passwordmeter", function (options) {
    options.rules["passwordmeter"] = true;
    if (options.message) { options.messages["passwordmeter"] = options.message; }
});