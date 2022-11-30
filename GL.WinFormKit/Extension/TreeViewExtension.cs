using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    public static class TreeViewExtension
    {
        /// <summary>
        /// 上移节点
        /// </summary>
        public static void MoveUpNode(this TreeView treeView, TreeNode moveNode)
        {
            if (moveNode.TreeView != treeView)
                throw new Exception("要移动的 TreeNode 不属于当前 TreeView");

            int selfIndex = moveNode.Index;
            if (selfIndex > 0)
            {
                if (moveNode.Level == 0)
                {
                    moveNode.Remove();
                    treeView.Nodes.Insert(selfIndex - 1, moveNode);
                }
                else
                {
                    TreeNode parent = moveNode.Parent;
                    moveNode.Remove();
                    parent.Nodes.Insert(selfIndex - 1, moveNode);
                }

                treeView.SelectedNode = moveNode;
            }
        }

        /// <summary>
        /// 下移节点
        /// </summary>
        public static void MoveDownNode(this TreeView treeView, TreeNode moveNode)
        {
            if (moveNode.TreeView != treeView)
                throw new Exception("要移动的 TreeNode 不属于当前 TreeView");

            int selfIndex = moveNode.Index;
            int count = moveNode.Parent?.Nodes.Count ?? treeView.Nodes.Count;
            if (selfIndex < count - 1)
            {
                if (moveNode.Level == 0)
                {
                    moveNode.Remove();
                    treeView.Nodes.Insert(selfIndex + 1, moveNode);
                }
                else
                {
                    TreeNode parent = moveNode.Parent;
                    moveNode.Remove();
                    parent.Nodes.Insert(selfIndex + 1, moveNode);
                }

                treeView.SelectedNode = moveNode;
            }
        }

        public static void MoveNode(this TreeNodeCollection nodes, int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= nodes.Count) throw new IndexOutOfRangeException();
            if (toIndex < 0 || toIndex >= nodes.Count) throw new IndexOutOfRangeException();

            if (fromIndex == toIndex) return;

            TreeNode node = nodes[fromIndex];

            nodes.RemoveAt(fromIndex);

            nodes.Insert(toIndex, node);

            node.TreeView.SelectedNode = node;
        }

        public static TreeNode FindNode(this TreeView treeView, bool searchAllChildren, Func<TreeNode, bool> predicate)
        {
            return treeView.Nodes.FirstOrDefault(searchAllChildren, predicate);
        }

        public static TreeNode FirstOrDefault(this TreeNodeCollection nodeCollection, bool searchAllChildren, Func<TreeNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TreeNode node in nodeCollection)
            {
                if (predicate(node))
                {
                    return node;
                }
            }

            if (searchAllChildren)
            {
                foreach (TreeNode node in nodeCollection)
                {
                    TreeNode n = FirstOrDefault(node.Nodes, searchAllChildren, predicate);
                    if (n != null)
                        return n;
                }
            }

            return null;
        }

        public static List<TreeNode> FindNodes(this TreeView treeView, bool searchAllChildren, Func<TreeNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FindNodes(treeView.Nodes, searchAllChildren, predicate);
        }

        public static List<TreeNode> FindNodes(this TreeNodeCollection nodeCollection, bool searchAllChildren, Func<TreeNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            List<TreeNode> nodes = new List<TreeNode>();

            foreach (TreeNode node in nodeCollection)
            {
                if (predicate(node))
                {
                    nodes.Add(node);
                }
            }

            if (searchAllChildren)
            {
                foreach (TreeNode node in nodeCollection)
                {
                    List<TreeNode> ns = FindNodes(node.Nodes, searchAllChildren, predicate);
                    if (ns != null && ns.Count > 0)
                    {
                        nodes.AddRange(ns);
                    }
                }
            }

            return nodes;
        }

        public static TreeNode RemoveFirst(this TreeNodeCollection nodeCollection, Func<TreeNode, bool> predicate)
        {
            TreeNode node = FirstOrDefault(nodeCollection, false, predicate);

            if (node != null)
                nodeCollection.Remove(node);

            return node;
        }

        public static int GetNewNameIndex(this TreeView treeView, string name, int level, Func<string, string> func = null)
        {
            return getNewNameIndex(treeView.Nodes, name, level, func);
        }

        static int getNewNameIndex(TreeNodeCollection nodeCollection, string name, int level, Func<string, string> func)
        {
            if (nodeCollection == null || nodeCollection.Count == 0) return 0;

            int curLevel = nodeCollection[0].Level;

            int max = 0;
            if (curLevel < level)
            {
                foreach (TreeNode node in nodeCollection)
                {
                    int index = getNewNameIndex(node.Nodes, name, level, func);
                    if (index > max)
                        max = index;
                }
            }
            else
            {
                foreach (TreeNode node in nodeCollection)
                {
                    string s = func == null ? node.Text : func(node.Text);

                    Match match = Regex.Match(s, $@"^{name}(\d+)$");
                    if (match.Success)
                    {
                        int index = int.Parse(match.Groups[1].Value);
                        if (index > max)
                            max = index;
                    }
                }
            }
            return max;
        }
    }
}
