# Use in the Postman
function interpolate(value) {
    const { Property } = require('postman-collection');
    return Property.replaceSubstitutions(value, pm.variables.toObject());
}
var uuid = require('uuid');
var moment = require("moment")

var hmacPrefix = "hmacauth";
var AppId = "8c8b3017-e88a-4ef4-941b-4f68229c2b45"; 
var APIKey = "MTIzNDU2";
var requestURI = encodeURIComponent(pm.environment.values.substitute(pm.request.url.getPathWithQuery(), null, false).toString().toLowerCase());
console.info(requestURI);
var requestMethod = pm.request.method;
var requestTimeStamp = moment(new Date().toUTCString()).valueOf() / 1000;
var nonce = uuid.v4();
var requestContentBase64String = "";
var bodyString = interpolate(pm.request.body.toString());

if (bodyString) {
    var sha1 = CryptoJS.SHA1(bodyString);
    requestContentBase64String = CryptoJS.enc.Base64.stringify(sha1);
}

var signatureRawData = AppId + requestMethod + requestURI + requestTimeStamp + nonce + requestContentBase64String; //check
var signature = CryptoJS.enc.Utf8.parse(signatureRawData);
var secretByteArray = CryptoJS.enc.Base64.parse(APIKey);
var signatureBytes = CryptoJS.HmacSHA256(signature, secretByteArray);
var requestSignatureBase64String = CryptoJS.enc.Base64.stringify(signatureBytes);

var hmacKey = hmacPrefix + " " + AppId + ":" + requestSignatureBase64String + ":" + nonce + ":" + requestTimeStamp;
postman.setEnvironmentVariable("hmacKey", hmacKey);
pm.request.headers.add({
  key: "Authorization",
  value: hmacKey
});

console.info(requestSignatureBase64String);