<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="CustomersFramework_AMPA_Pages_DashBoard" %>
<script>
    $("#MainPageTitle").html("<h1 class=\"page-title txt-color-blueDark\" style=\"font-size:18px;margin-top:-6px;\"><i class=\"fa fa-cog fa-spin\"></i>&nbsp;<strong>Càrrega de dades</strong></h1>");

    FK = new Array();
    if (typeof ClearAllUnsavedData !== "undefined") {
        ClearAllUnsavedData();
    }

    if (User.Core === true) {
        $("#statusTable").show();
    }

    var loaded = 0;
    var totalItems = 0;
    <%=this.FKScripts %>

    function LoadItem(itemName) {
        $.getJSON("/Data/FKScript.aspx?itemName=" + itemName + "&InstanceName=" + CustomerName + "&ac=<%=this.AntiCache%>&r=1", function (data) {
            console.log("loaded", itemName + " --> " + data.Data.length);
            $("#Count_" + itemName).html("&nbsp(" + data.Data.length + ")");
            UpdateLoading(itemName);
            FK[data.ItemName] = data.Data;
        });
    }

    function UpdateLoading(itemName) {
        loaded++;
        $("#pbar").css("width", (loaded * 100 / totalItems) + "%");
        $("#status").html("Loading... " + loaded + " / " + totalItems);
        $("#Item_" + itemName + "_Status").html("<i class=\"fa fa-check\" style=\"color:#3c3;\"></i>");
        if (loaded === totalItems) {
            $("#left-panel").show();
            $("#HeaderControls").show();
            GoEncryptedList("Noticia", "Custom");
            RefreshFK();
        }
    }

    $("#status").html("Loading... 0 / " + totalItems);

    for (var x = 0; x < totalItems; x++) {
        LoadItem(itemsToLoad[x]);
    }

    InstanceItems = itemsToLoad;
    //setTimeout(function () { GoEncryptedList('Adjudicacion', 'Custom'); ClearAllUnsavedData(); }, 1500);

</script>
<body>
    <div id="status"></div>
    <div style="col col-sm-12">
        <div class="progress">
            <div class="progress progress-striped">
                <div class="progress-bar bg-color-green" id="pbar" role="progressbar" style="width: 0%"></div>
            </div>
        </div>
    </div>
    <div class="row" id="statusTable" style="display:none;">
        <%=this.TableItems %>
    </div>
</body>