function _global_set_num_img(num)
{
    if (num == ".") {
        return '<img src="/Images/Common/_num_dot2.png" border="0" align="absbottom" />';
    }
    else if (num == ",") {
        return '<img src="/Images/Common/_num_dot.png" border="0" align="absbottom" />';
    }
    return '<img src="/Images/Common/_num_' + num + '.png" border="0" align="absbottom" />';
}

function _global_set_img_money(money)
{
    var result = "";
    for (var i = 0; i < money.length; i++)
    {
        result += _global_set_num_img(money.substring(i, i+1));
    }
    return result;
}