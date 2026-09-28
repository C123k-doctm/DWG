using System;
using System.Collections.Generic;
using Teigha.DatabaseServices;
using Teigha.Geometry;

namespace Teigha_Learn_Cad
{
    public static class DictionaryExtension
    {
        public static List<TKey> GetKeysByValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
        {
            List<TKey> keys = new List<TKey>();
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                {
                    keys.Add(pair.Key);
                }
            }
            return keys;
        }
    }
    struct bx_cir
    {
        //public MText txt;
        //public DBText dtxt;
        public TextWithCoordinates cirText;
        public Circle top;
        public Circle down;
    };
    struct bx_pl
    {
        //public MText txt;
        //public DBText dtxt;
        public TextWithCoordinates bxtext;
        public Teigha.DatabaseServices.Polyline pl;
        public Point3d top;
        public Point3d down;
    };
    struct Q_j
    {
        //public MText txt;
        public TextWithCoordinates txt;
        public Teigha.DatabaseServices.Polyline pl;
    };
    struct Myqj
    {
        public TextWithCoordinates txt;
        public Teigha.DatabaseServices.Polyline pl;
        public List<TextWithCoordinates> yj;
    };
    struct mqj
    {
        public TextWithCoordinates txt;
        public Line line;
    };
    struct my_line
    {
        public Line line;
        public bool visit;
    };
    struct myText
    {
        public string text;
        public Point3d position;
    }
    public class TextWithCoordinates
    {
        // 字符串属性，存储文本
        public string Text { get; set; }

        // Point3d 属性，存储坐标信息
        public Point3d Coordinates { get; set; }

        // 默认构造函数
        public TextWithCoordinates()
        {
            Text = string.Empty; // 默认值
            Coordinates = new Point3d(0, 0, 0); // 默认坐标
        }

        // 构造函数，用于初始化类实例
        public TextWithCoordinates(string text, Point3d coordinates)
        {
            Text = text;
            Coordinates = coordinates;
        }

        // 重写 ToString 方法，方便输出类的内容
        public override string ToString()
        {
            return $"Text: {Text}, Coordinates: ({Coordinates.X}, {Coordinates.Y}, {Coordinates.Z})";
        }
    }
    struct tuple
    {
        public Point3d start;
        public Point3d end;

    };
    struct tuple3
    {
        public Point3d start;
        public Point3d end;
        public int weight;
    }
    struct Poly
    {
        public Teigha.DatabaseServices.Polyline line;
        public bool visit;
    };
    public class BoTree<T>
    {
        public BoTree()
        {
            nodes = new List<BoTree<T>>();
        }

        public BoTree(T data)
        {
            this.Data = data;
            nodes = new List<BoTree<T>>();
        }

        private BoTree<T> parent;
        /// <summary>
        /// 父结点
        /// </summary>
        public BoTree<T> Parent
        {
            get { return parent; }
        }
        /// <summary>
        /// 结点数据
        /// </summary>
        public T Data { get; set; }

        private List<BoTree<T>> nodes;
        /// <summary>
        /// 子结点
        /// </summary>
        public List<BoTree<T>> Nodes
        {
            get { return nodes; }
        }
        /// <summary>
        /// 添加结点
        /// </summary>
        /// <param name="node">结点</param>
        public void AddNode(BoTree<T> node)
        {
            if (!nodes.Contains(node))
            {
                node.parent = this;
                nodes.Add(node);
            }
        }
        /// <summary>
        /// 添加结点
        /// </summary>
        /// <param name="nodes">结点集合</param>
        public void AddNode(List<BoTree<T>> nodes)
        {
            foreach (var node in nodes)
            {
                if (!nodes.Contains(node))
                {
                    node.parent = this;
                    nodes.Add(node);
                }
            }
        }
        public void AddNode(BoTree<T> node, int i)
        {
            if (!nodes.Contains(node))
            {
                node.parent = this;
                nodes.Insert(i, node);
            }
        }
        /// <summary>
        /// 移除结点
        /// </summary>
        /// <param name="node"></param>
        public void Remove(BoTree<T> node)
        {
            if (nodes.Contains(node))
                nodes.Remove(node);
        }
        /// <summary>
        /// 清空结点集合
        /// </summary>
        public void RemoveAll()
        {
            nodes.Clear();
        }
        private bool flag = false;
        public BoTree<T> Leaves()
        {
            BoTree<T> n = new BoTree<T>();
            if (nodes == null)
                return this;
            else
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].flag == false)
                    {
                        n = nodes[i];
                        nodes[i].flag = true;
                        break;
                    }
                }
                return n;
            }
        }
    }


    public struct Tuple
    {
        public Point3d Item1 { get; }
        public int Item2 { get; }

        public Tuple(Point3d item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    public struct Portsc
    {
        public Port start { get; }

        public Port end { get; }

        public int length { get; }

        public DBText jiecha { get; }
        public DBText lastjiecha { get; }
        public Portsc(Port item1, Port item2, int item3, DBText item4, DBText item5)
        {
            start = item2;
            end = item1;
            length = item3;
            jiecha = item4;
            lastjiecha = item5;
        }
    }
    public struct Port_jc
    {
        public Port pot { get; }

        public DBText jc { get; }

        public Point3d vet { get; }
        public Port_jc(DBText item1, Port item2, Point3d item3)
        {
            pot = item2;
            jc = item1;
            vet = item3;
        }
    }
    public struct Port
    {
        public DBText XS { get; }
        public DBText port { get; }
        public DBText color { get; }
        public DBText width { get; }
        public Port(DBText item1, DBText item2, DBText item3, DBText item4)
        {
            XS = item1;
            port = item2;
            color = item3;
            width = item4;
        }
    }
    class AllPort
    {
        public List<Port> add;
        public AllPort()
        {
            add = new List<Port>();
        }
        public void Add(Port pp)
        {
            add.Add(pp);
        }
    }
    class WeightedUndirectedGraph
    {
        private Dictionary<Point3d, List<Tuple>> adjacencyList;

        public WeightedUndirectedGraph()
        {
            adjacencyList = new Dictionary<Point3d, List<Tuple>>();
        }

        public void AddEdge(Point3d vertex1, Point3d vertex2, int weight)
        {
            if (!adjacencyList.ContainsKey(vertex1))
            {
                adjacencyList[vertex1] = new List<Tuple>();
            }
            if (!adjacencyList.ContainsKey(vertex2))
            {
                adjacencyList[vertex2] = new List<Tuple>();
            }
            adjacencyList[vertex1].Add(new Tuple(vertex2, weight));
            adjacencyList[vertex2].Add(new Tuple(vertex1, weight)); // since it's an undirected graph
        }

        public List<Tuple> GetNeighbors(Point3d vertex)
        {
            return adjacencyList[vertex];
        }
        public int DFS(Point3d from, Point3d to)
        {
            //List<Tuple> neighbors = adjacencyList[from];
            Dictionary<Point3d, int> dis = new Dictionary<Point3d, int>();
            Dictionary<Point3d, bool> visited = new Dictionary<Point3d, bool>();
            //bool[] visited = new bool[adjacencyList.Count];
            Stack<Point3d> s = new Stack<Point3d>();
            s.Push(from);
            dis[from] = 0;
            while (s.Count != 0)
            {
                Point3d temp = s.Pop();
                if (temp == to)
                    return dis[temp];
                List<Tuple> neighbors = adjacencyList[temp];
                visited[temp] = true;

                for (int i = 0; i < neighbors.Count; i++)
                {
                    Point3d nextnode = neighbors[i].Item1;
                    if (!visited.ContainsKey(nextnode))
                    {
                        s.Push(nextnode);
                        dis[nextnode] = dis[temp] + neighbors[i].Item2;
                    }
                }
            }
            return -1;
        }
        public List<Point3d> DFSWithPath(Point3d from, Point3d to)
        {
            Dictionary<Point3d, int> dis = new Dictionary<Point3d, int>();
            Dictionary<Point3d, bool> visited = new Dictionary<Point3d, bool>();
            Dictionary<Point3d, Point3d> parent = new Dictionary<Point3d, Point3d>();
            Stack<Point3d> s = new Stack<Point3d>();
            s.Push(from);
            dis[from] = 0;
            while (s.Count != 0)
            {
                Point3d temp = s.Pop();
                if (temp == to)
                {
                    List<Point3d> path = new List<Point3d>();
                    Point3d current = temp;
                    while (current != from)
                    {
                        path.Add(current);
                        current = parent[current];
                    }
                    path.Add(from);
                    path.Reverse(); // Reverse the path to get it in correct order
                    return path;
                }
                List<Tuple> neighbors = adjacencyList[temp];
                visited[temp] = true;

                for (int i = 0; i < neighbors.Count; i++)
                {
                    Point3d nextnode = neighbors[i].Item1;
                    if (!visited.ContainsKey(nextnode))
                    {
                        s.Push(nextnode);
                        dis[nextnode] = dis[temp] + neighbors[i].Item2;
                        parent[nextnode] = temp; // Record the parent node for each neighbor
                    }
                }
            }
            return new List<Point3d>(); // Return an empty list if no path is found
        }
        public List<Point3d> LDFS(Point3d from, Point3d to)
        {
            List<Point3d> ve = new List<Point3d>();
            Dictionary<Point3d, bool> visited = new Dictionary<Point3d, bool>();
            //bool[] visited = new bool[adjacencyList.Count];
            Stack<Point3d> s = new Stack<Point3d>();
            s.Push(from);
            ve.Add(from);
            while (s.Count != 0)
            {
                Point3d temp = s.Pop();
                if (temp == to)
                    return ve;
                List<Tuple> neighbors = adjacencyList[temp];
                visited[temp] = true;

                for (int i = 0; i < neighbors.Count; i++)
                {
                    Point3d nextnode = neighbors[i].Item1;
                    if (!visited.ContainsKey(nextnode))
                    {
                        s.Push(nextnode);
                        ve.Add(nextnode);
                    }
                }
            }
            return new List<Point3d>();
        }
        public List<Point3d> RE()
        {
            List<Point3d> vetex = new List<Point3d>();
            foreach (Point3d i in adjacencyList.Keys)
            {
                if (adjacencyList[i].Count == 1)
                    vetex.Add(i);
            }
            return vetex;
        }

        public Point3d Neighbor(Point3d op)
        {
            List<Tuple> opp = adjacencyList[op];

            return opp[0].Item1;
        }
    }
}


