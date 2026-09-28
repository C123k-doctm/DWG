using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.DatabaseServices;
using Teigha.Geometry;

class QuadTreeLeaf<T>
{
    private Point2d pos;
    private T refObject;

    public QuadTreeLeaf(Point2d pos, T obj)
    {
        this.pos = pos;
        refObject = obj;
    }

    public T LeafObject
    {
        get
        {
            return refObject;
        }
    }

    public Point2d Pos
    {
        get { return pos; }
        set { pos = value; }
    }
}
/// <summary>
/// 四叉树节点
/// </summary>
/// <typeparam name="T"></typeparam>
class QuadTreeNode<T>
{
    /// <summary>
    /// 节点拥有的叶子节点
    /// </summary>
    protected List<QuadTreeLeaf<T>> items;
    /// <summary>
    /// 节点拥有的分支
    /// </summary>
    protected QuadTreeNode<T>[] branch;
    /// <summary>
    /// 节点空间最大容量，受minSize影响
    /// </summary>
    protected int maxItems;
    /// <summary>
    /// 节点空间分割的最小大小（最小宽度，高度）
    /// </summary>
    protected double minSize;

    public const double TOLERANCE = 0.00001f;
    /// <summary>
    /// 节点的空间
    /// </summary>
    public Rect bounds;
    public Extents3d ext;

    public QuadTreeNode(double x, double y, double width, double height, int maximumItems, double minSize = -1)
    {
        this.ext = new Extents3d(new Point3d(x, y, 0), new Point3d(x + width, y + height, 0));
        bounds = new Rect(x, y, width, height);
        maxItems = maximumItems;
        this.minSize = minSize;
        items = new List<QuadTreeLeaf<T>>();
    }
    public QuadTreeNode(Extents3d ext, int maximumItems, double minSize = -1)
    {
        this.ext = ext;
        bounds = new Rect(ext.MinPoint.X, ext.MinPoint.Y, ext.MaxPoint.X - ext.MinPoint.X, ext.MaxPoint.Y - ext.MinPoint.Y);
        maxItems = maximumItems;
        this.minSize = minSize;
        items = new List<QuadTreeLeaf<T>>();
    }


    public bool HasChildren()
    {
        if (branch != null)
            return true;
        else
            return false;
    }
    protected bool CanSplit()
    {
        if (this.bounds.width <= this.minSize && this.bounds.height <= this.minSize)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 将节点空间分割4份
    /// </summary>
    protected void Split()
    {
        if (!this.CanSplit()) return;


        double nsHalf = bounds.height - bounds.height / 2;
        double ewHalf = bounds.width - bounds.width / 2;

        branch = new QuadTreeNode<T>[4];

        branch[0] = new QuadTreeNode<T>(bounds.x, bounds.y, ewHalf, nsHalf, maxItems, minSize);
        branch[1] = new QuadTreeNode<T>(bounds.x + ewHalf, bounds.y, ewHalf, nsHalf, maxItems, minSize);
        branch[2] = new QuadTreeNode<T>(bounds.x, bounds.y + nsHalf, ewHalf, nsHalf, maxItems, minSize);
        branch[3] = new QuadTreeNode<T>(bounds.x + ewHalf, bounds.y + nsHalf, ewHalf, nsHalf, maxItems, minSize);




        foreach (var item in items)
        {
            AddNode(item);
        }

        items.Clear();
    }

    /// <summary>
    /// 根据坐标获得相应的子空间
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    protected QuadTreeNode<T> GetChild(Point2d pos)
    {
        if (bounds.Contains(pos))
        {
            if (branch != null)
            {
                for (int i = 0; i < branch.Length; i++)
                    if (branch[i].bounds.Contains(pos))
                        return branch[i].GetChild(pos);

            }
            else
                return this;
        }
        return null;
    }
    /// <summary>
    /// 增加叶子节点数据
    /// </summary>
    /// <param name="leaf"></param>
    /// <returns></returns>
    private bool AddNode(QuadTreeLeaf<T> leaf)
    {
        if (branch == null)
        {
            this.items.Add(leaf);

            if (this.items.Count > this.maxItems)
            {
                Split();
            }
            return true;
        }
        else
        {
            QuadTreeNode<T> node = GetChild(leaf.Pos);
            if (node != null)
            {
                return node.AddNode(leaf);
            }
        }
        return false;
    }

    public bool AddNode(Point2d pos, T obj)
    {
        return AddNode(new QuadTreeLeaf<T>(pos, obj));
    }
    public bool AddNode(Point3d pos, T obj)
    {
        return AddNode(new QuadTreeLeaf<T>(new Point2d(pos.X, pos.Y), obj));
    }

    /// <summary>
    /// 可以是空间任意位置，只是根据这个位置找到所在的空间去删除对象
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool RemoveNode(Point3d pt, T obj)
    {
        Point2d pos = pt.Convert2d(new Plane());
        if (branch == null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                QuadTreeLeaf<T> qtl = items[i];
                if (qtl.LeafObject.Equals(obj))
                {
                    items.RemoveAt(i);
                    return true;
                }
            }
        }
        else
        {
            QuadTreeNode<T> node = GetChild(pos);
            if (node != null)
            {
                return node.RemoveNode(pt, obj);
            }
        }
        return false;
    }

    public bool UpdateNode(Point2d pos, T obj)
    {
        // TODO  参找RemoveNode
        return false;
    }

    /// <summary>
    /// 得到在一个Rect内的所有数据
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public int GetNode(Rect rect, ref List<T> nodes)
    {
        if (branch == null)
        {
            foreach (QuadTreeLeaf<T> item in items)
            {
                if (rect.Contains(item.Pos))
                {
                    nodes.Add(item.LeafObject);
                }
            }
        }
        else
        {
            for (int i = 0; i < branch.Length; i++)
            {
                if (branch[i].bounds.Overlaps(rect))
                    branch[i].GetNode(rect, ref nodes);
            }
        }
        return nodes.Count;
    }
    public int GetNode(Extents3d ext, ref List<T> nodes)
    {
        Point2d p0 = new Point2d(ext.MinPoint.X, ext.MinPoint.Y);
        Vector3d vt = ext.MaxPoint - ext.MinPoint;
        Rect rect = new Rect(p0, new Point2d(vt.X, vt.Y));
        if (branch == null)
        {
            foreach (QuadTreeLeaf<T> item in items)
            {
                if (rect.Contains(item.Pos))
                {
                    nodes.Add(item.LeafObject);
                }
            }
        }
        else
        {
            for (int i = 0; i < branch.Length; i++)
            {
                if (branch[i].bounds.Overlaps(rect))
                    branch[i].GetNode(rect, ref nodes);
            }
        }
        return nodes.Count;
    }
    /// <summary>
    /// 根据坐标得到坐标附近节点的数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="ShortestDistance">离坐标最短距离</param>
    /// <param name="list"></param>
    /// <returns></returns>
    public int GetNodeRecRange2d(Point2d pos, double ShortestDistance, ref List<T> list)
    {
        double distance;
        if (branch == null)
        {
            foreach (QuadTreeLeaf<T> leaf in this.items)
            {
                distance = (pos - leaf.Pos).Length;

                if (distance < ShortestDistance)
                {
                    list.Add(leaf.LeafObject);
                }
            }
        }
        else
        {
            for (int i = 0; i < branch.Length; i++)
            {
                double childDistance = branch[i].bounds.PointToBorderDistance(pos);
                if (childDistance < ShortestDistance)
                {
                    branch[i].GetNodeRecRange2d(pos, ShortestDistance, ref list);
                }
            }
        }
        return list.Count;
    }


    public int GetNodeRecRange(Point3d pos, double ShortestDistance, ref List<T> list)
    {
        Point2d pos0 = new Point2d(pos.X, pos.Y);
        return GetNodeRecRange2d(pos0, ShortestDistance, ref list);
    }
    public List<T> GetNodeRecRange(Point3d pos, double ShortestDistance)
    {
        Point2d pos0 = new Point2d(pos.X, pos.Y);
        List<T> list = new List<T>();

        int n = GetNodeRecRange2d(pos0, ShortestDistance, ref list);
        return list;
    }
}


struct Rect : IEquatable<Rect>
{
    private double m_XMin;
    private double m_YMin;
    private double m_Width;
    private double m_Height;

    public static Rect zero
    {
        get
        {
            return new Rect(0f, 0f, 0f, 0f);
        }
    }

    public double x
    {
        get
        {
            return m_XMin;
        }
        set
        {
            m_XMin = value;
        }
    }

    public double y
    {
        get
        {
            return m_YMin;
        }
        set
        {
            m_YMin = value;
        }
    }

    public Point2d position
    {
        get
        {
            return new Point2d(m_XMin, m_YMin);
        }
        set
        {
            m_XMin = value.X;
            m_YMin = value.Y;
        }
    }

    public Point2d center
    {
        get
        {
            return new Point2d(x + m_Width / 2f, y + m_Height / 2f);
        }
        set
        {
            m_XMin = value.X - m_Width / 2f;
            m_YMin = value.Y - m_Height / 2f;
        }
    }

    public Point2d min
    {
        get
        {
            return new Point2d(xMin, yMin);
        }
        set
        {
            xMin = value.X;
            yMin = value.Y;
        }
    }

    public Point2d max
    {
        get
        {
            return new Point2d(xMax, yMax);
        }
        set
        {
            xMax = value.X;
            yMax = value.Y;
        }
    }

    public double width
    {
        get
        {
            return m_Width;
        }
        set
        {
            m_Width = value;
        }
    }

    public double height
    {
        get
        {
            return m_Height;
        }
        set
        {
            m_Height = value;
        }
    }

    public Point2d size
    {
        get
        {
            return new Point2d(m_Width, m_Height);
        }
        set
        {
            m_Width = value.X;
            m_Height = value.Y;
        }
    }

    public double xMin
    {
        get
        {
            return m_XMin;
        }
        set
        {
            double xMax = this.xMax;
            m_XMin = value;
            m_Width = xMax - m_XMin;
        }
    }

    public double yMin
    {
        get
        {
            return m_YMin;
        }
        set
        {
            double yMax = this.yMax;
            m_YMin = value;
            m_Height = yMax - m_YMin;
        }
    }

    public double xMax
    {
        get
        {
            return m_Width + m_XMin;
        }
        set
        {
            m_Width = value - m_XMin;
        }
    }

    public double yMax
    {
        get
        {
            return m_Height + m_YMin;
        }
        set
        {
            m_Height = value - m_YMin;
        }
    }

    public double left => m_XMin;

    public double right => m_XMin + m_Width;

    public double top => m_YMin;

    public double bottom => m_YMin + m_Height;

    public Rect(double x, double y, double width, double height)
    {
        m_XMin = x;
        m_YMin = y;
        m_Width = width;
        m_Height = height;
    }

    public Rect(Point2d position, Point2d size)
    {
        m_XMin = position.X;
        m_YMin = position.Y;
        m_Width = size.X;
        m_Height = size.Y;
    }

    public Rect(Rect source)
    {
        m_XMin = source.m_XMin;
        m_YMin = source.m_YMin;
        m_Width = source.m_Width;
        m_Height = source.m_Height;
    }

    public static Rect MinMaxRect(double xmin, double ymin, double xmax, double ymax)
    {
        return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
    }

    public void Set(double x, double y, double width, double height)
    {
        m_XMin = x;
        m_YMin = y;
        m_Width = width;
        m_Height = height;
    }

    public bool Contains(Point2d point)
    {
        return point.X >= xMin && point.X < xMax && point.Y >= yMin && point.Y < yMax;
    }

    public bool Contains(Point3d point)
    {
        return point.X >= xMin && point.X < xMax && point.X >= yMin && point.Y < yMax;
    }

    public bool Contains(Point3d point, bool allowInverse)
    {
        if (!allowInverse)
        {
            return Contains(point);
        }

        bool flag = false;
        if ((width < 0f && point.X <= xMin && point.X > xMax) || (width >= 0f && point.X >= xMin && point.X < xMax))
        {
            flag = true;
        }

        if (flag && ((height < 0f && point.Y <= yMin && point.Y > yMax) || (height >= 0f && point.Y >= yMin && point.Y < yMax)))
        {
            return true;
        }

        return false;
    }

    private static Rect OrderMinMax(Rect rect)
    {
        if (rect.xMin > rect.xMax)
        {
            double xMin = rect.xMin;
            rect.xMin = rect.xMax;
            rect.xMax = xMin;
        }

        if (rect.yMin > rect.yMax)
        {
            double yMin = rect.yMin;
            rect.yMin = rect.yMax;
            rect.yMax = yMin;
        }

        return rect;
    }

    public bool Overlaps(Rect other)
    {
        return other.xMax > xMin && other.xMin < xMax && other.yMax > yMin && other.yMin < yMax;
    }

    public bool Overlaps(Rect other, bool allowInverse)
    {
        Rect rect = this;
        if (allowInverse)
        {
            rect = OrderMinMax(rect);
            other = OrderMinMax(other);
        }

        return rect.Overlaps(other);
    }

    public static Point2d NormalizedToPoint(Rect rectangle, Point2d normalizedRectCoordinates)
    {
        return new Point2d(Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.X), Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.Y));
    }

    public static Point2d PointToNormalized(Rect rectangle, Point2d point)
    {
        return new Point2d(InverseLerp(rectangle.x, rectangle.xMax, point.X), InverseLerp(rectangle.y, rectangle.yMax, point.Y));
    }

    public static bool operator !=(Rect lhs, Rect rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator ==(Rect lhs, Rect rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);
    }

    public override bool Equals(object other)
    {
        if (!(other is Rect))
        {
            return false;
        }

        return Equals((Rect)other);
    }

    public bool Equals(Rect other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
    }

    public override string ToString()
    {
        return String.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", x, y, width, height);
    }

    public string ToString(string format)
    {
        return String.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format), y.ToString(format), width.ToString(format), height.ToString(format));
    }

    public static double InverseLerp(double a, double b, double value)
    {
        if (a != b)
        {
            return Clamp01((value - a) / (b - a));
        }

        return 0f;
    }
    public static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * Clamp01(t);
    }

    public static double Clamp01(double value)
    {
        if (value < 0)
        {
            return 0;
        }

        if (value > 1)
        {
            return 1;
        }

        return value;
    }
}


/// <summary>
/// Rect的扩展方法类
/// </summary>
static class RectExtension
{
    /// <summary>
    /// 计算点到Rect的border的距离，若点在Rect内则返回0
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static double PointToBorderDistance(this Rect rect, Point2d pos)
    {
        double xdisance;
        double ydisance;

        if (rect.x <= pos.X && pos.X <= rect.x + rect.width)
        {
            xdisance = 0;
        }
        else
        {
            xdisance = Math.Min((Math.Abs(pos.X - (rect.width + rect.x))), (rect.x - pos.X));
        }

        if (rect.y <= pos.Y && pos.Y <= rect.y + rect.height)
        {
            ydisance = 0;
        }
        else
        {
            ydisance = Math.Min((Math.Abs(pos.Y - (rect.height + rect.y))), (rect.y - pos.Y));
        }

        return Math.Max(xdisance, ydisance);
    }
}


