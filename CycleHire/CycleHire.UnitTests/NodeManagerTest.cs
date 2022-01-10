using CycleHire.Core;
using CycleHire.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleHire.UnitTests
{
    [TestClass]
    public class NodeManagerTest
    {
        [TestMethod]
        public void Flatten_SimpleHierarchy_CountReturns7()
        {
            //Arrange
            List<Node> hierarchy = new List<Node>
            {
                new FolderItem()
                {
                    Name = "A",
                    Children = new List<Node>()
                    {
                        new FolderItem(){ Name = "A.i" },
                        new FolderItem(){ Name = "A.ii" },
                        new FolderItem(){ Name = "A.iii"}
                    }
                }
            };

            hierarchy.AddRange(new List<FolderItem>()
            {
                new FolderItem(){ Name = "B" },
                new FolderItem(){ Name = "C" },
                new FolderItem(){ Name = "D" }
            });

            //Act
            var result = NodeManager.Flatten(hierarchy);

            //Assert
            Assert.IsTrue(result.ToList().Count == 7);

        }

        [TestMethod]
        public void Flatten_ComplexHierarchy_CountReturns12()
        {
            //Arrange
            List<Node> hierarchy = new List<Node>
            {
                new FolderItem()
                {
                    Name = "A",
                    Children = new List<Node>()
                    {
                        new FolderItem(){
                            Name = "A.i",
                            Children = new List<Node>()
                            {
                                new FolderItem(){ Name = "A.i.i"},
                                new FolderItem(){ Name = "A.i.ii"},
                                new FolderItem(){
                                    Name = "A.i.iii",
                                    Children = new List<Node>()
                                    {
                                        new RouteItem(){ Name = "FoobarA"},
                                        new RouteItem(){ Name = "FoobarB"},
                                    }
                                }
                            }
                        },
                        new FolderItem(){ Name = "A.ii" },
                        new FolderItem(){ Name = "A.iii"}
                    }
                }
            };

            hierarchy.AddRange(new List<FolderItem>()
            {
                new FolderItem(){ Name = "B" },
                new FolderItem(){ Name = "C" },
                new FolderItem(){ Name = "D" }
            });

            //Act
            var result = NodeManager.Flatten(hierarchy);

            //Assert
            Assert.IsTrue(result.ToList().Count == 12);
        }

        [TestMethod]
        public void Flatten_SimpleHierarchy_Returns7FoldersAnd2Routes()
        {
            //Arrange
            List<Node> hierarchy = new List<Node>
            {
                new FolderItem()
                {
                    Name = "A",
                    Children = new List<Node>()
                    {
                        new FolderItem(){ Name = "A.i"},
                        new FolderItem(){ Name = "A.ii" },
                        new FolderItem(){
                            Name = "A.iii",
                            Children = new List<Node>()
                            {
                                new RouteItem(){ Name = "Hi"},
                                new RouteItem(){ Name = "Bye"}
                            }
                        }
                    }
                }
            };

            hierarchy.AddRange(new List<FolderItem>()
            {
                new FolderItem(){ Name = "B" },
                new FolderItem(){ Name = "C" },
                new FolderItem(){ Name = "D" }
            });

            //Act
            var nodes = NodeManager.Flatten(hierarchy);
            var folders = nodes.OfType<FolderItem>();
            var routes = nodes.OfType<RouteItem>();

            //Assert
            Assert.IsTrue(folders.Count() == 7);
            Assert.IsTrue(routes.Count() == 2);
        }

        [TestMethod]
        public void BuildHierarchy_SimpleList_ReturnsCorrectHierarchy()
        {
            //Arrange
            var parent = new FolderItem()
            {
                Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482")
            };

            var child = new RouteItem()
            {
                Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                ParentId = parent.Id
            };

            List<Node> listHierarchy = new List<Node> { parent, child };

            //Act
            var result = NodeManager.BuildHierarchy(listHierarchy);

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Children.Count == 1);
        }

        [TestMethod]
        public void BuildHierarchy_ComplexList_ReturnsCorrectHierarchy()
        {
            //Arrange
            var root = new FolderItem()
            {
                Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481")
            };

            var children = new List<Node>()
            {
                new RouteItem
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                    ParentId = root.Id
                },
                new RouteItem
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247483"),
                    ParentId = root.Id
                },
                new RouteItem
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247484"),
                    ParentId = root.Id
                }
            };

            var secondLevelParent = new FolderItem
            {
                Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                ParentId = root.Id
            };

            var secondLevelParentChild = new RouteItem
            {
                Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247486"),
                ParentId = secondLevelParent.Id
            };

            List<Node> listHierarchy = new List<Node> { root, secondLevelParent, secondLevelParentChild };
            listHierarchy.AddRange(children);

            //Act
            var result = NodeManager.BuildHierarchy(listHierarchy);

            //Assert
            //Root has 1 parent with 4 children
            //1st children of root is a folder with 1 child
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Children.Count == 4);
            int firstChildChildrenCount = result[0].Children[0].Children.Count;
            Assert.IsTrue(firstChildChildrenCount == 1);
        }

        [TestMethod]
        public void Traverse_SimpleHierarchy_ReturnsFolderItem()
        {
            //Arrange
            List<Node> hierarchy = new List<Node>
            {
                new FolderItem()
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                    Name = "A",
                    Children = new List<Node>()
                    {
                        new FolderItem(){
                            Name = "A.i",
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481")
                        },
                        new FolderItem(){
                            Name = "A.ii",
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247483"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481")
                        },
                        new FolderItem(){
                            Name = "A.iii",
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247484"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481")
                        }
                    }
                }
            };

            hierarchy.AddRange(new List<FolderItem>()
            {
                new FolderItem(){
                    Name = "B",
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485")
                },
                new FolderItem(){
                    Name = "C",
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247486")
                },
                new FolderItem(){
                    Name = "D",
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247487")
                }
            });

            //Act
            var node = NodeManager.Traverse(hierarchy, Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247487"));

            //Assert
            Assert.IsTrue(node.Id == Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247487") && node.Name == "D");
        }

        [TestMethod]
        public void Traverse_ComplexHierarchy_ReturnsRouteItem()
        {
            //Arrange
            List<Node> hierarchy = new List<Node>
            {
                new FolderItem()
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                    Name = "A",
                    Children = new List<Node>()
                    {
                        new FolderItem(){
                            Name = "A.i",
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                            Children = new List<Node>()
                            {
                                new FolderItem()
                                {
                                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247483"),
                                    ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                                    Name = "A.i.i"
                                },
                                new FolderItem(){
                                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247484"),
                                    ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                                    Name = "A.i.ii"
                                },
                                new FolderItem(){
                                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                                    ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                                    Name = "A.i.iii",
                                    Children = new List<Node>()
                                    {
                                        new RouteItem(){
                                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247486"),
                                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                                            Name = "FoobarA"
                                        },
                                        new RouteItem(){
                                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247487"),
                                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                                            Name = "FoobarB"
                                        },
                                    }
                                }
                            }
                        },
                        new FolderItem(){
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247488"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                            Name = "A.ii"
                        },
                        new FolderItem(){
                            Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247489"),
                            ParentId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247481"),
                            Name = "A.iii"
                        }
                    }
                }
            };

            hierarchy.AddRange(new List<FolderItem>()
            {
                new FolderItem(){
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247490"),
                    Name = "B"
                },
                new FolderItem(){
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247491"),
                    Name = "C"
                },
                new FolderItem(){
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247492"),
                    Name = "D"
                }
            });

            //Act
            var result = NodeManager.Traverse(hierarchy, Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247486"));

            //Assert
            Assert.IsTrue(result.Name == "FoobarA");
            Assert.IsTrue(result.Id == Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247486"));
            Assert.IsTrue(result.ParentId == Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"));
        }

    }
}
