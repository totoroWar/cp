(function ($)
{
    $.fn.set_lpc_select = function(pid,cid,sid)
    {
        var _location_where = new Array(31);
        function _location_comefrom(loca, locacity) { this.loca = loca; this.locacity = locacity; }
        _location_where[0] = new _location_comefrom("请选择省份名", "请选择城市名");
        _location_where[1] = new _location_comefrom("北京", "东城|西城|崇文|宣武|朝阳|丰台|石景山|海淀|门头沟|房山|通州|顺义|昌平|大兴|平谷|怀柔|密云|延庆");
        _location_where[2] = new _location_comefrom("上海", "黄浦|卢湾|徐汇|长宁|静安|普陀|闸北|虹口|杨浦|闵行|宝山|嘉定|浦东|金山|松江|青浦|南汇|奉贤|崇明");
        _location_where[3] = new _location_comefrom("天津", "和平|东丽|河东|西青|河西|津南|南开|北辰|河北|武清|红挢|塘沽|汉沽|大港|宁河|静海|宝坻|蓟县");
        _location_where[4] = new _location_comefrom("重庆", "万州|涪陵|渝中|大渡口|江北|沙坪坝|九龙坡|南岸|北碚|万盛|双挢|渝北|巴南|黔江|长寿|綦江|潼南|铜梁|大足|荣昌|壁山|梁平|城口|丰都|垫江|武隆|忠县|开县|云阳|奉节|巫山|巫溪|石柱|秀山|酉阳|彭水|江津|合川|永川|南川");
        _location_where[5] = new _location_comefrom("河北", "石家庄|邯郸|邢台|保定|张家口|承德|廊坊|唐山|秦皇岛|沧州|衡水");
        _location_where[6] = new _location_comefrom("山西", "太原|大同|阳泉|长治|晋城|朔州|吕梁|忻州|晋中|临汾|运城");
        _location_where[7] = new _location_comefrom("内蒙古", "呼和浩特|包头|乌海|赤峰|呼伦贝尔盟|阿拉善盟|哲里木盟|兴安盟|乌兰察布盟|锡林郭勒盟|巴彦淖尔盟|伊克昭盟");
        _location_where[8] = new _location_comefrom("辽宁", "沈阳|大连|鞍山|抚顺|本溪|丹东|锦州|营口|阜新|辽阳|盘锦|铁岭|朝阳|葫芦岛");
        _location_where[9] = new _location_comefrom("吉林", "长春|吉林|四平|辽源|通化|白山|松原|白城|延边");
        _location_where[10] = new _location_comefrom("黑龙江", "哈尔滨|齐齐哈尔|牡丹江|佳木斯|大庆|绥化|鹤岗|鸡西|黑河|双鸭山|伊春|七台河|大兴安岭");
        _location_where[11] = new _location_comefrom("江苏", "南京|镇江|苏州|南通|扬州|盐城|徐州|连云港|常州|无锡|宿迁|泰州|淮安");
        _location_where[12] = new _location_comefrom("浙江", "杭州|宁波|温州|嘉兴|湖州|绍兴|金华|衢州|舟山|台州|丽水");
        _location_where[13] = new _location_comefrom("安徽", "合肥|芜湖|蚌埠|马鞍山|淮北|铜陵|安庆|黄山|滁州|宿州|池州|淮南|巢湖|阜阳|六安|宣城|亳州");
        _location_where[14] = new _location_comefrom("福建", "福州|厦门|莆田|三明|泉州|漳州|南平|龙岩|宁德");
        _location_where[15] = new _location_comefrom("江西", "南昌市|景德镇|九江|鹰潭|萍乡|新馀|赣州|吉安|宜春|抚州|上饶");
        _location_where[16] = new _location_comefrom("山东", "济南|青岛|淄博|枣庄|东营|烟台|潍坊|济宁|泰安|威海|日照|莱芜|临沂|德州|聊城|滨州|菏泽");
        _location_where[17] = new _location_comefrom("河南", "郑州|开封|洛阳|平顶山|安阳|鹤壁|新乡|焦作|濮阳|许昌|漯河|三门峡|南阳|商丘|信阳|周口|驻马店|济源");
        _location_where[18] = new _location_comefrom("湖北", "武汉|宜昌|荆州|襄樊|黄石|荆门|黄冈|十堰|恩施|潜江|天门|仙桃|随州|咸宁|孝感|鄂州");
        _location_where[19] = new _location_comefrom("湖南", "长沙|常德|株洲|湘潭|衡阳|岳阳|邵阳|益阳|娄底|怀化|郴州|永州|湘西|张家界");
        _location_where[20] = new _location_comefrom("广东", "广州|深圳|珠海|汕头|东莞|中山|佛山|韶关|江门|湛江|茂名|肇庆|惠州|梅州|汕尾|河源|阳江|清远|潮州|揭阳|云浮");
        _location_where[21] = new _location_comefrom("广西", "南宁|柳州|桂林|梧州|北海|防城港|钦州|贵港|玉林|南宁地区|柳州地区|贺州|百色|河池");
        _location_where[22] = new _location_comefrom("海南", "海口|三亚");
        _location_where[23] = new _location_comefrom("四川", "成都|绵阳|德阳|自贡|攀枝花|广元|内江|乐山|南充|宜宾|广安|达川|雅安|眉山|甘孜|凉山|泸州");
        _location_where[24] = new _location_comefrom("贵州", "贵阳|六盘水|遵义|安顺|铜仁|黔西南|毕节|黔东南|黔南");
        _location_where[25] = new _location_comefrom("云南", "昆明|大理|曲靖|玉溪|昭通|楚雄|红河|文山|思茅|西双版纳|保山|德宏|丽江|怒江|迪庆|临沧");
        _location_where[26] = new _location_comefrom("西藏", "拉萨|日喀则|山南|林芝|昌都|阿里|那曲");
        _location_where[27] = new _location_comefrom("陕西", "西安|宝鸡|咸阳|铜川|渭南|延安|榆林|汉中|安康|商洛");
        _location_where[28] = new _location_comefrom("甘肃", "兰州|嘉峪关|金昌|白银|天水|酒泉|张掖|武威|定西|陇南|平凉|庆阳|临夏|甘南");
        _location_where[29] = new _location_comefrom("宁夏", "银川|石嘴山|吴忠|固原");
        _location_where[30] = new _location_comefrom("青海", "西宁|海东|海南|海北|黄南|玉树|果洛|海西");
        _location_where[31] = new _location_comefrom("新疆", "乌鲁木齐|石河子|克拉玛依|伊犁|巴音郭勒|昌吉|克孜勒苏柯尔克孜|博尔塔拉|吐鲁番|哈密|喀什|和田|阿克苏");
        //_location_where[32] = new _location_comefrom("香港", "");
        //_location_where[33] = new _location_comefrom("澳门", "");
        //_location_where[34] = new _location_comefrom("台湾", "台北|高雄|台中|台南|屏东|南投|云林|新竹|彰化|苗栗|嘉义|花莲|桃园|宜兰|基隆|台东|金门|马祖|澎湖");
        ///_location_where[35] = new _location_comefrom("其它", "北美洲|南美洲|亚洲|非洲|欧洲|大洋洲");
        function set_content(content)
        {
            $("#" + sid).val(content);
        }

        for (var i = 0; i < _location_where.length; i++)
        {
            var p = _location_where[i].loca;
            var c = _location_where[i].locacity.split("|");
            $("#" + pid).append("<option value='" + i + "'>" + p + "</option>");
            if (0 == i)
            {
                $("#" + cid).append("<option value='0'>" + c + "</option>");
            }
        }
        $("#" + pid).change(function ()
        {
            var index = $(this).find("option:selected").val();
            var c = _location_where[index].locacity.split("|");
            $("#" + cid).empty();
            if (Object.prototype.toString.call(c) == "[object Array]")
            {
                for (var i = 0; i < c.length; i++)
                {
                    $("#" + cid).append("<option value='" + c[i] + "'>" + c[i] + "</option>");
                }
            }
            else
            {
                $("#" + cid).append("<option value='0'>" + c + "</option>");
            }
            set_content(_location_where[index].loca + $("#" + cid).find("option:selected").text());
        });
        $("#" + cid).change(function ()
        {
            var index = $("#"+pid).find("option:selected").val();
            set_content(_location_where[index].loca + $("#" + cid).find("option:selected").text());
        });
    }
})(jQuery);