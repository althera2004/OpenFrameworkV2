function OptionRenderLevel1(option) {
    var active = "";
    if (option.Id === MenuOptionId) {
        var active = " class=\"active\"";
    }
	
    if (typeof option.Item !== "undefined" && option.Item !== null) {
        return "<li" + active + "><a data-link=\"" + option.Item + "\" data-listId=\"" + option.ListId + "\" id=\"" + option.Id + "\" class=\"ownerLink\">" + option.Label + "</a></li>";
    }

    return "<li" + active + "><a href=\"" + option.Url + "\" id=\"" + option.Id + "\">" + option.Label + "</a></li>";
}

function OptionRenderLevel0(option) {
    var active = "";
    if (option.Id === MenuOptionId) {
        var active = " class=\"active\"";
    }
	
	var res = "";
	if (typeof option.Item !== "undefined" && option.Item !== null) {
        res = "<li" + active + "><a data-link=\"" + option.Item + "\" data-listId=\"" + option.ListId + "\" id=\"" + option.Id + "\" class=\"ownerLink\">" + option.Label + "</a></li>";
    }
	else {
		res = "<li" + active + "><a href=\"" + option.Url + "\" id=\"" + option.Id + "\">"+ option.Label+"</a></li>";
	}
	
    if (typeof option.Remark !== "undefined" && option.Remark !== null) {
        res += "<span class=\"label label-" + option.Remark.Type + " pull-right\">" + option.Remark.Label + "</span>";
    }
    res += "</span></a></li>";
    return res;
}

function SubmenuRender(submenu) {
    var expanded = SubmenuContainsActual(submenu);
    res = "<li" + (expanded === true ? " class=\"active\"" : "") + ">";
    res += "<a href=\"#\" aria-expanded=\"true\"><span class=\"nav-label\">" + submenu.Label + "</span><span class=\"fa arrow\"></span> </a>";


    if (expanded === true) {
        res += "<ul class=\"nav nav-second-level collapse-in\" aria-expanded=\"true\">";
    }
    else {
        res += "<ul class=\"nav nav-second-level collapse\" aria-expanded=\"false\" style=\"height: 0px;\">";
    }

    for (var x = 0; x < submenu.Options.length; x++) {
        res += OptionRenderLevel1(submenu.Options[x]);
    }

    res += "</ul></li>";
    return res;
}

function SubmenuContainsActual(submenu) {
    for (var x = 0; x < submenu.Options.length; x++) {
        if (submenu.Options[x].Id === MenuOptionId) {
            return true;
        }
    }

    return false;
}


function MenuRender(menu) {
    var res = "";
    for (var x = 0; x < menu.length; x++) {
        var actualItem = menu[x];
        if (actualItem.Leaf === true) {
            res += OptionRenderLevel0(actualItem);
        }
        else {
            res += SubmenuRender(actualItem);
        }
    }

    return res;
}