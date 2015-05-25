/**
 * wechat login/subscribe
 */

window.QRLogin || (function() {

    // simple load script with timeout
    function loadScript(settings) {

		var url = settings.url;
		var timeout = 'timeout' in settings ? settings.timeout : 0;
		var onSuccess = 'success' in settings ? settings.success : function() {};
		var onError = 'error' in settings? settings.error : function() {};


		var script, head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;

		script = document.createElement("script");

		script.src = url;

		script.onload = script.onreadystatechange = function(_, isAbort) {

			if (isAbort || !script.readyState || /loaded|complete/.test(script.readyState)) {

				script.onload = script.onreadystatechange = null;

				if (head && script.parentNode) {
					head.removeChild(script);
				}

				script = undefined;

				if (!isAbort) {
					onSuccess();
				} else {
					onError("timeout");
				}

				clearTimeout(timeoutTimer);
			}
		};

		head.insertBefore(script, head.firstChild);

		
		if (timeout != 0) {
			var timeoutTimer = setTimeout(function() { script.onload( 0, 1 ); }, timeout);
		}

    }


    // string format
    String.prototype.format = function() {
		var args = arguments;
		return this.replace(/{(\d+)}/g, function(match, number) { 
			return typeof args[number] != 'undefined'
				? args[number]
				: match
			;
		});
    };

	var time = function() {
		return Math.round(new Date().getTime()/1000);
    };


    var R = {

		_settings: {}

    };

    R._isFunction = function(obj) {
		return !!(obj && obj.constructor && obj.call && obj.apply);
    };

    
	R.initSettings = function(settings) {

		this._settings.div = settings.div;

		var url = "//login.weixin.qq.com/jslogin?appid={0}".format(settings.appid);

		if ('type' in settings) {
			url += '&type=' + settings.type;
		}

		if ('script_uri' in settings) {
			url += '&redirect_uri=' + encodeURIComponent(settings.script_uri);
		}

		if ('state' in settings) {
			url += '&state=' + encodeURIComponent(settings.state);
		}

		this._settings.url = url;

		if ('width' in settings) {
			this._settings.width = settings.width;
		} else {
			this._settings.width = 0;
		}

		if ('auto_restart' in settings) {
			this._settings.auto_restart = settings.auto_restart;
		} else {
			this._settings.auto_restart = false;
		}

		if ('onError' in settings && this._isFunction(settings.onError)) {
			this._settings.onError = settings.onError;
		}

		if ('onRefresh' in settings && this._isFunction(settings.onRefresh)) {
			this._settings.onRefresh = settings.onRefresh;
		}

		if ('onScan' in settings && this._isFunction(settings.onScan)) {
			this._settings.onScan = settings.onScan;
		}

		if ('onFinish' in settings && this._isFunction(settings.onFinish)) {
			this._settings.onFinish = settings.onFinish;
		}

	};


	R.onEvent = function(e) {

		if (e == 'refresh' && this._settings.onRefresh) {
			this._settings.onRefresh();
		}

		if (e == 'scan' && this._settings.onScan) {
			this._settings.onScan();
		}

		if (e == 'finish' && this._settings.onFinish) {
			this._settings.onFinish();
		}
	};

	R.onError = function(error) {

		if (window.console) console.log(error);

		if (this._settings.onError) {
			this._settings.onError(error);
		}
	};


	R.verify = function() {
		
		loadScript({
			url: this._settings.url + "&t=" + time(),
			timeout: 3000,
			success: function() {
				switch (window.QRLogin.code) {
				case 200:
					R.refresh(window.QRLogin.uuid);
					break;
				case 400:
				case 500:
					R.onError(window.QRLogin.error);
				}
			},
			error: function(t) {
				// retry
				if (t == "timeout") {
					R.onError("init timeout");
				}
			}
		});
	};
	
	
	R.refresh = function(uuid) {
		var img;
		var qrcode = "//login.weixin.qq.com/qrcode/" + uuid;
		if (this._settings.width != 0) {
			img = "<img src=\"" + qrcode + "\" width=" + this._settings.width + " " + "height=" + this._settings.width + " />";
		} else {
			img = "<img src=\"" + qrcode + "\" />";
		}
		if (this._settings.div.charAt(0) == '#') {
			var target = document.getElementById(this._settings.div.substr(1));
			target.innerHTML = img;
		} else if (this._settings.div.charAt(0) == '.') {
			var targets = document.getElementsByClassName(this._settings.div.substr(1));
			for (var i = 0; i < targets.length; i++) {
				targets[i].innerHTML = img;
			}
		} else {
			// not implemented
		}

		this.onEvent("refresh");
		this.poll(uuid, 1);

	};


	R.poll = function(uuid, show_tip) {

		var url = "//login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?uuid={0}&tip={1}"
			.format(uuid, show_tip);

		var pollTimeout = function(t) {
			setTimeout(function() { window.QRLogin.RunTime.poll(uuid, show_tip); } , t);
		};

		loadScript({
			url: url + "&t=" + time(),
			timeout: 120000,
			success: function() {

				switch (window.code) {
				case 200:
					loadScript({url: window.redirect_uri});
					R.onEvent("finish");
					if (R._settings.auto_restart) {
						R.verify();
					}
					break;
				case 201:
					show_tip = 0;
					R.onEvent("scan");
				case 408:
					pollTimeout(1000);
					break;
				case 400:
				case 500:
					// restart;
					R.verify();
					break;
				}
			},
			error: function(t) {
				t == "timeout" ? pollTimeout(500) : pollTimeout(2000);
			}
		});
	};


	R.run = R.verify;

	var Q = {};

	Q.RunTime = R;

	Q.setup = function(settings) {

		if ('div' in settings && 'appid' in settings) {

			Q.RunTime.initSettings(settings);
			
			Q.RunTime.run();
			
			return true;
		}

		return false;
	};

	window.QRLogin = Q;
	window.setTimeout(function() {
		if (window.QRAsyncInit && !window.QRAsyncInit.hasRun) {
			window.QRAsyncInit.hasRun = true;
			window.QRAsyncInit();
		}
	}, 0);
	
})();

