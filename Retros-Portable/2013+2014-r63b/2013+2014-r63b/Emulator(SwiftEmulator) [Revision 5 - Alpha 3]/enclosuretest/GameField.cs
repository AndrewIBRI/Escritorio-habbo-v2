namespace enclosuretest
{
    using Astar.Algorithm;
    using enclosuretest.Algorithm;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class GameField : IPathNode
    {
        private AStarSolver<GameField> astarSolver;
        private byte[,] currentField;
        private GametileUpdate currentlyChecking;
        private bool diagonal;
        private Queue<GametileUpdate> newEntries = new Queue<GametileUpdate>();

        public GameField(byte[,] theArray, bool diagonalAllowed)
        {
            this.currentField = theArray;
            this.diagonal = diagonalAllowed;
            this.astarSolver = new AStarSolver<GameField>(diagonalAllowed, AStarHeuristicType.EXPERIMENTAL_SEARCH, this, theArray.GetUpperBound(1) + 1, theArray.GetUpperBound(0) + 1);
        }

        public void destroy()
        {
            this.currentField = null;
        }

        public List<PointField> doUpdate(bool oneloop = false)
        {
            List<PointField> list = new List<PointField>();
            while (this.newEntries.Count > 0)
            {
                this.currentlyChecking = this.newEntries.Dequeue();
                List<Point> pointList = this.getConnectedItems(this.currentlyChecking);
                if (pointList.Count > 1)
                {
                    List<LinkedList<AStarSolver<GameField>.PathNode>> list3 = this.handleListOfConnectedPoints(pointList, this.currentlyChecking);
                    foreach (LinkedList<AStarSolver<GameField>.PathNode> list4 in list3)
                    {
                        if (list4.Count >= 4)
                        {
                            PointField item = this.findClosed(list4);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
                this.currentField[this.currentlyChecking.y, this.currentlyChecking.x] = this.currentlyChecking.value;
            }
            return list;
        }

        private PointField findClosed(LinkedList<AStarSolver<GameField>.PathNode> nodeList)
        {
            PointField field = new PointField(this.currentlyChecking.value);
            int num = 0x7fffffff;
            int num2 = -2147483648;
            int num3 = 0x7fffffff;
            int num4 = -2147483648;
            foreach (AStarSolver<GameField>.PathNode node in nodeList)
            {
                if (node.X < num)
                {
                    num = node.X;
                }
                if (node.X > num2)
                {
                    num2 = node.X;
                }
                if (node.Y < num3)
                {
                    num3 = node.Y;
                }
                if (node.Y > num4)
                {
                    num4 = node.Y;
                }
            }
            int x = ((int) Math.Ceiling((double) (((float) (num2 - num)) / 2f))) + num;
            int y = ((int) Math.Ceiling((double) (((float) (num4 - num3)) / 2f))) + num3;
            List<Point> list = new List<Point>();
            List<Point> list2 = new List<Point> {
                new Point(this.currentlyChecking.x, this.currentlyChecking.y),
                new Point(x, y)
            };
            while (list.Count > 0)
            {
                Point point2;
                Point p = list[0];
                int num7 = p.X;
                int num8 = p.Y;
                if (num7 < num)
                {
                    return null;
                }
                if (num7 > num2)
                {
                    return null;
                }
                if (num8 < num3)
                {
                    return null;
                }
                if (num8 > num4)
                {
                    return null;
                }
                if (this[num8 - 1, num7] && (this.currentField[num8 - 1, num7] == 0))
                {
                    point2 = new Point(num7, num8 - 1);
                    if (!(list.Contains(point2) || list2.Contains(point2)))
                    {
                        list.Add(point2);
                    }
                }
                if (this[num8 + 1, num7] && (this.currentField[num8 + 1, num7] == 0))
                {
                    point2 = new Point(num7, num8 + 1);
                    if (!(list.Contains(point2) || list2.Contains(point2)))
                    {
                        list.Add(point2);
                    }
                }
                if (this[num8, num7 - 1] && (this.currentField[num8, num7 - 1] == 0))
                {
                    point2 = new Point(num7 - 1, num8);
                    if (!(list.Contains(point2) || list2.Contains(point2)))
                    {
                        list.Add(point2);
                    }
                }
                if (this[num8, num7 + 1] && (this.currentField[num8, num7 + 1] == 0))
                {
                    point2 = new Point(num7 + 1, num8);
                    if (!(list.Contains(point2) || list2.Contains(point2)))
                    {
                        list.Add(point2);
                    }
                }
                if (this.getValue(p) == 0)
                {
                    field.add(p);
                }
                list2.Add(p);
                list.RemoveAt(0);
            }
            return field;
        }

        private List<Point> getConnectedItems(GametileUpdate update)
        {
            List<Point> list = new List<Point>();
            int x = update.x;
            int y = update.y;
            if (this.diagonal)
            {
                if (this[y - 1, x - 1] && (this.currentField[y - 1, x - 1] == update.value))
                {
                    list.Add(new Point(x - 1, y - 1));
                }
                if (this[y - 1, x + 1] && (this.currentField[y - 1, x + 1] == update.value))
                {
                    list.Add(new Point(x + 1, y - 1));
                }
                if (this[y + 1, x - 1] && (this.currentField[y + 1, x - 1] == update.value))
                {
                    list.Add(new Point(x - 1, y + 1));
                }
                if (this[y + 1, x + 1] && (this.currentField[y + 1, x + 1] == update.value))
                {
                    list.Add(new Point(x + 1, y + 1));
                }
            }
            if (this[y - 1, x] && (this.currentField[y - 1, x] == update.value))
            {
                list.Add(new Point(x, y - 1));
            }
            if (this[y + 1, x] && (this.currentField[y + 1, x] == update.value))
            {
                list.Add(new Point(x, y + 1));
            }
            if (this[y, x - 1] && (this.currentField[y, x - 1] == update.value))
            {
                list.Add(new Point(x - 1, y));
            }
            if (this[y, x + 1] && (this.currentField[y, x + 1] == update.value))
            {
                list.Add(new Point(x + 1, y));
            }
            return list;
        }

        public byte getValue(Point p)
        {
            if (this[p.Y, p.X])
            {
                return this.currentField[p.Y, p.X];
            }
            return 0;
        }

        public byte getValue(int x, int y)
        {
            if (this[y, x])
            {
                return this.currentField[y, x];
            }
            return 0;
        }

        private List<LinkedList<AStarSolver<GameField>.PathNode>> handleListOfConnectedPoints(List<Point> pointList, GametileUpdate update)
        {
            List<LinkedList<AStarSolver<GameField>.PathNode>> list = new List<LinkedList<AStarSolver<GameField>.PathNode>>();
            int num = 0;
            foreach (Point point in pointList)
            {
                num++;
                if (num == ((pointList.Count / 2) + 1))
                {
                    return list;
                }
                foreach (Point point2 in pointList)
                {
                    if (point != point2)
                    {
                        LinkedList<AStarSolver<GameField>.PathNode> item = this.astarSolver.Search(point2, point);
                        if (item != null)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public bool IsBlocked(int x, int y, bool lastTile)
        {
            return (((this.currentlyChecking.x == x) && (this.currentlyChecking.y == y)) || (this.getValue(x, y) != this.currentlyChecking.value));
        }

        private void setValue(int x, int y, byte value)
        {
            if (this[y, x])
            {
                this.currentField[y, x] = value;
            }
        }

        public void updateLocation(int x, int y, byte value)
        {
            this.newEntries.Enqueue(new GametileUpdate(x, y, value));
        }

        public bool this[int y, int x]
        {
            get
            {
                if ((y < 0) || (x < 0))
                {
                    return false;
                }
                if ((y > this.currentField.GetUpperBound(0)) || (x > this.currentField.GetUpperBound(1)))
                {
                    return false;
                }
                return true;
            }
        }
    }
}

