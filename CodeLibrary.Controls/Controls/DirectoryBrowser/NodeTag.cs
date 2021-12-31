using System.Windows.Forms;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    internal class NodeTag
    {
        /// <summary>
        /// Placeholder for extended info
        /// </summary>
        private object Data { get; set; }

        /// <summary>
        /// Type for the node.
        /// </summary>
        private NodeType NodeType { get; set; }

        /// <summary>
        /// Returns the DirectoryBrowser's Node Data for a specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object GetNodeData(TreeNode node)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t != null)
            {
                return t.Data;
            }
            return null;
        }

        public static object GetNodeData(ListViewItem node)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t != null)
            {
                return t.Data;
            }
            return null;
        }

        /// <summary>
        /// Returns the DirectoryBrowser's NodeTag for a specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodeTag GetNodeTag(TreeNode node)
        {
            if (node.Tag == null)
            {
                return null;
            }
            if (node.Tag.GetType() == typeof(NodeTag))
            {
                NodeTag t = (NodeTag)node.Tag;
                return t;
            }
            return null;
        }

        public static NodeTag GetNodeTag(ListViewItem node)
        {
            if (node.Tag == null)
            {
                return null;
            }
            if (node.Tag.GetType() == typeof(NodeTag))
            {
                NodeTag t = (NodeTag)node.Tag;
                return t;
            }
            return null;
        }

        /// <summary>
        /// Returns the DirectoryBrowser's NodeType for a specified node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodeType GetNodeType(TreeNode node)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t != null)
            {
                return t.NodeType;
            }
            return NodeType.Unknown;
        }

        public static NodeType GetNodeType(ListViewItem node)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t != null)
            {
                return t.NodeType;
            }
            return NodeType.Unknown;
        }

        public static void SetNodeData(TreeNode node, object nodedata)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { Data = nodedata };
            }
            else
            {
                t.Data = nodedata;
            }
        }

        public static void SetNodeData(ListViewItem node, object nodedata)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { Data = nodedata };
            }
            else
            {
                t.Data = nodedata;
            }
        }

        public static void SetNodeTag(TreeNode node, NodeType nodetype, object nodedata)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { NodeType = nodetype, Data = nodedata };
            }
            else
            {
                t.Data = nodedata;
                t.NodeType = nodetype;
            }
        }

        public static void SetNodeTag(ListViewItem node, NodeType nodetype, object nodedata)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { NodeType = nodetype, Data = nodedata };
            }
            else
            {
                t.Data = nodedata;
                t.NodeType = nodetype;
            }
        }

        public static void SetNodeType(TreeNode node, NodeType nodetype)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { NodeType = nodetype, Data = null };
            }
            else
            {
                t.NodeType = nodetype;
            }
        }

        public static void SetNodeType(ListViewItem node, NodeType nodetype)
        {
            NodeTag t = NodeTag.GetNodeTag(node);
            if (t == null)
            {
                node.Tag = new NodeTag() { NodeType = nodetype, Data = null };
            }
            else
            {
                t.NodeType = nodetype;
            }
        }
    }
}