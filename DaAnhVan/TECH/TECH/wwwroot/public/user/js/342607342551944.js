
var du = document.user;
var huid = !du ? getCookie('hrv_ajs_user_id') : undefined;
var utmc = getCookie('_haravan_utm_p');
var hutm = utmc ? decodeURIComponent(decodeURIComponent(utmc)) : undefined;
var hutmSearch = hutm ? new URLSearchParams(hutm) : undefined;
var url = window.location.href  

var cce = [{
    n: 'un',
    v: du ? du['Name'] : undefined   
}, {
    n: 'ui',
    v: du ? du['Id'] : huid ? huid : undefined
}, {
    n: 'ac',
    v: du ? du['OrgName'] : undefined   
}, {
    n: 'ai',
    v: du ? du['OrgId'] : undefined   
}, {
    n: 'ls',
    v: 'harasocial_livechat'  
}, {
    n: 'lu',
    v:  url
}, {
    n: 'us',
    v: hutmSearch ? hutmSearch.get('utm_source') : undefined    
}, {
    n: 'um',
    v: hutmSearch ? hutmSearch.get('utm_medium') : undefined   
}, {
    n: 'uc',
    v: hutmSearch ? hutmSearch.get('utm_campaign') : undefined   
}]

var segments = []
for (var i = 0; i < cce.length; i++) {
    var obj = cce[i];

    if(obj.v === undefined || obj.v === null)
        continue;
    
   segments.push(obj.n + '::' + obj.v)
}

var cces = segments && segments.length > 0 ? '__hrf_rrs_b64_' + btoa(segments.join('|')) : null;  
      
window.hrfwidget = {
    appId: '363772567412181',
    pageId: '342607342551944',
    widgets: [{"id":1159525,"type":"customer_chat","ref":"__hrf_w_1159525"}],
    checkboxs:[],
    customer_chats:[{"id":32583,"growthtool_id":1159525,"logged_in_greeting":null,"logged_out_greeting":null,"greeting_dialog_display":"hide"}],
    widgetLocale: 'vi_VN',
    fbSDKVersion: 'v16.0',
    customerChatExtras: cces
};

(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s);
    js.id = id;
    js.src = '//hstatic.net/harasocial/widget.js?v=1.1';  
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'hrfwidget-core'));

function getCookie(name) {
    var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    if(match && match.length > 2){
        var m = match[2];
        return m !== undefined && m !== 'undefined' && m !== null && m !== 'null' && m.trim() !== ''
            ? m 
            : undefined
    }
    
    return undefined;
}

function confirmOptIn(id) {
        var w = hrfwidget.widgets.filter(function(a) { return a.id == id })

        if (w && w.length > 0) {
            var x = document.getElementById(id);
            var userRef = x.getAttribute('user_ref');
            FB.AppEvents.logEvent('MessengerCheckboxUserConfirmation', null, {
                'app_id': '363772567412181',
                'page_id': '342607342551944',
                'ref': w[0].ref,
                'user_ref': userRef
            });
        }
    }