function basic(title, message, type) {
	Swal.fire({
		title: "Any fool can use a computer",
		confirmButtonColor: "#5b73e8"
	})
};

function title(title, message, type) {
	Swal.fire({
		title: "The Internet?",
		text: "That thing is still around?",
		icon: "question",
		confirmButtonColor: "#5b73e8"
	})
};

function success(title, message, type) {
	Swal.fire({
		title: "Good job!",
		text: "You clicked the button!",
		icon: "success",
		showCancelButton: !0,
		confirmButtonColor: "#5b73e8",
		cancelButtonColor: "#f46a6a"
	})
};

function warning(title, message, type) {
	Swal.fire({
		title: "Are you sure?",
		text: "You won't be able to revert this!",
		icon: "warning",
		showCancelButton: !0,
		confirmButtonColor: "#34c38f",
		cancelButtonColor: "#f46a6a",
		confirmButtonText: "Yes, delete it!"
	}).then(function (t) {
		t.value && Swal.fire("Deleted!", "Your file has been deleted.", "success")
	})
};

function params(title, message, type) {
	Swal.fire({
		title: "Are you sure?",
		text: "You won't be able to revert this!",
		icon: "warning",
		showCancelButton: !0,
		confirmButtonText: "Yes, delete it!",
		cancelButtonText: "No, cancel!",
		confirmButtonClass: "btn btn-success mt-2",
		cancelButtonClass: "btn btn-danger ms-2 mt-2",
		buttonsStyling: !1
	}).then(function (t) {
		t.value ? Swal.fire({
			title: "Deleted!",
			text: "Your file has been deleted.",
			icon: "success"
		}) : t.dismiss === Swal.DismissReason.cancel && Swal.fire({
			title: "Cancelled",
			text: "Your imaginary file is safe :)",
			icon: "error"
		})
	})
};

function image(title, message, type) {
	Swal.fire({
		title: "Sweet!",
		text: "Modal with a custom image.",
		imageUrl: "assets/images/logo-dark.png",
		imageHeight: 20,
		confirmButtonColor: "#5b73e8",
		animation: !1
	})
};

function close(title, message, type) {
	var t;
	Swal.fire({
		title: "Auto close alert!",
		html: "I will close in <strong></strong> seconds.",
		timer: 2e3,
		confirmButtonColor: "#5b73e8",
		onBeforeOpen: function () {
			Swal.showLoading(), t = setInterval(function () {
				Swal.getContent().querySelector("strong").textContent = Swal.getTimerLeft()
			}, 100)
		},
		onClose: function () {
			clearInterval(t)
		}
	}).then(function (t) {
		t.dismiss === Swal.DismissReason.timer && console.log("I was closed by the timer")
	})
};

function htms_alert(title, message, type) {
	Swal.fire({
		title: "<i>HTML</i> <u>example</u>",
		icon: "info",
		html: 'You can use <b>bold text</b>, <a href="//Themesbrand.in/">links</a> and other HTML tags',
		showCloseButton: !0,
		showCancelButton: !0,
		confirmButtonClass: "btn btn-success",
		cancelButtonClass: "btn btn-danger ms-1",
		confirmButtonColor: "#47bd9a",
		cancelButtonColor: "#f46a6a",
		confirmButtonText: '<i class="fas fa-thumbs-up me-1"></i> Great!',
		cancelButtonText: '<i class="fas fa-thumbs-down"></i>'
	})
};

function position(title, message, type) {
	Swal.fire({
		position: "top-end",
		icon: "success",
		title: "Your work has been saved",
		showConfirmButton: !1,
		timer: 1500
	})
};

function padding_alert(title, message, type) {
	Swal.fire({
		title: "Custom width, padding, background.",
		width: 600,
		padding: 100,
		confirmButtonColor: "#5b73e8",
		background: "#fff url(//subtlepatterns2015.subtlepatterns.netdna-cdn.com/patterns/geometry.png)"
	})
};

function ajax_alert(title, message, type) {
	Swal.fire({
		title: "Submit email to run ajax request",
		input: "email",
		showCancelButton: !0,
		confirmButtonText: "Submit",
		showLoaderOnConfirm: !0,
		confirmButtonColor: "#5b73e8",
		cancelButtonColor: "#f46a6a",
		preConfirm: function (n) {
			return new Promise(function (t, e) {
				setTimeout(function () {
					"taken@example.com" === n ? e("This email is already taken.") : t()
				}, 2e3)
			})
		},
		allowOutsideClick: !1
	}).then(function (t) {
		Swal.fire({
			icon: "success",
			title: "Ajax request finished!",
			html: "Submitted email: " + t
		})
	})
};

function chain(title, message, type) {
	Swal.mixin({
		input: "text",
		confirmButtonText: "Next &rarr;",
		showCancelButton: !0,
		confirmButtonColor: "#5b73e8",
		cancelButtonColor: "#74788d",
		progressSteps: ["1", "2", "3"]
	}).queue([{
		title: "Question 1",
		text: "Chaining swal2 modals is easy"
	}, "Question 2", "Question 3"]).then(function (t) {
		t.value && Swal.fire({
			title: "All done!",
			html: "Your answers: <pre><code>" + JSON.stringify(t.value) + "</code></pre>",
			confirmButtonText: "Lovely!"
		})
	})
};

function dynamic_alert(title, message, type) {
	swal.queue([{
		title: "Your public IP",
		confirmButtonColor: "#5b73e8",
		confirmButtonText: "Show my public IP",
		text: "Your public IP will be received via AJAX request",
		showLoaderOnConfirm: !0,
		preConfirm: function () {
			return new Promise(function (e) {
				t.get("https://api.ipify.org?format=json").done(function (t) {
					swal.insertQueueStep(t.ip), e()
				})
			})
		}
	}]).catch(swal.noop)
};