if (top.location.pathname === '/DashBoard/DashBoard') {
    $('.dashboard').addClass('active');

    $('#datepicker-from').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#datepicker-to').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}
    });

    $('#datetimepicker-default').datetimepicker();

    // View mode datepicker [shows only years and month]
    $('#datepicker-view-mode').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });

    // Inline datepicker
    $('#datepicker-inline').datetimepicker({
        inline: true
    });


    // Disabled Days of the Week (Disable sunday and saturday) [ 0-Sunday, 1-Monday, 2-Tuesday   3-wednesday 4-Thusday 5-Friday 6-Saturday]
    $('#datepicker-disabled-days').datetimepicker({
        daysOfWeekDisabled: [0, 6]
    });

    // Datepicker in popup
    $('#datepicker-popup-inline').datetimepicker({
        inline: true
    });

    $("[data-header-left='true']").parent().addClass("pmd-navbar-left");

    // Datepicker left header
    $('#datepicker-left-header').datetimepicker({
        'format': "YYYY-MM-DD", // HH:mm:ss
    });

    $("#datepicker-from").on("dp.change", function (e) {
        $('#datepicker-to').data("DateTimePicker").minDate(e.date);
    });
}

if (window.location.href.indexOf("/TempEmployeeMasters/Edit") > -1) {
    $('.user').addClass('active');
    $('#DateOfBirth').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#DateOfJoining').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#ConfirmationDate').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });

    $('#lastLeaveUpdated').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#datetimepicker-default').datetimepicker();

    // View mode datepicker [shows only years and month]
    $('#datepicker-view-mode').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });

    // Inline datepicker
    $('#datepicker-inline').datetimepicker({
        inline: true
    });


    // Disabled Days of the Week (Disable sunday and saturday) [ 0-Sunday, 1-Monday, 2-Tuesday   3-wednesday 4-Thusday 5-Friday 6-Saturday]
    $('#datepicker-disabled-days').datetimepicker({
        daysOfWeekDisabled: [0, 6]
    });

    // Datepicker in popup
    $('#datepicker-popup-inline').datetimepicker({
        inline: true
    });

    $("[data-header-left='true']").parent().addClass("pmd-navbar-left");

    // Datepicker left header
    $('#datepicker-left-header').datetimepicker({
        'format': "YYYY-MM-DD", // HH:mm:ss
    });
}

if (top.location.pathname === '/TempEmployeeMasters/Index') {
    $('.user').addClass('active');
    $(".freezing").freezeTable({
        'columnNum': 2
    });
}

if (top.location.pathname === '/MobileDevice/Index') {
    $(".freezing").freezeTable({
        'columnNum': 2
    });
    $('.mobile').addClass('active');
}

if (top.location.pathname === '/ReasonMasters/Index' || window.location.href.indexOf("/ReasonMasters/Edit") > -1) {
    $('.reason').addClass('active');
}

if (top.location.pathname === '/SiteMaster/Index' || window.location.href.indexOf("/SiteMaster/Edit") > -1) {
    $('.sitemaster').addClass('active');
}

if (top.location.pathname === '/RoleMaster/Index') {
    $('.role').addClass('active');
    $("input[type=checkbox]").on('change', function (e) {
        var status;
        if ($(this).prop("checked") == true) {
            status = true;
        }
        else {
            status = false;
        }
        var id = $(this).parents("tr").find('td:first-child input').val()
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/RoleMaster/ChangeStatus",
            data: { id: id, status: status },
            cache: false,
            success: function (result) {
            },
            error: function (reponse) {

            }
        });
    });
}

if (top.location.pathname === '/SuperMaster/Index') {
    $('.super').addClass('active');
    $("input[type=checkbox]").on('change', function (e) {
        var status;
        if ($(this).prop("checked") == true) {
            status = true;
        }
        else {
            status = false;
        }
        var id = $(this).parents("tr").find('td:first-child input').val()
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SuperMaster/ChangeStatus",
            data: { id: id, status: status },
            cache: false,
            success: function (result) {
            },
            error: function (reponse) {

            }
        });
    });
}

$("body").on("submit", "#Create", function () {
    return confirm("Do you want to submit?");
});

$("body").on("submit", "#Edit", function () {
    return confirm("Do you want to submit?");
});

if (top.location.pathname === '/MonthlyAttendanceReport/Index') {
    $('.monthly-report').addClass('active');
    $(".freezing").freezeTable({
        'columnNum': 2
    });
    $("#export").click(function () {
        $("#export").empty();
        $("#export").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MonthlyAttendanceReport/ExcelReport",
            cache: false,
            success: function (result) {
                $("#export").empty();
                $("#export").append('<i class="fa fa-file-excel-o fa-2x" aria-hidden="true"></i>');
                window.location = '/MonthlyAttendanceReport/Download'
            },
            error: function (reponse) {

            }
        });
    });

    $("#exportpdf").click(function () {
        $("#exportpdf").empty()
        $("#exportpdf").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MonthlyAttendanceReport/ExportPDF",
            cache: false,
            success: function (result) {
                $("#exportpdf").empty();
                $("#exportpdf").append('<i class="fa fa-file-pdf-o fa-2x" aria-hidden="true"></i>');
                window.location = '/MonthlyAttendanceReport/DownloadPdf'
            },
            error: function (reponse) {

            }
        });
    });

    $("#exportcsv").click(function () {
        $("#exportcsv").empty()
        $("#exportcsv").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MonthlyAttendanceReport/ExcelReport",
            cache: false,
            success: function (result) {
                $("#exportcsv").empty()
                $("#exportcsv").append('<i class="fas fa-file-csv fa-2x" aria-hidden="true"></i>');
                window.location = '/MonthlyAttendanceReport/DownloadCsv'
            },
            error: function (reponse) {

            }
        });
    });
}

if (top.location.pathname === '/BannerUploader/BannerUpload') {
    $(document).ready(function (e) {
        if ($.cookie("bannerupload") != "shown") {
            $.cookie("bannerupload", "shown", { expires: 1 });
            $("#instructions").modal('show');
        }
    });

    $('.gallery').addClass('active');
    $('#datepicker-from').datetimepicker({
    });
    $('#datepicker-to').datetimepicker({
    });

    $("#datepicker-from").on("dp.change", function (e) {
        $('#datepicker-to').data("DateTimePicker").minDate(e.date);
    });

    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $("input[type=checkbox]").on('change', function (e) {
        var status;
        if ($(this).prop("checked") == true) {
            status = true;
        }
        else {
            status = false;
        }
        var id = $(this).parents("tr").find('td:first-child input').val()
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/BannerUploader/ChangeStatus",
            data: { id: id, status: status },
            cache: false,
            success: function (result) {
            },
            error: function (reponse) {

            }
        });
    });
}

if (top.location.pathname === '/HRAnnouncement/List') {
    $('.hrann').addClass('active');
    $('#datepicker-from').datetimepicker({
    });
    $('#datepicker-to').datetimepicker({
    });

    $(document).ready(function (e) {
        $('#branch').select2();
        $('#branch').val("");
        if ($.cookie("hrannouncement") != "shown") {
            $.cookie("hrannouncement", "shown", { expires: 1 });
            $("#instructions").modal('show');
        }
    });

    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $("input[type=checkbox]").on('change', function (e) {
        var status;
        if ($(this).prop("checked") == true) {
            status = true;
        }
        else {
            status = false;
        }
        var id = $(this).parents("tr").find('td:first-child input').val()
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/HRAnnouncement/ChangeStatus",
            data: { id: id, status: status },
            cache: false,
            success: function (result) {
            },
            error: function (reponse) {

            }
        });
    });
}

if (window.location.href.indexOf('/PolygonPortal/SiteWisePolygon') > -1) {
    $('.polygon').addClass('active');
    $(document).ready(function (e) {

        $("#tblpolygon tbody").empty()
        var SiteName = $("#site").val();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/PolygonPortal/GetPolygon",
            data: { SiteName: SiteName },
            cache: false,
            success: function (result) {
                var j = 1;
                $.each(result, function (i, list) {
                    if (j > 3) {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value="' + list.Lattitude + '"/></td>'
                            + '<td><input type="number" class="form-control" value="' + list.Longitude + '"/></td>'
                            + '<td><button class="btndelete btn btn-light"><i class="far fa-trash-alt"></i></button></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp);
                    }
                    else {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value="' + list.Lattitude + '"/></td>'
                            + '<td><input type="number" class="form-control" value="' + list.Longitude + '"/></td>'
                            + '<td></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp);
                    }
                    j++;
                });
                var count = $("#tblpolygon tbody tr").length;
                if (count < 3) {
                    for (var k = 1; k <= (3 - count); k++) {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value=""/></td>'
                            + '<td><input type="number" class="form-control" value=""/></td>'
                            + '<td></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp)
                    }
                }
            },
            error: function (reponse) {

            }
        });
    });
    $("#addPolygon").on('click', function (e) {
        var temp = '<tr>'
            + '<td><input type="number" class="form-control" val=""/></td>'
            + '<td><input type="number" class="form-control" val=""/></td>'
            + '<td><button class="btndelete btn btn-light"><i class="far fa-trash-alt"></i></button></td>'
            + '</tr>'
        $("#tblpolygon tbody").append(temp)
    });

    // Delete row on delete button click
    $(document).on("click", ".btndelete", function () {
        $(this).parents("tr").remove();
        $("#btnrooms").removeAttr("disabled");
    });

    $("#SavePloygon").on('click', function (e) {
        var SiteName = $("#site").val();
        var count = $("#tblpolygon tbody tr").length;
        if ($('#officelat').val() == "" || $('#officelat').val() == null || $('#officelong').val() == "" || $('#officelong').val() == null) {
            alert("Enter Office Coordinates")
        }
        else if (count < 3) {
            alert("Please provide Atleast 3 Latitude & Longitude Coordinates for the Polygon Structure Making")
        }
        else if (SiteName == "" || SiteName == null) {
            alert("Please Select a Site Name Before Submitting Latitude and Longitude for Polygon")
        }
        else {
            var count = 0;
            $('#tblpolygon tbody input[type="number"]').each(function () {
                if ($(this).val() == "") {
                    $(this).addClass("red-alert");
                    count++;
                } else {
                    $(this).removeClass("red-alert");
                }
            });

            if (count == 0) {
                var polygonarray = [];
                $("#tblpolygon tbody tr").each(function () {
                    var polygondata = {};
                    polygondata.Latitude = $(this).find('td').eq(0).find('input').val();
                    polygondata.Longitude = $(this).find('td').eq(1).find('input').val();

                    polygonarray.push(polygondata);
                });

                var polygonJSON = JSON.stringify(polygonarray);

                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/PolygonPortal/SavePolygon",
                    data: { polygonJSON: polygonJSON, SiteName: SiteName, officelat: $('#officelat').val(), officelong: $('#officelong').val() },
                    cache: false,
                    success: function (result) {
                        window.location.href = '/PolygonPortal/List';
                    },
                    error: function (reponse) {

                    }
                });
            }
        }

    });

    $("#site").on('change', function (e) {
        $("#tblpolygon tbody").empty()
        var SiteName = $("#site").val();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/PolygonPortal/GetPolygon",
            data: { SiteName: SiteName },
            cache: false,
            success: function (result) {
                var j = 1;
                $.each(result, function (i, list) {
                    if (j > 3) {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value="' + list.Lattitude + '"/></td>'
                            + '<td><input type="number" class="form-control" value="' + list.Longitude + '"/></td>'
                            + '<td><button class="btndelete btn btn-light"><i class="far fa-trash-alt"></i></button></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp);
                    }
                    else {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value="' + list.Lattitude + '"/></td>'
                            + '<td><input type="number" class="form-control" value="' + list.Longitude + '"/></td>'
                            + '<td></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp);
                    }
                    j++;
                });
                var count = $("#tblpolygon tbody tr").length;
                if (count < 3) {
                    for (var k = 1; k <= (3 - count); k++) {
                        var temp = '<tr>'
                            + '<td><input type="number" class="form-control" value=""/></td>'
                            + '<td><input type="number" class="form-control" value=""/></td>'
                            + '<td></td>'
                            + '</tr>'
                        $("#tblpolygon tbody").append(temp)
                    }
                }
            },
            error: function (reponse) {

            }
        });
    });
}

if (top.location.pathname === '/AttendanceReport/Report') {
    if ((window.location.href.indexOf('?page=1') > 0)) {
        $(document).ready(function (e) {
            if ($.cookie("attendancereport") != "shown") {
                $.cookie("attendancereport", "shown", { expires: 1 });
                $("#instructions").modal('show');
            }
        });
    }
    if ((window.location.href.indexOf('?page=') < 0)) {
        $(document).ready(function (e) {
            if ($.cookie("attendancereport") != "shown") {
                $.cookie("attendancereport", "shown", { expires: 1 });
                $("#instructions").modal('show');
            }
        });
    }
}

if (top.location.pathname === '/ApproveRejectAttendance/Attendance') {
    if ((window.location.href.indexOf('?page=1') > 0)) {
        $(document).ready(function (e) {
            if ($.cookie("approverejectattendance") != "shown") {
                $.cookie("approverejectattendance", "shown", { expires: 1 });
                $("#instructions").modal('show');
            }
        });
    }
    if ((window.location.href.indexOf('?page=') < 0)) {
        $(document).ready(function (e) {
            if ($.cookie("approverejectattendance") != "shown") {
                $.cookie("approverejectattendance", "shown", { expires: 1 });
                $("#instructions").modal('show');
            }
        });
    }
}

if (window.location.href.indexOf('/AttendanceReport/Report') > -1) {

    $("#selectallheader").on('click', function (e) {
        $("#tblattendancereport").find('input[type="checkbox"]').each(function () {
            $(this).prop('checked', true);
        });
    });

    $("#deselectallheader").on('click', function (e) {
        $("#tblattendancereport").find('input[type="checkbox"]:checked').each(function () {
            $(this).prop('checked', false);
        });
    });

    $('.report').addClass('active');

    $('#datepicker-from').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#datepicker-to').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}
    });

    $('#datetimepicker-default').datetimepicker();

    // View mode datepicker [shows only years and month]
    $('#datepicker-view-mode').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });

    // Inline datepicker
    $('#datepicker-inline').datetimepicker({
        inline: true
    });


    // Disabled Days of the Week (Disable sunday and saturday) [ 0-Sunday, 1-Monday, 2-Tuesday   3-wednesday 4-Thusday 5-Friday 6-Saturday]
    $('#datepicker-disabled-days').datetimepicker({
        daysOfWeekDisabled: [0, 6]
    });

    // Datepicker in popup
    $('#datepicker-popup-inline').datetimepicker({
        inline: true
    });

    $("[data-header-left='true']").parent().addClass("pmd-navbar-left");

    // Datepicker left header
    $('#datepicker-left-header').datetimepicker({
        'format': "YYYY-MM-DD", // HH:mm:ss
    });

    $("#datepicker-from").on("dp.change", function (e) {
        $('#datepicker-to').data("DateTimePicker").minDate(e.date);
    });

    $("#export").click(function () {
        var attendancestatus = $("#attendancestatus option:selected").val();
        var branch = $("#branch option:selected").val();
        var emp = $("#emp option:selected").val();
        var head = $("#head option:selected").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();

        var ColumnName = "";
        $("#tblattendancereport").find('input[type="checkbox"]:checked').each(function () {
            ColumnName += $(this).val() + ","
        });
        ColumnName = ColumnName.slice(0, -1)

        $("#export").empty();
        $("#export").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/AttendanceReport/ExcelReport",
            data: { attendancestatus: attendancestatus, branch: branch, emp: emp, head: head, fromdate: fromdate, todate: todate, ColumnName: ColumnName },
            cache: false,
            success: function (result) {
                $("#export").empty();
                $("#export").append('<i class="fa fa-file-excel-o fa-2x" aria-hidden="true"></i>');
                window.location = '/AttendanceReport/Download'
            },
            error: function (reponse) {

            }
        });
    });

    $("#exportpdf").click(function () {
        var attendancestatus = $("#attendancestatus option:selected").val();
        var branch = $("#branch option:selected").val();
        var emp = $("#emp option:selected").val();
        var head = $("#head option:selected").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();

        var ColumnName = "";
        $("#tblattendancereport").find('input[type="checkbox"]:checked').each(function () {
            ColumnName += $(this).val() + ","
        });
        ColumnName = ColumnName.slice(0, -1)

        $("#exportpdf").empty()
        $("#exportpdf").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/AttendanceReport/ExportPDF",
            data: { attendancestatus: attendancestatus, branch: branch, emp: emp, head: head, fromdate: fromdate, todate: todate, ColumnName: ColumnName },
            cache: false,
            success: function (result) {
                $("#exportpdf").empty();
                $("#exportpdf").append('<i class="fa fa-file-pdf-o fa-2x" aria-hidden="true"></i>');
                window.location = '/AttendanceReport/DownloadPdf'
            },
            error: function (reponse) {

            }
        });
    });

    $("#exportcsv").click(function () {
        var attendancestatus = $("#attendancestatus option:selected").val();
        var branch = $("#branch option:selected").val();
        var emp = $("#emp option:selected").val();
        var head = $("#head option:selected").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();

        var ColumnName = "";
        $("#tblattendancereport").find('input[type="checkbox"]:checked').each(function () {
            ColumnName += $(this).val() + ","
        });
        ColumnName = ColumnName.slice(0, -1)

        $("#exportcsv").empty()
        $("#exportcsv").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/AttendanceReport/ExcelReport",
            data: { attendancestatus: attendancestatus, branch: branch, emp: emp, head: head, fromdate: fromdate, todate: todate, ColumnName: ColumnName },
            cache: false,
            success: function (result) {
                $("#exportcsv").empty()
                $("#exportcsv").append('<i class="fas fa-file-csv fa-2x" aria-hidden="true"></i>');
                window.location = '/AttendanceReport/DownloadCsv'
            },
            error: function (reponse) {

            }
        });
    });
}

if (top.location.pathname === '/DashBoard/DashBoard') {

    $(document).ready(function (e) {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChartBranchAbsent);
        google.charts.setOnLoadCallback(drawPie);
    });

    function drawChartBranchAbsent() {
        var location = $("#branch").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Status');
        data.addColumn('number', 'Count');

        $.ajax({
            type: "GET",
            dataType: "json",
            url: "/DashBoard/DashBoardAbsentSiteWise",
            cache: false,
            data: { location: location, fromdate: fromdate, todate: todate },
            success: function (result) {
                $("#absent").text("0");
                $("#present").text("0");
                $("#pending").text("0");
                $.each(result, function (i, list) {
                    data.addRow([list.Status, list.Count]);
                    if (list.Status == "Absent") {
                        $("#absent").text(list.Count);
                    }
                    if (list.Status == "Present") {
                        $("#present").text(list.Count);
                    }
                    if (list.Status == "Pending") {
                        $("#pending").text(list.Count);
                    }
                });
                var options = {
                    title: 'Status Wise Pictorial Representation',
                    height: 320
                };
                var chart = new google.visualization.ColumnChart(document.getElementById('columnchart'));
                $("#columnchart").find('span').remove()
                chart.draw(data, options);
            },
            error: function (reponse) {

            }
        });
    }

    function drawPie() {
        var location = $("#branch").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Status');
        data.addColumn('number', 'Count');

        $.ajax({
            type: "GET",
            dataType: "json",
            url: "/DashBoard/DashBoardAbsentSiteWise",
            cache: false,
            data: { location: location, fromdate: fromdate, todate: todate },
            success: function (result) {
                $("#absent").text("0");
                $("#present").text("0");
                $("#pending").text("0");
                $.each(result, function (i, list) {
                    data.addRow([list.Status, list.Count]);
                    if (list.Status == "Absent") {
                        $("#absent").text(list.Count);
                    }
                    if (list.Status == "Present") {
                        $("#present").text(list.Count);
                    }
                    if (list.Status == "Pending") {
                        $("#pending").text(list.Count);
                    }
                });
                var options = {
                    title: 'Status Wise Pictorial Representation',
                    height: 320
                };
                var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                $("#columnchart").find('span').remove()
                chart.draw(data, options);
            },
            error: function (reponse) {

            }
        });
    }

    $("#filter").on('click', function (e) {
        var location = $("#branch").val();
        var fromdate = $("#datepicker-from").val();
        var todate = $("#datepicker-to").val();
        $("#columnchart").empty();
        $("#columnchart").append('<span style="margin-left: 30%; font-size: 20px;">  Chart Will be Loaded Here </span>');
        $("#piechart").empty();
        $("#piechart").append('<span style="margin-left: 30%; font-size: 20px;">  Chart Will be Loaded Here </span>');

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChartBranchAbsent);
        google.charts.setOnLoadCallback(drawPie);
    });
}

if (top.location.pathname === '/ApproveRejectAttendance/Attendance') {
    $('#datepicker-from').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}

    });
    $('#datepicker-to').datetimepicker({

        'format': "YYYY-MM-DD", // HH:mm:ss}
    });

    $('#datetimepicker-default').datetimepicker();

    // View mode datepicker [shows only years and month]
    $('#datepicker-view-mode').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });

    // Inline datepicker
    $('#datepicker-inline').datetimepicker({
        inline: true
    });


    // Disabled Days of the Week (Disable sunday and saturday) [ 0-Sunday, 1-Monday, 2-Tuesday   3-wednesday 4-Thusday 5-Friday 6-Saturday]
    $('#datepicker-disabled-days').datetimepicker({
        daysOfWeekDisabled: [0, 6]
    });

    // Datepicker in popup
    $('#datepicker-popup-inline').datetimepicker({
        inline: true
    });

    $("[data-header-left='true']").parent().addClass("pmd-navbar-left");

    // Datepicker left header
    $('#datepicker-left-header').datetimepicker({
        'format': "YYYY-MM-DD", // HH:mm:ss
    });

    $("#datepicker-from").on("dp.change", function (e) {
        $('#datepicker-to').data("DateTimePicker").minDate(e.date);
    });

    $('.approvereject').addClass('active');
    $(document).ready(function (e) {
        $("#selectall").on('click', function (e) {
            if ($("#selectall").val() == "Select All") {
                $("#tblapprovereject tbody tr").each(function () {
                    var input = $(this).find('td').eq(0).find('input');
                    $(input).prop('checked', true)
                });
                $("#selectall").val("UnSelect All");
            }
            else {
                $("#tblapprovereject tbody tr").each(function () {
                    var input = $(this).find('td').eq(0).find('input');
                    $(input).prop('checked', false)
                });
                $("#selectall").val("Select All");
            }
        });

        $("#approveall").on('click', function (e) {
            var Status = "Approved";
            var attendanceId = [];
            var Reason = "Approved By HR";
            $('#tblapprovereject').find('input[type="checkbox"]:checked').each(function () {
                attendanceId.push($(this).val())
            });
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/ApproveRejectAttendance/ApproveRejectAll",
                data: { attendanceId: attendanceId, Status: Status, Reason: Reason },
                cache: false,
                success: function (result) {
                    alert("Attendance Approved Successfully")
                    location.reload(true);
                },
                error: function (reponse) {

                }
            });
        });

        $("#rejectall").on('click', function (e) {
            $("#rejectreason").modal('show');

        });

        $("#modalOk").on('click', function (e) {
            var Reason = [];
            $('#rejectreasons input:checked').each(function () {
                Reason.push($(this).attr('name'));
            });
            if (Reason.length > 0) {
                var Status = "Rejected";
                var attendanceId = [];
                $('#tblapprovereject').find('input[type="checkbox"]:checked').each(function () {
                    attendanceId.push($(this).val())
                });
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/ApproveRejectAttendance/ApproveRejectAll",
                    data: { attendanceId: attendanceId, Status: Status, Reason: Reason.toString() },
                    cache: false,
                    success: function (result) {
                        alert("Attendance Rejected Successfully")
                        location.reload(true);
                    },
                    error: function (reponse) {

                    }
                });
            }
        });
    });
}



if (top.location.pathname === '/MobileDevice/Index') {
    $('.monthly-report').addClass('active');
    $(".freezing").freezeTable({
        'columnNum': 2
    });
    $("#export").click(function () {
        $("#export").empty();
        $("#export").append('Downloading..');
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/MobileDevice/ExcelReport",
            cache: false,
            success: function (result) {
                $("#export").empty();
                $("#export").append('<i class="fa fa-file-excel-o fa-2x" aria-hidden="true"></i>');
                window.location = '/MobileDevice/Download'
            },
            error: function (reponse) {

            }
        });
    });




}
