var SaveMapsController = function () {

    var initialize = function (container) {

        $(container).jstree({
            plugins: ["contextmenu", "dnd"],
            contextmenu: {
                items: customMenu
            },
            core: {
                check_callback: true,
                data: [
                    { "id": "ajson1", "parent": "#", "text": "Simple root node" },
                    { "id": "ajson2", "parent": "#", "text": "Root node 2" },
                    { "id": "ajson3", "parent": "ajson2", "text": "Child 1" },
                    { "id": "ajson4", "parent": "ajson2", "text": "Child 2" },
                ]
            }
        });
    };

    function customMenu(node) {
        // The default set of all items
        var items = {
            addRouteItem: { // The "rename" menu item
                label: "New Route",
                action: function () {
                    console.log("Hello");
                }
            },
            deleteItem: { // The "delete" menu item
                label: "Delete",
                action: function () {
                    console.log("Delete");
                }
            }
        };

        //if ($(node).hasClass("folder")) {
        //    // Delete the "delete" menu item
        //    delete items.deleteItem;
        //}

        return items;
    }

    return {
        initialize: initialize
    };

}();