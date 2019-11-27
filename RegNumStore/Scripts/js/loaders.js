var GlobalFlag = 0;

$(window).resize(function () {
    MoveOn();
});

$(document).ready(function () {
    setTimeout(function () {
        $('input:checkbox').trigger('refresh');
    }, 1);
    MoveOn();
    setTimeout(function () {
        $('input:checkbox').trigger('refresh');
    }, 1);
});

function MoveOn() {
    $(".jq-selectbox__dropdown ul").css("margin-left", "0");
    var reg = $("#reg-num");
    if ($(window).width() < 750) {
        reg.detach();
        reg.appendTo(".mobile-menu ul li:first");
        $("#reg-num li").css("text-align", "left");
        $(".jq-checkbox__div").attr('style', $(".jq-checkbox__div").attr('style') + '; ' + 'margin-left:2px!important;');
            
        for (var i = 1; i < 7; i++) {
            $("#Num" + i + "-styler").attr('style', $("#Num" + i + "-styler").attr('style') + '; ' + 'left: 5px !important; text-align: center');
        }
        $("#Num7-styler").attr('style', $("#Num7-styler").attr('style') + '; ' + 'left: 13px !important; text-align: center');

    } else {
        reg.detach();
        reg.appendTo(".wrapper ul:first");
        $(".jq-checkbox__div").attr('style', $(".jq-checkbox__div").attr('style') + '; ' + 'margin-left:7px!important;');
        for (var j = 1; j < 7; j++) {
            $("#Num" + j + "-styler").attr('style', $("#Num"+j+"-styler").attr('style') + '; ' + 'left: -3px !important');
        }
        $("#Num7-styler").attr('style', $("#Num7-styler").attr('style') + '; ' + 'left: 4px !important');
    }
}

var min = parseInt($("#minCost").val());
var max = parseInt($("#maxCost").val());

//console.log("min: " + minAlt + " max: " + maxAlt);
$("#slider").slider({
    min: min,
    max: max,
    //values: [min, max],
    values: [min, max],
    range: true,
    stop: function (event, ui) {
        $("input#minCost").val($("#slider").slider("values", 0));
        $("input#maxCost").val($("#slider").slider("values", 1));
    },
    slide: function (event, ui) {
        GlobalFlag = 1;
        $("input#minCost").val($("#slider").slider("values", 0));
        $("input#maxCost").val($("#slider").slider("values", 1));
    }
});





$("input#minCost").change(function () {
    GlobalFlag = 1;
    var value1 = $("input#minCost").val();
    var value2 = $("input#maxCost").val();

    if (parseInt(value1) > parseInt(value2)) {
        value1 = value2;
        $("input#minCost").val(value1);
    }
    jQuery("#slider").slider("values", 0, value1);
});


$("input#maxCost").change(function () {
    GlobalFlag = 1;
    var value1 = $("input#minCost").val();
    var value2 = $("input#maxCost").val();

    if (value2 > max) {
        value2 = max;
        $("input#maxCost").val(max);
    }

    if (parseInt(value1) > parseInt(value2)) {
        value2 = value1;
        $("input#maxCost").val(value2);
    }
    $("#slider").slider("values", 1, value2);
});

var minAlt = $("#minCost").attr('alt');
var maxAlt = $("#maxCost").attr('alt');

if (minAlt==" ") { } else {
    $("input#minCost").val(minAlt);
    $("input#minCost").change();
}

if (maxAlt==" ") {
}else{
$("input#maxCost").val(maxAlt);
    $("input#maxCost").change();
}





$(".menu-checks b").click(function (event) {
    $(this).parent().find("input").trigger('click');
});

//--------------------------------
var BlockNumber = 2;  //Infinate Scroll starts from second block
var NoMoreData = false;
var inProgress = false;
    
function cats() {
    var checkses = $("input.checks:checkbox:checked");
    var check_ch = checkses.length;
    console.log("Count: " + checkses.length);
    var num = new Array(check_ch);
    if (check_ch > 0) {
        var j = 0;
        checkses.each(function () {
            //console.log($(this).attr('id'));
            num[j] = $(this).attr('id');
            j++;
        });
    }
    return num;
}

function carNumFunc() {
    return carNumber =
    {
        Num1: $("#Num1").val(),
        Num2: $("#Num2").val(),
        Num3: $("#Num3").val(),
        Num4: $("#Num4").val(),
        Num5: $("#Num5").val(),
        Num6: $("#Num6").val(),
        Num7: $("#Num7").val()
    };

}

function updateData() {
    //console.log(GlobalFlag);

    setTimeout(function () {
        $('input:checkbox').trigger('refresh');
    }, 1);

    var num = cats();
    var carNumber = carNumFunc();

    var minCost = $("input#minCost").val();
    var maxCost = $("input#maxCost").val();
    $("#loadingDiv").show();

    $.post("/Home/GetNumAttributeList", { "carNumber": JSON.stringify(carNumFunc()), "category2": JSON.stringify(num), minCost: minCost, maxCost: maxCost },
        function (data) {
            //var mn = $.toJSON(data);

            for (var i = 0; i < 7; i++) {
                for (var j = 0; j < data[i].length; j++) {
                    //console.log(data[i][j].toArray);
                    console.log(JSON.stringify(num));

                    if (j == 0) {
                        var m1 = $("select#Num" + (i + 1) + " option:selected").text();
                        $("select#Num" + (i + 1)).empty();

                        $("select#Num" + (i + 1)).append("<option>" + m1 + "</option>");
                        if (m1 != "*") {
                            $("select#Num" + (i + 1)).append("<option>*</option>");
                        }
                    }

                    if ($(".jq-selectbox__dropdown ul").length == 0) {

                        $("select#Num" + (i + 1)).append("<option>" + data[i][j] + "</option>");

                    } else {

                        $("select#Num" + (i + 1)).append("<option>" + data[i][j] + "</option>");
                        $("select#Num" + (i + 1)).trigger('refresh');
                    }
                }
            }
            success:
                {
                    $('input, select').styler();
                    MoveOn();

                    $("#loadingPreDiv").hide();
                    //$("#loadingDiv").hide();

                    setTimeout($(".wrapper").css('visibility', 'visible'), 10);
                }
        });

    if (carNumber.Num1 + carNumber.Num2 + carNumber.Num3 + carNumber.Num4 + carNumber.Num5 + carNumber.Num6 != '******' || GlobalFlag != 0) {
        $("#revs-n").hide();
        $.post("/Home/Portfolio", { "page": 1, "ajax": true, "carNumber": JSON.stringify(carNumber), "category2": JSON.stringify(num), minCost: minCost, maxCost: maxCost },
            function (data) {
                BlockNumber = 2;
                NoMoreData = false;
                inProgress = false;

                $("#revs-n").empty();
                $("#revs-n").append(data.HTMLString);
                $("#loadingDiv").hide();
                inProgress = false;
                success: {
                    $("#revs-n").show();
                }
                $('#revs-n').freetile({
                    animate: true,
                    elementDelay: 30,
                    selector: '.portfolio-modal, .rev-n'
                });

            });
    }
}

$(document).ready(function () {
    
    updateData();
    //---------------------------------
        
    $('#slider').slider({ change: function () { updateData() } });
        
    $("#Num1, #Num2, #Num3, #Num4, #Num5, #Num6, #Num7, .checks, input#minCost, input#minCost").change(function () {
        //$("#loadingPreDiv").show();
        //$("#revs-n").hide();
        GlobalFlag = 1;
        updateData();
            
    });
        
    //--------------------------------
    $(window).scroll(function () {
        if ($(window).scrollTop() >= $(document).height() - $(window).height() - 10 && !NoMoreData && !inProgress) {

            inProgress = true;
            $("#loadingDiv").show();


            var carNumber = carNumFunc();
            var num = cats();
            //var category = "";
           // var p = $(".navigation-portfolio li.active a").attr('href');
            //if (p != undefined) {
            //    category = p.substring(p.lastIndexOf("/") + 1);
            //}

            var minCost = $("input#minCost").val();
            var maxCost = $("input#maxCost").val();
            $.post("/Home/Portfolio", {"page": BlockNumber, "ajax": true, "carNumber": JSON.stringify(carNumber), "category2": JSON.stringify(num), minCost: minCost, maxCost: maxCost },
                function (data) {
                    BlockNumber = BlockNumber + 1;
                    NoMoreData = data.NoMoreData;
                    $("#revs-n").append(data.HTMLString);
                    $("#loadingDiv").hide();
                    inProgress = false;

                    //console.log(JSON.stringify(num));
                    //$('a[data-rel^=lightcase]').lightcase({
                    //    swipe: true,
                    //    showTitle: false,
                    //    showCaption: false,
                    //    showSequenceInfo: false,
                    //    closeOnOverlayClick: false,
                    //    transition: 'scrollHorizontal',
                    //    maxWidth: 1200,
                    //    maxHeight: 800,
                    //});

                    $('#revs-n').freetile({
                        animate: true,
                        elementDelay: 30,
                        selector: '.portfolio-modal, .rev-num'
                    });

                });
        }
    });

   
        
 

      
});
    
   
//--------------------------------
    
   

var menu = $('.navigation-portfolio ul');
var a = menu.find('li');


a.hover(function() {
    var t = $(this);
    var s = $(this).siblings();
    t.toggleClass('shadow');
    s.toggleClass('blur');

    

});


//----------------------


$(".rev-n").click(function () {
    var url = $(this).find("a").attr('href');
    console.log(url);
    //console.log(url.parent().html());
    document.location.href = url;
     // trigger('click');
});



                
             