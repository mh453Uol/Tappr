var RoutePlannerService = function () {

    function getRouteById(id, onSuccess, onError) {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: "/api/routeplanner/route/"+id,
            success: function (data) {
                onSuccess(data);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function onRouteSave(route_data,onSuccess) {

        console.log(JSON.stringify(route_data));

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "api/routeplanner/route/create",
            data: JSON.stringify(route_data),
            success: function (data) {
                onSuccess(data);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function addFolder(parentId,folderName,onSuccess) {

        console.log(parentId,folderName);

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "api/routeplanner/folder/create?parentId=" + parentId + "&folderName=" + folderName,
            success: function (data) {
                onSuccess(data);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function updateFolder(id, folderName, onSuccess) {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "api/routeplanner/folder/edit?id=" + id + "&folderName=" + folderName,
            success: function (data) {
                onSuccess(data);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function getItems(onSuccess) {
        $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType:"json",
            url: "api/routeplanner",
            success: function (data) {
                onSuccess(data);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function deleteNode(node,onSuccess) {
        $.ajax({
            type: "DELETE",
            url: "api/routeplanner/node/"+node.id,
            success: function () {
                console.log("Success");
                onSuccess(node);
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    function moveNode(nodeId, parentId) {
        $.ajax({
            type: "POST",
            url: "api/routeplanner/node/move?Id=" + nodeId+"&parentId="+parentId,
            success: function () {
                console.log("Success");
            },
            error: function () {
                console.log("Error");
            }
        });
    }

    return {
        getRoute: getRouteById,
        addRoute: onRouteSave,
        addFolder: addFolder,
        getItems: getItems,
        updateFolder: updateFolder,
        deleteNode: deleteNode,
        moveNode: moveNode
    };

}();