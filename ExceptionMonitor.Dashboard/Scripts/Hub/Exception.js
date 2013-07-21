function Exception() {

	var exceptionHub;
	var isSpainPaused = false;
	var isUkPaused = false;
	var isItalyPaused = false;
	var maxListSize = 20;

	var init = function () {

		setupClientEvents();
		setupHiddenLists();
		exceptionHub = $.connection.exceptionHub;

		exceptionHub.client.displayException = function(exception) {
			console.log('message is: ' + exception.Message);
			console.log('country is: ' + exception.Country);

			var thisCountryIsPaused = isCountryListPaused(exception.Country);
			
			var exceptionContainers = getExceptionHtmlContainers(exception.Country);
			
			if (!thisCountryIsPaused) {
				buildUpHtmlForListItem(exception, exceptionContainers.list);
				playSound(exception.Type);
			} else {
				buildUpHtmlForListItem(exception, exceptionContainers.listHidden);
				displayPendingExceptionsCount(exceptionContainers.pendingNotification, exceptionContainers.listHidden);
			}                
		};
		
		$.connection.hub.logging = true;
		$.connection.hub.start()
			.done(function () {
				console.log("Connected!");
				$(document).trigger("Connected");
			})
			.fail(function () { console.log("Could not Connect!"); });

		function setupClientEvents() {
			$("a#uk-paused").click(function () {
				togglePausedButtonState(this, isUkPaused);
				if (isUkPaused) {
					isUkPaused = false;
					replayHiddenItems("ul#uk-list", "ul#uk-list-hidden");
					hideNotificationCounter("div#uk-pending-notification");
				} else {
					isUkPaused = true;
				}
			});
			$("a#spain-paused").click(function () {
				togglePausedButtonState(this, isSpainPaused);
				if (isSpainPaused) {
					isSpainPaused = false;
					replayHiddenItems("ul#spain-list", "ul#spain-list-hidden");
					hideNotificationCounter("div#spain-pending-notification");
				} else {
					isSpainPaused = true;
				}
			});
			$("a#italy-paused").click(function () {
				togglePausedButtonState(this, isItalyPaused);
				if (isItalyPaused) {
					isItalyPaused = false;
					replayHiddenItems("ul#italy-list", "ul#italy-list-hidden");
					hideNotificationCounter("div#italy-pending-notification");
				} else {
					isItalyPaused = true;
				}
			});
		}
		
		function replayHiddenItems(visibleList, hiddenList) {
			if ($(hiddenList).length > 0) {
				$($(hiddenList).children("li").get().reverse()).each(function() {
					$(this).hide();
					$(this).css('opacity', 0.0);
					$(this).prependTo(visibleList);
					$(this).slideDown('slow');
					$(this).animate({ opacity: 1.0 });
				});
			}
		}
		
		function hideNotificationCounter(element) {
			$(element).hide();
		}
		
		function setupHiddenLists() {
			$("ul#uk-list-hidden").hide();
			$("ul#spain-list-hidden").hide();
			$("ul#italy-list-hidden").hide();
			$("div#uk-pending-notification").hide();
			$("div#spain-pending-notification").hide();
			$("div#italy-pending-notification").hide();
		}
		
		function displayPendingExceptionsCount(countContainer, hiddenList) {
			$(countContainer).children("span.pending-count").text($(hiddenList).children("li").length);
			$(countContainer).show();
		}

		function togglePausedButtonState(element, state) {
			if (state) {
				$(element).find("i").removeClass("icon-play");
				$(element).find("i").addClass("icon-pause");
				$(element).html($(element).html().replace("Play", "Pause"));
			} else {
				$(element).find("i").removeClass("icon-pause");
				$(element).find("i").addClass("icon-play");
				$(element).html($(element).html().replace("Pause", "Play"));
			}
		}

		function getClassForExceptiontype(value) {
			if (value === 0)
			{return "unhandled";}
			else if (value === 1)
			{return "software";}
			return "unknown";
		}
		
		function getBootstrapClassForExceptiontype(value) {
			if (value === 0)
				return "alert alert-error";
			else if (value === 1)
				return "alert alert-block";
			return "unknown";
		}
		
		function isCountryListPaused(country) {
			if (country === 0 && isSpainPaused) //Spain
			{
				return true;
			}
			if (country === 1 && isItalyPaused) //Italy
			{
				return true;
			}
			if (country === 2 && isUkPaused) //Italy
			{
				return true;
			}
			return false;
		}
		
		function playSound(exceptionType) {
			console.log("exception type: " + exceptionType);
			
			if (exceptionType === 0) {
				document.getElementById('unhandled_sound').play();
			}
			else if (exceptionType === 1) {
				document.getElementById('software_sound').play();
			}
		}
		
		function buildUpHtmlForListItem(exception, list) {
			
			var exceptionTypeClass = getClassForExceptiontype(exception.Type);
			var bootstrapExceptionTypeClass = getBootstrapClassForExceptiontype(exception.Type);

			$("<li>").addClass(exceptionTypeClass)
				.append($("<div>").addClass("box-effect").addClass(bootstrapExceptionTypeClass)
					.append($("<div>").addClass("box-header")
					.append($("<div>").addClass("box-header-info")
						.append($("<h3>").text(exception.Message))
						.append($("<span>").addClass("exception-date").text(javascriptPrettifyDateTime(exception.TimeOccurred))))
						.append($("<div>").addClass("exception-actions")
						.append("<a class='btn btn-small expander' href='#'><i class='icon-circle-arrow-right'></i></a>")
						.click(function () {
							if ($(this).find("i").hasClass("icon-circle-arrow-right")) {
								$(this).closest("div.box-effect").find("div.stacktrace-content").slideDown("slow");
								$(this).find("i").removeClass("icon-circle-arrow-right");
								$(this).find("i").addClass("icon-circle-arrow-left");
							}
							else if ($(this).find("i").hasClass("icon-circle-arrow-left")) {
								$(this).closest("div.box-effect").find("div.stacktrace-content").slideUp("slow");
								$(this).find("i").removeClass("icon-circle-arrow-left");
								$(this).find("i").addClass("icon-circle-arrow-right");
							}
						})
						))
					.append($("<div>").addClass("stacktrace-content")
						.text(exception.StackTrace).hide()))
						
				.hide()
				.css('opacity', 0.0)
				.prependTo(list)
				.slideDown('slow')
				.animate({ opacity: 1.0 });

			
			var n = $(list).children().length;
			if (n > maxListSize) { $(list).children("li:last").remove(); }
		}
		
		function javascriptPrettifyDateTime(serverDateTime) {
			var clientDateTime = new Date(serverDateTime);
			return clientDateTime.toString('dddd, MMMM ,yyyy');
		}
		
		function getExceptionHtmlContainers(country) {
			switch(country) {
				case 0:
					return { list: "ul#spain-list", listHidden: "ul#spain-list-hidden", pendingNotification: "div#spain-pending-notification" };
				case 1:
					return { list: "ul#italy-list", listHidden: "ul#italy-list-hidden", pendingNotification: "div#italy-pending-notification" };
				case 2:
					return { list: "ul#uk-list", listHidden: "ul#uk-list-hidden", pendingNotification: "div#uk-pending-notification" };
			}
			
			return undefined;
		}

	};
	
	init();
};