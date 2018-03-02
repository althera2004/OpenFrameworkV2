function ATTACHMENT_Render(attachment) {
    var description = "<i>Sin descripción</i>";
    if (typeof attachment.Description !== "undefined" && attachment.Description !== null && attachment.Description !== "") {
        description = attachment.Description;
    }
    var res = "<div class=\"col-lg-4\" style=\"\">";
    res += "    <div class=\"hpanel hgreen contact-panel\">";
    res += "        <div class=\"panel-body\">";
    res += "            <img alt=\"logo\" class=\"imgprofile m-b\" src=\"/images/FileIcons/" + attachment.FileExtension + ".png\">";
    res += "            <span class=\"label pull-right\"><button class=\"btn btn-danger btn-circle\" type=\"button\"><i class=\"fa fa-trash\"></i></button></span>";
    res += "            <span class=\"label pull-right\"><button class=\"btn btn-info btn-circle\" type=\"button\"><i class=\"fa fa-download\"></i></button></span>";
    res += "            <span class=\"label pull-right\"><button class=\"btn btn-info btn-circle\" type=\"button\"><i class=\"fa fa-eye\"></i></button></span>";
    res += "            <h3><a href=\"#\">"+ attachment.FileName +"</a></h3 > ";
    res += "            <div class=\"text-muted font-bold m-b-xs\">" + description + "</div>";
    res += "        </div > ";
    res += "        <div class=\"panel-footer contact-footer\">";
    res += "            <div class=\"row\">";
    res += "                <div class=\"col-md-6 border-right\" style=\"\"> <div class=\"contact-stat\"><span>Size: </span> <strong>12MB</strong></div> </div>";
    res += "                <div class=\"col-md-6\" style=\"\"> <div class=\"contact-stat\"><span>Created: </span> <strong>12/11/2017</strong></div> </div>";
    res += "            </div>";
    res += "        </div>";
    res += "    </div>";
    res += "</div > ";
    return res;
}

var files = [
    { "FileName": "hkjh kjh kjh k.xlsx", "FileExtension": "xlsx", "Description": "lkj l sñkldj añls kdlfa skdf sldkf sldñfk slñdkf lsadk fñlaskd flñasdkj lkj lk l l", "Size": 1234 },
    { "FileName": "un word.doc", "FileExtension": "doc", "Description": "", "Size": 1234 },
    { "FileName": "hkjh kjh kjh k.png", "FileExtension": "png", "Description": "lk", "Size": 1234 },
    { "FileName": "hkjh kjh kjh k.docx", "FileExtension": "docx", "Description": "lkj lkj lk ñlk ñl lkj lk l l", "Size": 1234 },
    { "FileName": "hkjh kjh kjh k.pdf", "FileExtension": "pdf", "Description": "lkj lkj lkj  jlkj lñkj lkñj ñlk jlk l l", "Size": 1234 },
    { "FileName": "hola.jpg", "FileExtension": "jpeg", "Description": "lkj lkj lkj lk l l", "Size": 1234 },
    { "FileName": "otro.mov", "FileExtension": "mov", "Description": "lkj lkj lkj lk l l", "Size": 1234 },
    { "FileName": "hkjh kjh kjh k.pptx", "FileExtension": "pptx", "Description": "lkj lkj lkj lk l l", "Size": 1234 },
]

/*var count = 0;
for (var x = 0; x < files.length; x++) {
    $("#test").append(ATTACHMENT_Render(files[x]));
    count++;
    if (count === 3) {
        cont = 0;
        $("#test").append("<div style=\"clear:both;\">&nbsp;</div>");
    }
}*/