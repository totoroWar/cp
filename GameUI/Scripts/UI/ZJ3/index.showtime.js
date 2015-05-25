gameid = 5;
classid = 1
endtime = new Date();
opentime=new Date();
isopen = false;
var ShowGameTimeInfo = function () {
   

    this.games = [
      { gamename: "重庆时时彩", classid: 1, gameid: 4 },
      { gamename: "天津时时彩", classid: 1, gameid: 5 },
      { gamename: "江西时时彩", classid: 1, gameid: 7 },
      { gamename: "新疆时时彩", classid: 1, gameid: 6 },
      { gamename: "分分彩", classid: 1, gameid: 24 },
      { gamename: "三分彩", classid: 1, gameid: 25 },
      { gamename: "福彩3D", classid: 5, gameid: 20 },
      { gamename: "上海时时乐", classid: 5, gameid: 23 },
      { gamename: "体彩排列3", classid: 5, gameid: 26 },
      { gamename: "高频3D", classid: 5, gameid: 28 },
      { gamename: "广东11选5", classid: 3, gameid: 1 },
      { gamename: "重庆11选5", classid: 3, gameid: 14 },
      { gamename: "11夺运金", classid: 3, gameid: 19 },
      { gamename: "江西多乐彩", classid: 3, gameid: 22 },
      { gamename: "高频11选5", classid: 3, gameid: 27 }
    ];
    this.init = function (gameindex) {
        gameid = this.games[gameindex].gameid;
        classid = this.games[gameindex].classid;
    }

    this.ShowInfo = function () {
        var p = $(this).parent();
        jQuery.ajax({
            type: "post",
            async: true,
            url: "/ui2/ShowPlayTime",
            data: "{gameID:" + gameid + ",gameClassID:" + classid + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (json) {
                if (json.code == 1) {
                    $("#issue").text(json.issue);
                    json.endtime.replace(/-/g, "/")
                    endtime = new Date(json.endtime.replace(/-/g, "/"));
                    opentime = new Date(json.opentime.replace(/-/g, "/"));
                    isopen = true;
                    $(".tz_time_box h4").css("display", "block");
                }
                else {
                    isopen = false;
                    $("#issue").text("00000000");
                    var cc = document.getElementById("count_down"); 
                    cc.innerHTML = json.msg;
                    $(".tz_time_box h4").css("display", "none");
                }
            },
            error: function (err) {
               // alert(err);
            }
        });
    }

    this.ShowCountDown = function () {

        var now = new Date();
        var endDate = endtime;
        
        var leftTime = endDate.getTime() - now.getTime();

        var leftsecond = parseInt(leftTime / 1000);
        var day1 = Math.floor(leftsecond / (60 * 60 * 24));
        var hour = Math.floor((leftsecond - day1 * 24 * 60 * 60) / 3600);
        var minute = Math.floor((leftsecond - day1 * 24 * 60 * 60 - hour * 3600) / 60);
        var second = Math.floor(leftsecond - day1 * 24 * 60 * 60 - hour * 3600 - minute * 60);
     
        if (now >= endtime) {
            if (second % 5 == 0) {
                this.ShowInfo();
            }
        }
        if (!isopen)
            return;
        var cc = document.getElementById("count_down");
        if (leftTime < 0) {
            cc.innerHTML = "还剩" + 0 + "天" + 0 + "小时" + 0 + "分" + 0 + "秒";
        }
        else{
            cc.innerHTML = "还剩" + day1 + "天" + hour + "小时" + minute + "分" + second + "秒";
        }
    }


    

}



var gameindex = 1;
var showgametime = new ShowGameTimeInfo();
$(function () {

    showgametime.init(gameindex);
    showgametime.ShowInfo();
    showgametime.ShowCountDown();
    $(".sNext").click(function () {
        gameindex++;
        if (gameindex > 14)
            gameindex = 0;
        showgametime.init(gameindex);
        showgametime.ShowInfo();
        showgametime.ShowCountDown();
        setlink();
    })
    $(".sPrev").click(function () {
        gameindex--
        if (gameindex < 0) {
            gameindex = 14;
        }
            showgametime.init(gameindex);
            showgametime.ShowInfo();
            showgametime.ShowCountDown();
            setlink();
    })

    setlink();
    window.setInterval(function () { showgametime.ShowCountDown(); }, 1000);
    
    //setTimeout("xShowCountDown()", 1000);
});
function setlink() {
    $(".m_touzhu_xd a").attr("href", "/Game_" + gameid + "_" + classid + ".html");
}