﻿define([], function() {
    /*  
 * A JavaScript implementation of the RSA Data Security, Inc. MD5 Message  
 * Digest Algorithm, as defined in RFC 1321.  
 * Version 2.1 Copyright (C) Paul Johnston 1999 - 2002.  
 * Other contributors: Greg Holt, Andrew Kepert, Ydnar, Lostinet  
 * Distributed under the BSD License  
 * See http://pajhome.org.uk/crypt/md5 for more info.  
 */
    var md5={};
    var hexcase = 0;
    var b64Pad = "";
    var chrsz = 8;
    md5.hexMd5=function(s) { return binl2Hex(coreMd5(str2Binl(s), s.length * chrsz)); }
    md5.b64Md5=function(s) { return binl2B64(coreMd5(str2Binl(s), s.length * chrsz)); }
    md5.strMd5=function(s) { return binl2Str(coreMd5(str2Binl(s), s.length * chrsz)); }
    md5.hexHmacMd5=function(key, data) { return binl2Hex(coreHmacMd5(key, data)); }
    md5.b64HmacMd5=function(key, data) { return binl2B64(coreHmacMd5(key, data)); }
    md5.strHmacMd5=function(key, data) { return binl2Str(coreHmacMd5(key, data)); }
    function coreMd5(x, len) {
        x[len >> 5] |= 0x80 << ((len) % 32);
        x[(((len + 64) >>> 9) << 4) + 14] = len;

        var a = 1732584193;
        var b = -271733879;
        var c = -1732584194;
        var d = 271733878;

        for (var i = 0; i < x.length; i += 16) {
            var olda = a;
            var oldb = b;
            var oldc = c;
            var oldd = d;

            a = md5Ff(a, b, c, d, x[i + 0], 7, -680876936);
            d = md5Ff(d, a, b, c, x[i + 1], 12, -389564586);
            c = md5Ff(c, d, a, b, x[i + 2], 17, 606105819);
            b = md5Ff(b, c, d, a, x[i + 3], 22, -1044525330);
            a = md5Ff(a, b, c, d, x[i + 4], 7, -176418897);
            d = md5Ff(d, a, b, c, x[i + 5], 12, 1200080426);
            c = md5Ff(c, d, a, b, x[i + 6], 17, -1473231341);
            b = md5Ff(b, c, d, a, x[i + 7], 22, -45705983);
            a = md5Ff(a, b, c, d, x[i + 8], 7, 1770035416);
            d = md5Ff(d, a, b, c, x[i + 9], 12, -1958414417);
            c = md5Ff(c, d, a, b, x[i + 10], 17, -42063);
            b = md5Ff(b, c, d, a, x[i + 11], 22, -1990404162);
            a = md5Ff(a, b, c, d, x[i + 12], 7, 1804603682);
            d = md5Ff(d, a, b, c, x[i + 13], 12, -40341101);
            c = md5Ff(c, d, a, b, x[i + 14], 17, -1502002290);
            b = md5Ff(b, c, d, a, x[i + 15], 22, 1236535329);

            a = md5Gg(a, b, c, d, x[i + 1], 5, -165796510);
            d = md5Gg(d, a, b, c, x[i + 6], 9, -1069501632);
            c = md5Gg(c, d, a, b, x[i + 11], 14, 643717713);
            b = md5Gg(b, c, d, a, x[i + 0], 20, -373897302);
            a = md5Gg(a, b, c, d, x[i + 5], 5, -701558691);
            d = md5Gg(d, a, b, c, x[i + 10], 9, 38016083);
            c = md5Gg(c, d, a, b, x[i + 15], 14, -660478335);
            b = md5Gg(b, c, d, a, x[i + 4], 20, -405537848);
            a = md5Gg(a, b, c, d, x[i + 9], 5, 568446438);
            d = md5Gg(d, a, b, c, x[i + 14], 9, -1019803690);
            c = md5Gg(c, d, a, b, x[i + 3], 14, -187363961);
            b = md5Gg(b, c, d, a, x[i + 8], 20, 1163531501);
            a = md5Gg(a, b, c, d, x[i + 13], 5, -1444681467);
            d = md5Gg(d, a, b, c, x[i + 2], 9, -51403784);
            c = md5Gg(c, d, a, b, x[i + 7], 14, 1735328473);
            b = md5Gg(b, c, d, a, x[i + 12], 20, -1926607734);


            a = md5Hh(a, b, c, d, x[i + 5], 4, -378558);
            d = md5Hh(d, a, b, c, x[i + 8], 11, -2022574463);
            c = md5Hh(c, d, a, b, x[i + 11], 16, 1839030562);
            b = md5Hh(b, c, d, a, x[i + 14], 23, -35309556);
            a = md5Hh(a, b, c, d, x[i + 1], 4, -1530992060);
            d = md5Hh(d, a, b, c, x[i + 4], 11, 1272893353);
            c = md5Hh(c, d, a, b, x[i + 7], 16, -155497632);
            b = md5Hh(b, c, d, a, x[i + 10], 23, -1094730640);
            a = md5Hh(a, b, c, d, x[i + 13], 4, 681279174);
            d = md5Hh(d, a, b, c, x[i + 0], 11, -358537222);
            c = md5Hh(c, d, a, b, x[i + 3], 16, -722521979);
            b = md5Hh(b, c, d, a, x[i + 6], 23, 76029189);
            a = md5Hh(a, b, c, d, x[i + 9], 4, -640364487);
            d = md5Hh(d, a, b, c, x[i + 12], 11, -421815835);
            c = md5Hh(c, d, a, b, x[i + 15], 16, 530742520);
            b = md5Hh(b, c, d, a, x[i + 2], 23, -995338651);

            a = md5Ii(a, b, c, d, x[i + 0], 6, -198630844);
            d = md5Ii(d, a, b, c, x[i + 7], 10, 1126891415);
            c = md5Ii(c, d, a, b, x[i + 14], 15, -1416354905);
            b = md5Ii(b, c, d, a, x[i + 5], 21, -57434055);
            a = md5Ii(a, b, c, d, x[i + 12], 6, 1700485571);
            d = md5Ii(d, a, b, c, x[i + 3], 10, -1894986606);
            c = md5Ii(c, d, a, b, x[i + 10], 15, -1051523);
            b = md5Ii(b, c, d, a, x[i + 1], 21, -2054922799);
            a = md5Ii(a, b, c, d, x[i + 8], 6, 1873313359);
            d = md5Ii(d, a, b, c, x[i + 15], 10, -30611744);
            c = md5Ii(c, d, a, b, x[i + 6], 15, -1560198380);
            b = md5Ii(b, c, d, a, x[i + 13], 21, 1309151649);
            a = md5Ii(a, b, c, d, x[i + 4], 6, -145523070);
            d = md5Ii(d, a, b, c, x[i + 11], 10, -1120210379);
            c = md5Ii(c, d, a, b, x[i + 2], 15, 718787259);
            b = md5Ii(b, c, d, a, x[i + 9], 21, -343485551);

            a = safeAdd(a, olda);
            b = safeAdd(b, oldb);
            c = safeAdd(c, oldc);
            d = safeAdd(d, oldd);
        }
        return Array(a, b, c, d);
    }
    function md5Cmn(q, a, b, x, s, t) {
        return safeAdd(bitRol(safeAdd(safeAdd(a, q), safeAdd(x, t)), s), b);
    }
    function md5Ff(a, b, c, d, x, s, t) {
        return md5Cmn((b & c) | ((~b) & d), a, b, x, s, t);
    }
    function md5Gg(a, b, c, d, x, s, t) {
        return md5Cmn((b & d) | (c & (~d)), a, b, x, s, t);
    }
    function md5Hh(a, b, c, d, x, s, t) {
        return md5Cmn(b ^ c ^ d, a, b, x, s, t);
    }
    function md5Ii(a, b, c, d, x, s, t) {
        return md5Cmn(c ^ (b | (~d)), a, b, x, s, t);
    }
    function coreHmacMd5(key, data) {
        var bkey = str2Binl(key);
        if (bkey.length > 16) bkey = coreMd5(bkey, key.length * chrsz);

        var ipad = Array(16), opad = Array(16);
        for (var i = 0; i < 16; i++) {
            ipad[i] = bkey[i] ^ 0x36363636;
            opad[i] = bkey[i] ^ 0x5C5C5C5C;
        }

        var hash = coreMd5(ipad.concat(str2Binl(data)), 512 + data.length * chrsz);
        return coreMd5(opad.concat(hash), 512 + 128);
    }
    function safeAdd(x, y) {
        var lsw = (x & 0xFFFF) + (y & 0xFFFF);
        var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
        return (msw << 16) | (lsw & 0xFFFF);
    }
    function bitRol(num, cnt) {
        return (num << cnt) | (num >>> (32 - cnt));
    }
    function str2Binl(str) {
        var bin = Array();
        var mask = (1 << chrsz) - 1;
        for (var i = 0; i < str.length * chrsz; i += chrsz)
            bin[i >> 5] |= (str.charCodeAt(i / chrsz) & mask) << (i % 32);
        return bin;
    }
    function binl2Str(bin) {
        var str = "";
        var mask = (1 << chrsz) - 1;
        for (var i = 0; i < bin.length * 32; i += chrsz)
            str += String.fromCharCode((bin[i >> 5] >>> (i % 32)) & mask);
        return str;
    }
    function binl2Hex(binarray) {
        var hex_tab = hexcase ? "0123456789ABCDEF" : "0123456789abcdef";
        var str = "";
        for (var i = 0; i < binarray.length * 4; i++) {
            str += hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8 + 4)) & 0xF) +
                   hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8)) & 0xF);
        }
        return str;
    }
    function binl2B64(binarray) {
        var tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        var str = "";
        for (var i = 0; i < binarray.length * 4; i += 3) {
            var triplet = (((binarray[i >> 2] >> 8 * (i % 4)) & 0xFF) << 16)
                        | (((binarray[i + 1 >> 2] >> 8 * ((i + 1) % 4)) & 0xFF) << 8)
                        | ((binarray[i + 2 >> 2] >> 8 * ((i + 2) % 4)) & 0xFF);
            for (var j = 0; j < 4; j++) {
                if (i * 8 + j * 6 > binarray.length * 32) str += b64Pad;
                else str += tab.charAt((triplet >> 6 * (3 - j)) & 0x3F);
            }
        }
        return str;
    }
    return md5;
});
