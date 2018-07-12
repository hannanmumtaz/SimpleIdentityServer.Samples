import $ from 'jquery';

module.exports = {
	/**
	* Authenticate the end-user with the login & password.
	*/
	authenticate: function(request) {
		return new Promise(function(resolve, reject){
			var data = JSON.stringify(request);
			$.ajax({
				url : '/Home/Authenticate',
				method: 'POST',
				data: data,
				contentType: 'application/json'
			}).then(function(r) {
				resolve(r);
			}).fail(function(e) {
				reject(e);
			});
		});
	}
};