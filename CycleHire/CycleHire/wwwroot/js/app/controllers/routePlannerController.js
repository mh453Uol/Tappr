var RoutePlannerController = function (map, routeService) {

    var route = {
        origin: { latitude: 0, longitude: 0 },
        destination: { latitude: 0, longitude: 0 },
        waypoints: [],
        polyline: "",
        name: "",
        parent: 0
    };

    function onDirectionChanged(routeWithWaypoints) {
        route = routeWithWaypoints;
    }

    var initialize = function (container) {
        routeService.getItems(showMapsHierarchy);
        onRouteAdd("#add-route");
        onFolderAdd("#add-folder");
        onFolderEdit("#edit-folder");
        onNodeMove();
    };

    function onRouteAdd(form) {
        $(form).submit(function (e) {
            e.preventDefault();

            route.name = $("#js-route-name").val();
            route.parent = getSelectedNode()[0];

            routeService.addRoute(route, showAddedRoute);
        });
    }

    function onViewRouteHideInputs() {
        $("#right-panel").addClass("hidden")
        $("#js-start-end-inputs").addClass("hidden");
        $("#js-route-save").addClass("hidden");

        $("#js-route-name").prop("disabled", true);
        $("#js-route-name").removeClass("hidden");
    }

    function onViewRoute(id) {
        onViewRouteHideInputs();
        routeService.getRoute(id, showMapWithRoute);
    }

    function showAddedRoute(data) {
        onViewRouteHideInputs();
        addNodeToHierarchy(data.node);
        showMapWithRoute(data.route);
    }

    function showMapWithRoute(existingRoute) {
        route = existingRoute;

        $("#js-route-name").val(route.name);

        map.addMapWithExistingRoute("map", route);
    }

    function resetRoutePanel() {
        //Reset start and end input box
        $("#js-start").val("");
        $("#js-end").val("");
        $("#js-route-name").val("");
        $("#js-route-name").prop("disabled", false);
        $("#js-start-end-inputs").removeClass("hidden");
        $("#js-route-name").removeClass("hidden");
        $("#js-route-name").removeClass("hidden");
        $("#js-route-save").removeClass("hidden");
        $("#right-panel").removeClass("hidden")
        $("#right-panel").empty();
    }

    function showFolderModal(modal) {
        $(modal).modal("show");
        $(modal).modal("attach events", ".js-close-folder-modal", "hide");
    }

    function resetFolderModal() {
        $("#js-foldername").val("")
        $("#js-rootfolder-checkbox").checkbox("uncheck");
    }

    function onFolderAdd(form) {
        $(form).submit(function (e) {

            e.preventDefault();

            //close modal
            $("#js-add-folder-modal").modal("hide");

            var parentId = getSelectedNode();

            if ($("#js-rootfolder-checkbox").checkbox("is checked")) {
                parentId = "";
            }

            var folderName = $("#js-foldername").val();

            routeService.addFolder(parentId, folderName, addNodeToHierarchy)
        })
    }

    function onFolderEdit(form) {
        $(form).submit(function (e) {

            e.preventDefault();

            //close modal
            $("#js-edit-folder").modal("hide");

            var folderId = getSelectedNode()[0];

            var folderName = $("#js-edit-foldername").val()

            routeService.updateFolder(folderId, folderName,editNodeTextById)
        })
    }

    function onNodeDelete(node) {
        if (confirm("Are you sure you want to delete this item ?")) {
            routeService.deleteNode(node, deleteNode);
        }
    }

    function onNodeMove() {
        $("#jstree-hierarchy").bind("move_node.jstree", function (e, data) {

            if(confirm("Are you sure you want to move this item ?")) {
                var nodeId = data.node.id;
                var parentId = data.node.parent;
                console.log(nodeId, parentId);

                routeService.moveNode(nodeId,parentId);
            }
            else
            {
                //refresh/revert move by refreshing tree
                routeService.getItems(refreshTree);
            }
        });
    }

    function refreshTree(data) {
        $("#jstree-hierarchy").jstree(true).settings.core.data = data.map(setIconAndState);
        $("#jstree-hierarchy").jstree(true).refresh();
    }

    function showMapsHierarchy(nodes) {

        $("#jstree-hierarchy").jstree({
            core: {
                check_callback: function (operation, node, parent, position, more) {
                    if (operation == "move_node") {
                        // Only move if we are moving to a different folder
                        if (parent.id === node.parent) {
                            return false;
                        }

                        //// When creating a new root node has to be a folder
                        if (parent.id === "#" && node.original.type === "FolderItem") {
                            console.log("Creating a new root folder");
                            return true;
                        }

                        // Parent must only be a folder.
                        if (parent.original.type === "FolderItem") {
                            return true;
                        }

                        return false;
                    }
                },
                data: nodes.map(setIconAndState)
            },
            plugins: ["contextmenu", "dnd"],
            contextmenu: {
                items: customContextMenu
            },

        });

        if (!nodes.length) {
            //No root folders - show add root folder button
            $("#jstree-empty").removeClass("hidden");
            $("#jstree-empty").on("click", function () {
                showFolderModal("#js-add-folder-modal");
            });
        }
    }

    function addNodeToHierarchy(node) {
        node = setIconAndState(node);
        $("#jstree-hierarchy").jstree().create_node(node.parent, node, "last");

        //hide js-empty-tree button
        $("#jstree-empty").addClass("hidden");
    }

    function editNodeTextById(node) {
        var jsTreeNode = $("#jstree-hierarchy").jstree(true).get_node(node.id);
        $("#jstree-hierarchy").jstree("rename_node", jsTreeNode, node.text);
    }

    function deleteNode(node) {
        $("#jstree-hierarchy").jstree(true).delete_node(node);
    }

    function getSelectedNode() {
        var tree = $("#jstree-hierarchy").jstree(true);
        var bookmarks = tree.settings.core.data;
        var selected = tree.get_selected();

        return selected;
    }

    function setIconAndState(bookmark) {
        switch (bookmark.type) {
            case "RouteItem":
                bookmark.icon = "map icon";
                break;
        }

        bookmark.state = { opened: false, selected: false };

        return bookmark;
    }

    function customContextMenu(node) {
        // The default set of all items
        var items = {
            view: {
                label: "View",
                action: function () {
                    var id = getSelectedNode();
                    onViewRoute(id);
                }
            },
            create: {
                label: "Create",
                action: false,
                submenu: {
                    folder: {
                        label: "Folder",
                        action: false,
                        submenu: {
                            current: {
                                label: "In Selected Folder",
                                action: function () {
                                    resetFolderModal();
                                    showFolderModal("#js-add-folder-modal");
                                }
                            },
                            root: {
                                label: "Root Folder",
                                action: function () {
                                    resetFolderModal();
                                    showFolderModal("#js-add-folder-modal");
                                    $("#js-rootfolder-checkbox").checkbox("check");
                                }
                            }
                        }
                    },
                    items: {
                        label: "Item",
                        action: false,
                        submenu: {
                            link: {
                                label: "Route",
                                action: function () {
                                    resetRoutePanel();

                                    //add map 
                                    map.addMapWithDraggableDirections("map", "right-panel", "#js-start", "#js-end", onDirectionChanged);
                                }
                            }
                        }
                    }
                }
            },
            edit: {
                label: "Edit",
                action: function () {
                    //edit only available on folders
                    showFolderModal("#js-edit-folder");
                    //set edit folder modal folder name
                    $("#js-edit-foldername").val($(node)[0].text)
                }
            },
            delete: {
                label: "Delete",
                action: function () {
                    var jsTreeNode = $(node)[0].original;
                    onNodeDelete(jsTreeNode);
                }
            }
        };

        var node = $(node);

        var type = node[0].original.type;

        console.log(type);

        if (type == "FolderItem") {
            delete items.view;
        } else {
            delete items.create;
            delete items.edit;
        }

        return items;
    }

    return {
        initialize: initialize
    };

}(GoogleMapsController, RoutePlannerService);