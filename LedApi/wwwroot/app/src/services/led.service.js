import * as tslib_1 from "tslib";
import axios from 'axios';
var LedService = /** @class */ (function () {
    function LedService() {
    }
    LedService.prototype.setLedColors = function (green, blue, red) {
        return tslib_1.__awaiter(this, void 0, void 0, function () {
            return tslib_1.__generator(this, function (_a) {
                return [2 /*return*/, axios.get("http://192.168.1.157/api/colorchange/" + green + "/" + red + "/" + blue)];
            });
        });
    };
    return LedService;
}());
export default LedService;
//# sourceMappingURL=led.service.js.map