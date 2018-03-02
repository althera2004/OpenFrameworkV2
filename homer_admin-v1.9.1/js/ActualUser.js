function LayoutMenuRender(user) {
    res =   "<div class=\"profile-picture\">";
    res +=  "   <a href=\"index.html\">" ;
    res += "       <img src=\"images/profiles/" + user.Id + ".png\" class=\"img-circle m-b\" alt=\"logo\" style=\"width:45px;height:45px;\">";
    res +=  "   </a>";
    res +=  "   <div class=\"stats-label text-color\">";
    res += "       <span class=\"font-extra-bold font-uppercase\">" + user.FullName + "</span>";
    res +=  "       <div class=\"dropdown\">";
    res +=  "           <a class=\"dropdown-toggle\" href=\"#\" data-toggle=\"dropdown\">";
    res += "               <small class=\"text-muted\">" + user.JobPosition + " <strong class=\"caret\"></strong></small>";
    res +=  "           </a>";
    res +=  "           <ul class=\"dropdown-menu animated flipInX m-t-xs\">";
    res +=  "               <li><a href=\"contacts.html\">Contacts</a></li>";
    res +=  "               <li><a href=\"profile.html\">Profile</a></li>";
    res +=  "               <li><a href=\"/analytics.html\">Analytics</a></li>";
    res +=  "               <li class=\"divider\"></li>";
    res +=  "               <li><a href=\"/login.html\">Logout</a></li>";
    res +=  "           </ul>";
    res +=  "       </div>";
    res +=  "   </div>";
    res += "</div>";
    return res;
}